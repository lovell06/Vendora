// Pointer-based reordering for the product image grid (mouse, pen and touch).
// The lifted tile follows the pointer and the other tiles slide to show where it will land.
// On release the new order is sent to .NET; when Blazor moves the DOM nodes, the temporary
// transforms are cleared in the same frame (MutationObserver), so nothing jumps.
//
// Mouse / pen: drag after moving 5px. Touch: press and hold ~220ms (a quick swipe still scrolls),
// or drag the handle right away. Esc cancels. Buttons inside a tile keep working as clicks.

const HOLD_MS = 220;
const MOVE_TOLERANCE = 8;
const MOUSE_THRESHOLD = 5;
const EDGE = 64;

export function attach(grid, dotNet) {
    if (!grid || grid.__sortable) return;
    const state = { pending: null, drag: null, settling: false };

    const items = () => [...grid.querySelectorAll(':scope > .pip-item')];

    const clearStyles = () => {
        for (const el of items()) {
            el.style.transition = 'none';
            el.style.transform = '';
            el.classList.remove('is-lifted');
            void el.offsetWidth; // flush so the next style change animates again
            el.style.transition = '';
        }
        grid.classList.remove('is-sorting');
    };

    // Blazor moved the keyed <li> nodes: the real order now matches what was shown.
    const observer = new MutationObserver(() => {
        if (!state.settling) return;
        state.settling = false;
        clearStyles();
    });
    observer.observe(grid, { childList: true });

    const begin = (p, x, y) => {
        const list = items();
        const from = list.indexOf(p.item);
        if (from < 0) return;
        state.pending = null;
        state.drag = {
            id: p.id, item: p.item, from, to: from, list,
            rects: list.map(el => el.getBoundingClientRect()),
            startX: x, startY: y, startScroll: window.scrollY
        };
        try { p.item.setPointerCapture(p.id); } catch { /* pointer already gone */ }
        p.item.classList.add('is-lifted');
        grid.classList.add('is-sorting');
        if (p.type === 'touch') navigator.vibrate?.(10);
    };

    const layout = to => {
        const d = state.drag;
        d.to = to;
        d.list.forEach((el, i) => {
            if (i === d.from) return;
            let target = i;
            if (d.from < to && i > d.from && i <= to) target = i - 1;
            else if (d.from > to && i >= to && i < d.from) target = i + 1;
            const a = d.rects[i], b = d.rects[target];
            el.style.transform = target === i ? '' : `translate(${b.left - a.left}px, ${b.top - a.top}px)`;
        });
    };

    const update = (x, y) => {
        const d = state.drag;
        const dx = x - d.startX;
        const dy = y - d.startY + (window.scrollY - d.startScroll);
        d.item.style.transform = `translate(${dx}px, ${dy}px) scale(1.04)`;

        // Target = slot whose centre is closest to the centre of the lifted tile.
        const o = d.rects[d.from];
        const cx = o.left + o.width / 2 + dx, cy = o.top + o.height / 2 + dy - (window.scrollY - d.startScroll);
        let best = d.to, bestDist = Infinity;
        d.rects.forEach((r, i) => {
            const dist = Math.hypot(r.left + r.width / 2 - cx, r.top + r.height / 2 - cy);
            if (dist < bestDist) { bestDist = dist; best = i; }
        });
        if (best !== d.to) layout(best);

        if (y < EDGE) window.scrollBy(0, -12);
        else if (y > window.innerHeight - EDGE) window.scrollBy(0, 12);
    };

    const finish = commit => {
        const d = state.drag;
        state.drag = null;
        if (!d) return;
        try { d.item.releasePointerCapture(d.id); } catch { /* ignore */ }
        if (commit && d.to !== d.from) {
            // Glide into the target slot, then let .NET apply the order.
            const a = d.rects[d.from], b = d.rects[d.to];
            const sy = window.scrollY - d.startScroll;
            d.item.style.transform = `translate(${b.left - a.left}px, ${b.top - a.top + sy}px)`;
            d.item.classList.remove('is-lifted');
            state.settling = true;
            setTimeout(() => {
                dotNet.invokeMethodAsync('OnReorder', d.from, d.to).catch(() => { state.settling = false; clearStyles(); });
            }, 160);
        } else {
            clearStyles();
        }
    };

    const onDown = e => {
        if (state.drag || state.settling) return;
        const item = e.target.closest('.pip-item');
        if (!item || item.parentElement !== grid) return;
        if (e.target.closest('button, a, input')) return;      // action buttons stay clickable
        if (e.pointerType === 'mouse' && e.button !== 0) return;
        const p = { id: e.pointerId, item, type: e.pointerType, x: e.clientX, y: e.clientY, lx: e.clientX, ly: e.clientY };
        state.pending = p;
        if (e.target.closest('.pip-handle')) { e.preventDefault(); begin(p, e.clientX, e.clientY); return; }
        if (p.type === 'touch') p.timer = setTimeout(() => { if (state.pending === p) begin(p, p.lx, p.ly); }, HOLD_MS);
    };

    const onMove = e => {
        const p = state.pending;
        if (p && e.pointerId === p.id) {
            p.lx = e.clientX; p.ly = e.clientY;
            const dist = Math.hypot(e.clientX - p.x, e.clientY - p.y);
            if (p.type === 'touch') {
                if (dist > MOVE_TOLERANCE) { clearTimeout(p.timer); state.pending = null; } // it is a scroll
            } else if (dist > MOUSE_THRESHOLD) {
                begin(p, p.x, p.y);
            }
        }
        if (state.drag && e.pointerId === state.drag.id) { e.preventDefault(); update(e.clientX, e.clientY); }
    };

    const onUp = e => {
        if (state.pending && e.pointerId === state.pending.id) { clearTimeout(state.pending.timer); state.pending = null; }
        if (state.drag && e.pointerId === state.drag.id) finish(e.type === 'pointerup');
    };

    const onKey = e => { if (e.key === 'Escape' && state.drag) finish(false); };
    // While a tile is lifted, a finger move must not scroll or zoom the page.
    const onTouchMove = e => { if (state.drag) e.preventDefault(); };
    // Long-press on touch would otherwise open the image context menu.
    const onContextMenu = e => { if (state.drag || state.pending) e.preventDefault(); };

    grid.addEventListener('pointerdown', onDown);
    grid.addEventListener('contextmenu', onContextMenu);
    window.addEventListener('pointermove', onMove, { passive: false });
    window.addEventListener('pointerup', onUp);
    window.addEventListener('pointercancel', onUp);
    window.addEventListener('keydown', onKey);
    window.addEventListener('touchmove', onTouchMove, { passive: false });

    grid.__sortable = () => {
        observer.disconnect();
        grid.removeEventListener('pointerdown', onDown);
        grid.removeEventListener('contextmenu', onContextMenu);
        window.removeEventListener('pointermove', onMove);
        window.removeEventListener('pointerup', onUp);
        window.removeEventListener('pointercancel', onUp);
        window.removeEventListener('keydown', onKey);
        window.removeEventListener('touchmove', onTouchMove);
    };
}

// Called by .NET when the reorder was rejected or applied without moving nodes.
export function reset(grid) {
    if (!grid) return;
    for (const el of grid.querySelectorAll(':scope > .pip-item')) {
        el.style.transition = 'none';
        el.style.transform = '';
        el.classList.remove('is-lifted');
        void el.offsetWidth;
        el.style.transition = '';
    }
    grid.classList.remove('is-sorting');
}

export function detach(grid) {
    grid?.__sortable?.();
    if (grid) delete grid.__sortable;
}
