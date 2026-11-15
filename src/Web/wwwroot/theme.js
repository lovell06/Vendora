(() => {
    const key = 'vendora-theme';
    const media = window.matchMedia('(prefers-color-scheme: dark)');
    const valid = value => ['system', 'light', 'dark'].includes(value) ? value : 'system';
    let preference = 'system';
    try { preference = valid(localStorage.getItem(key)); } catch {}
    const subscribers = new Map();
    let nextId = 0;
    const get = () => ({ preference, isDark: preference === 'system' ? media.matches : preference === 'dark' });
    const apply = () => {
        const snapshot = get();
        document.documentElement.dataset.themePreference = preference;
        document.documentElement.dataset.theme = snapshot.isDark ? 'dark' : 'light';
        subscribers.forEach(reference => {
            reference.invokeMethodAsync('ThemeChanged', snapshot).catch(() => {});
        });
        return snapshot;
    };
    window.vendoraTheme = {
        get,
        set(value) {
            preference = valid(value);
            try { localStorage.setItem(key, preference); } catch {}
            return apply();
        },
        subscribe(reference) { const id = ++nextId; subscribers.set(id, reference); return id; },
        unsubscribe(id) { subscribers.delete(id); }
    };
    apply();
    media.addEventListener('change', apply);
    window.addEventListener('storage', event => {
        if (event.key !== key && event.key !== null) return;
        try { preference = valid(localStorage.getItem(key)); } catch { preference = 'system'; }
        apply();
    });
})();
