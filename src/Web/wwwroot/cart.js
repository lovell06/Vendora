// Client-side helpers for the cart. The floating cart panel's opening origin and every way of closing it are handled
// here, in the browser, so the animation starts on the same frame as the click instead of waiting for a
// Blazor Server round trip. The server is told to close the dialog only after the exit animation has played.
window.vendoraCart = (() => {
  const PANEL = '.mud-dialog.cart-drawer';
  const EXIT_MS = 260;
  let origin = null;
  let dotnet = null;

  const panel = () => document.querySelector(PANEL);
  const reducedMotion = () => window.matchMedia('(prefers-reduced-motion: reduce)').matches;

  // transform-origin, relative to the panel, at the centre of the given element (the cart icon).
  // The panel is pinned to the top right of the viewport with the given margin.
  const originFor = (element, margin = 16, maxWidth = 440) => {
    const r = element.getBoundingClientRect();
    const width = Math.min(maxWidth, window.innerWidth - 2 * margin);
    const left = window.innerWidth - margin - width;
    return `${Math.round(r.left + r.width / 2 - left)}px ${Math.round(r.top + r.height / 2 - margin)}px`;
  };

  // Is the cart panel the dialog on top (not covered by, say, the checkout confirmation)?
  const panelOnTop = () => {
    const dialogs = document.querySelectorAll('.mud-dialog');
    return dialogs.length > 0 && dialogs[dialogs.length - 1].matches(PANEL);
  };

  // Starts the exit animation at once, then asks the server to remove the dialog.
  const close = () => {
    const p = panel();
    if (!p || p.classList.contains('cart-drawer--closing')) return;
    const finish = () => dotnet?.invokeMethodAsync('CloseNow');
    if (reducedMotion()) { finish(); return; }
    p.classList.add('cart-drawer--closing');
    setTimeout(finish, EXIT_MS);
  };

  const init = () => {
    // Remember where the cart icon is at the moment of the click, before the server is involved.
    document.addEventListener('click', (e) => {
      const button = e.target instanceof Element ? e.target.closest('.cart-button') : null;
      if (button) origin = originFor(button);
    }, true);

    // Set the origin as soon as the panel enters the DOM (before it is painted), so it grows out of the icon.
    new MutationObserver(() => {
      const p = document.querySelector(`${PANEL}:not([data-cart-ready])`);
      if (!p) return;
      p.dataset.cartReady = '1';
      if (origin) p.style.setProperty('--cart-origin', origin);
    }).observe(document.body, { childList: true, subtree: true });

    // Close actions: the backdrop, Esc, and any element marked data-cart-close inside the panel.
    document.addEventListener('click', (e) => {
      const t = e.target instanceof Element ? e.target : null;
      if (!t || !panel()) return;
      if (t.closest('[data-cart-close]') && t.closest(PANEL)) { e.preventDefault(); e.stopPropagation(); close(); return; }
      if (!t.closest('.mud-dialog') && t.closest('.overlay-blur-backdrop')) close();
    }, true);
    document.addEventListener('keydown', (e) => {
      if (e.key === 'Escape' && panel() && panelOnTop()) { e.preventDefault(); close(); }
    }, true);
  };

  if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();

  return {
    originFor,
    register: (ref) => { dotnet = ref; },
    unregister: () => { dotnet = null; },
    // Used by server-side close paths (checkout, "View full cart"): plays the exit, resolves when it is done.
    playExit: () => new Promise((resolve) => {
      const p = panel();
      if (!p || reducedMotion()) { resolve(); return; }
      p.classList.add('cart-drawer--closing');
      setTimeout(resolve, EXIT_MS + 10);
    }),
    // A checkbox's indeterminate state is a DOM property with no HTML attribute.
    setIndeterminate: (element, value) => { if (element) element.indeterminate = !!value; }
  };
})();
