/* ============================================================
   FoodOps ERP — Shell v7.0
   Sidebar (hover-intent) · Accordion · Active menu · Breadcrumb
   Filters · Shortcuts · Row click
   ============================================================ */
(function () {
    'use strict';

    const OPEN_DELAY = 160;    // ms: hover ke baad sidebar khulne se pehle
    const CLOSE_DELAY = 450;   // ms: mouse hatne ke baad band hone se pehle

    const body = document.body;
    const hoverCapable = matchMedia('(hover: hover)').matches;
    const isDesktop = () => matchMedia('(min-width: 992px)').matches;
    const $$ = (s, r = document) => [...r.querySelectorAll(s)];

    /* ── 1. Sidebar: hover-intent (overlay, no re-layout) ─── */
    function initSidebar() {
        const sb = document.getElementById('sidebar');
        if (!sb) return;
        let timer;
        const schedule = (open, ms) => {
            clearTimeout(timer);
            timer = setTimeout(() => body.classList.toggle('sidebar-open', open), ms);
        };
        if (hoverCapable) {
            sb.addEventListener('mouseenter', () => schedule(true, OPEN_DELAY));
            sb.addEventListener('mouseleave', () => schedule(false, CLOSE_DELAY));
        } else {                                      // touch: tap se kholo, bahar tap se band
            sb.addEventListener('click', () => { if (isDesktop()) schedule(true, 0); });
            document.addEventListener('click', e => { if (!sb.contains(e.target)) schedule(false, 0); });
        }
        sb.addEventListener('focusin', () => schedule(true, 0));
        sb.addEventListener('focusout', e => {
            if (!sb.contains(e.relatedTarget) && !sb.matches(':hover')) schedule(false, CLOSE_DELAY);
        });
    }

    /* ── 2. Mobile sidebar ───────────────────────────────── */
    function initMobileSidebar() {
        const toggle = document.getElementById('mobileSidebarToggle');
        if (!toggle) return;
        let backdrop = document.getElementById('sidebarBackdrop');
        if (!backdrop) {
            backdrop = document.createElement('div');
            backdrop.id = 'sidebarBackdrop';
            backdrop.className = 'sidebar-backdrop';
            document.body.appendChild(backdrop);
        }
        const close = () => body.classList.remove('sidebar-mobile-open');
        toggle.addEventListener('click', e => { e.stopPropagation(); body.classList.toggle('sidebar-mobile-open'); });
        backdrop.addEventListener('click', close);
        document.addEventListener('keydown', e => { if (e.key === 'Escape') close(); });
        document.addEventListener('foodops:navigated', close);
    }

    /* ── 3. Accordion: ek time pe ek hi open (Bootstrap events) ── */
    function initAccordion() {
        const nav = document.querySelector('.sidebar-nav');
        if (!nav || !window.bootstrap) return;
        nav.addEventListener('show.bs.collapse', e => {
            const el = e.target;
            const siblings = [...el.parentElement.parentElement.children]
                .map(li => li.querySelector(':scope > .collapse'))
                .filter(c => c && c !== el && c.classList.contains('show'));
            siblings.forEach(c => bootstrap.Collapse.getOrCreateInstance(c, { toggle: false }).hide());
        });
    }

    /* ── 4. Active menu + breadcrumb (sidebar hi single source) ── */
    const normPath = u => {
        try { return (new URL(u, location.origin).pathname.replace(/\/+$/, '') || '/').toLowerCase(); }
        catch { return '/'; }
    };

    function setCollapse(el, show) {
        if (!el) return;
        el.classList.toggle('show', show);
        $$(`[data-bs-target="#${el.id}"]`).forEach(t => {
            t.setAttribute('aria-expanded', String(show));
            t.classList.toggle('collapsed', !show);
        });
    }

    function openCollapse(el) {
        if (!el) return;
        [...el.parentElement.parentElement.children].forEach(li => {
            const c = li.querySelector(':scope > .collapse');
            if (c && c !== el) setCollapse(c, false);
        });
        setCollapse(el, true);
    }

    function setActive(url) {
        const p = normPath(url);
        $$('.sidebar-nav .active, .sidebar-nav .active-group, .sidebar-nav .active-module')
            .forEach(n => n.classList.remove('active', 'active-group', 'active-module'));
        $$('.sidebar-nav [aria-current]').forEach(n => n.removeAttribute('aria-current'));

        let best = null, bestLen = -1;
        $$('.sidebar-nav a.nav-link[href]').forEach(a => {
            const href = a.getAttribute('href');
            if (href === '#' || a.classList.contains('is-soon')) return;
            const h = normPath(href);
            const hit = h === p || (h !== '/' && p.startsWith(h + '/'));
            if (hit && h.length > bestLen) { best = a; bestLen = h.length; }   // sab se lamba match; tie me pehla
        });
        if (!best) return null;

        best.classList.add('active');
        best.setAttribute('aria-current', 'page');
        const mod = best.closest('.nav-item');
        const grp = best.closest('.nav-group');
        mod?.querySelector('.nav-module-toggle')?.classList.add('active-module');
        grp?.querySelector('.nav-group-toggle')?.classList.add('active-group');
        openCollapse(best.closest('.nav-features')?.parentElement);
        openCollapse(grp?.querySelector(':scope > .collapse'));
        return best;
    }

    function updateBreadcrumb(fallbackTitle) {
        const el = document.querySelector('.subheader-breadcrumb');
        if (!el) return;
        const active = document.querySelector('.sidebar-nav a.nav-link.active');
        const frag = document.createDocumentFragment();
        const add = node => {
            if (frag.childNodes.length) {
                const s = document.createElement('span');
                s.className = 'crumb-sep';
                s.innerHTML = '<i class="bi bi-chevron-right"></i>';
                frag.appendChild(s);
            }
            frag.appendChild(node);
        };
        const span = (text, cls = 'crumb') => {
            const n = document.createElement('span');
            n.className = cls; n.textContent = text; return n;
        };
        const home = (current) => {
            const n = document.createElement(current ? 'span' : 'a');
            n.className = 'crumb' + (current ? ' current' : '');
            if (!current) n.href = '/';
            n.innerHTML = '<i class="bi bi-house"></i><span>Dashboard</span>';
            return n;
        };

        if (active && active.closest('.nav-dashboard')) {
            add(home(true));
        } else {
            add(home(false));
            if (active) {
                const grp = active.closest('.nav-group')?.querySelector('.nav-group-toggle > span')?.textContent.trim();
                const mod = active.closest('.nav-item')?.querySelector('.nav-module-toggle > span')?.textContent.trim();
                if (grp) add(span(grp));
                if (mod) add(span(mod));
                add(span(active.textContent.trim(), 'crumb current'));
            } else if (fallbackTitle) {
                add(span(fallbackTitle, 'crumb current'));
            }
        }
        el.replaceChildren(frag);
    }

    /* ── 5. Placeholder menu items ("Soon") ──────────────── */
    function markPlaceholders() {
        $$('.sidebar-nav a.nav-link[href="#"]').forEach(a => {
            a.classList.add('is-soon');
            a.setAttribute('aria-disabled', 'true');
            a.tabIndex = -1;
            a.title = 'Coming soon';
        });
    }

    /* ── 6. Search shortcut + AI button ──────────────────── */
    function initSearch() {
        const kbd = document.querySelector('.topbar-search-kbd');
        if (kbd && !/Mac|iPhone|iPad/i.test(navigator.platform || navigator.userAgent)) kbd.textContent = 'Ctrl K';

        document.addEventListener('keydown', e => {
            const search = document.getElementById('globalSearch');
            if ((e.metaKey || e.ctrlKey) && e.key.toLowerCase() === 'k') {
                e.preventDefault();
                search?.focus(); search?.select();
            } else if (e.key === 'Escape' && search && document.activeElement === search) {
                search.blur();
            }
        });

        document.querySelector('.topbar-search-ai')?.addEventListener('click', e => {
            e.preventDefault(); e.stopPropagation();
            window.showToast?.('AI Assistant coming soon…', 'info');
        });
    }

    /* ── 7. Filters (group-wise selection, persist, event) ─ */
    const FILTER_KEY = 'foodops_filters_v1';
    const FILTER_DEFAULTS = {
        'time range': 'Today', 'shift': 'All Shifts',
        'department': 'All Departments', 'warehouse': 'All Warehouses'
    };

    function initFilters() {
        const menus = $$('.topbar-filter-menu');
        if (!menus.length) return;

        const state = { ...FILTER_DEFAULTS };
        try { Object.assign(state, JSON.parse(localStorage.getItem(FILTER_KEY) || '{}')); } catch { /* ignore */ }

        // markup se group/value nikalo (heading <strong> = group)
        menus.forEach(menu => {
            let group = '';
            $$(':scope > li', menu).forEach(li => {
                const h = li.querySelector('strong');
                if (h) { group = h.textContent.trim().toLowerCase(); return; }
                const a = li.querySelector('a.dropdown-item');
                if (a && group) {
                    const c = a.cloneNode(true);
                    c.querySelectorAll('i, .text-muted').forEach(n => n.remove());
                    a.dataset.filterGroup = group;
                    a.dataset.filterValue = c.textContent.trim();
                    a.setAttribute('role', 'menuitemradio');
                }
            });
        });

        const dateStr = new Date().toLocaleDateString('en-US', { month: 'short', day: '2-digit', year: 'numeric' });

        function render() {
            $$('.topbar-filter-menu a[data-filter-group]').forEach(a => {
                const on = state[a.dataset.filterGroup] === a.dataset.filterValue;
                a.classList.toggle('is-selected', on);
                a.setAttribute('aria-checked', String(on));
                const dot = a.querySelector('i.bi');
                if (dot) {
                    dot.classList.toggle('bi-circle-fill', on);
                    dot.classList.toggle('bi-circle', !on);
                    dot.classList.toggle('text-success', on);
                }
            });
            menus.forEach(menu => {
                const btn = menu.closest('.dropdown')?.querySelector('.topbar-filter-btn');
                const label = btn?.querySelector('span.d-none.d-lg-inline') || btn?.querySelector('span');
                if (!label) return;
                if (!label.dataset.base) label.dataset.base = label.textContent.trim();
                if (menu.querySelector('[data-filter-group="time range"]')) {
                    let t = state['time range'] === 'Today' ? `Today, ${dateStr}` : state['time range'];
                    if (state['shift'] !== FILTER_DEFAULTS['shift']) t += ' · ' + state['shift'];
                    label.textContent = t;
                } else {
                    let t = label.dataset.base;
                    if (state['department'] !== FILTER_DEFAULTS['department']) t += ' · ' + state['department'];
                    if (state['warehouse'] !== FILTER_DEFAULTS['warehouse']) t += ' · ' + state['warehouse'];
                    label.textContent = t;
                }
            });
        }

        document.addEventListener('click', e => {
            const a = e.target.closest('a[data-filter-group]');
            if (!a) return;
            e.preventDefault();
            state[a.dataset.filterGroup] = a.dataset.filterValue;
            try { localStorage.setItem(FILTER_KEY, JSON.stringify(state)); } catch { /* ignore */ }
            render();
            // dashboards/grids is event ko sun kar refetch karen
            document.dispatchEvent(new CustomEvent('foodops:filters', { detail: { ...state } }));
        });

        render();
        window.FoodOps.filters = { get: () => ({ ...state }) };
    }

    /* ── 8. Table row click (SPA navigate) ───────────────── */
    function initRowClick() {
        document.addEventListener('click', e => {
            const tr = e.target.closest('tr[data-href]');
            if (!tr || e.target.closest('a, button, input, select, textarea, label')) return;
            window.FoodOpsSPA ? window.FoodOpsSPA.navigate(tr.dataset.href) : (location.href = tr.dataset.href);
        });
    }

    /* ── Init ────────────────────────────────────────────── */
    document.addEventListener('DOMContentLoaded', () => {
        markPlaceholders();
        initSidebar();
        initMobileSidebar();
        initAccordion();
        initSearch();
        initFilters();
        initRowClick();

        setActive(location.pathname);
        updateBreadcrumb(document.querySelector('.page-title')?.textContent.trim());
    });

    window.FoodOps = Object.assign(window.FoodOps || {}, { nav: { setActive, updateBreadcrumb } });
})();