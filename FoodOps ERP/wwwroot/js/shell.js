/* ============================================================
   FoodOps ERP — Shell v8.0
   Sidebar (hover-intent) · Accordion · Active menu · Module tabs
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
        } else {
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

    /* ── 3. Accordion: ek time pe ek hi open ─────────────── */
    function initAccordion() {
        const nav = document.querySelector('.sidebar-nav');
        if (!nav || !window.bootstrap) return;
        nav.addEventListener('show.bs.collapse', e => {
            const el = e.target;
            [...el.parentElement.parentElement.children]
                .map(li => li.querySelector(':scope > .collapse'))
                .filter(c => c && c !== el && c.classList.contains('show'))
                .forEach(c => bootstrap.Collapse.getOrCreateInstance(c, { toggle: false }).hide());
        });
    }

    /* ── 4. Active menu (sidebar = single source of truth) ── */
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
            if (hit && h.length > bestLen) { best = a; bestLen = h.length; }   // sab se lamba match
        });
        if (!best) return null;

        best.classList.add('active');
        best.setAttribute('aria-current', 'page');
        best.closest('.nav-item')?.querySelector('.nav-module-toggle')?.classList.add('active-module');
        const grp = best.closest('.nav-group');
        grp?.querySelector('.nav-group-toggle')?.classList.add('active-group');
        openCollapse(best.closest('.nav-features')?.parentElement);
        openCollapse(grp?.querySelector(':scope > .collapse'));
        return best;
    }

    /* ── 5. Module tab bar ── */
    function updateBreadcrumb() {
        const bar = document.querySelector('.subheader');
        const nav = bar?.querySelector('.modnav');
        if (!bar || !nav) return;

        const active = document.querySelector('.sidebar-nav a.nav-link.active');
        const list = active?.closest('.nav-features');
        const seen = new Set();

        const links = list ? [...list.querySelectorAll('a.nav-link')].filter(a => {
            const href = a.getAttribute('href');
            if (!href || href === '#' || a.classList.contains('is-soon') || seen.has(href)) return false;
            seen.add(href);
            return true;
        }) : [];

        if (!active || links.length < 2) {
            bar.hidden = true;
            nav.replaceChildren();
            return;
        }

        const strip = document.createElement('div');
        strip.className = 'modnav__tabs';
        const activeHref = active.getAttribute('href');

        const iconFor = (a) => {
            const short = (a.dataset.short || '').toLowerCase();
            const full = (a.textContent || '').toLowerCase().replace(/\s+/g, ' ').trim();
            const t = short + ' ' + full;

            // —— more specific first ——
            if (t.includes('incoming qc')) return 'bi-clipboard-check';
            if (t.includes('production qc')) return 'bi-clipboard2-pulse';
            if (t.includes('qc setup')) return 'bi-sliders';
            if (t.includes('quality decision') || t.includes('decisions')) return 'bi-ui-checks';
            if (t.includes('release')) return 'bi-unlock';
            if (t.includes('quality records') || (t.includes('records') && t.includes('quality'))) return 'bi-folder2-open';
            if (t.includes('records')) return 'bi-folder2-open';

            if (t.includes('goods receiving') || t.includes('receiving') || t.includes('grn')) return 'bi-box-arrow-in-down';
            if (t.includes('purchase')) return 'bi-cart';
            if (t.includes('supplier')) return 'bi-building';
            if (t.includes('movement')) return 'bi-arrow-left-right';
            if (t.includes('lot') || t.includes('expiry')) return 'bi-calendar-x';
            if (t.includes('material issue') || t.includes('material')) return 'bi-box-arrow-up';
            if (t.includes('wastage')) return 'bi-trash3';
            if (t.includes('stock')) return 'bi-box-seam';

            if (t.includes('packaging')) return 'bi-box';
            if (t.includes('catalogue') || t.includes('catalog')) return 'bi-boxes';
            if (t.includes('versioning')) return 'bi-layers';
            if (t.includes('change') || t.includes('changes')) return 'bi-pencil-square';
            if (t.includes('approval') || t.includes('approvals')) return 'bi-check2-circle';
            if (t.includes('recipe')) return 'bi-journal-text';
            if (t.includes('product')) return 'bi-boxes';

            if (t.includes('demand')) return 'bi-graph-up-arrow';
            if (t.includes('plan')) return 'bi-diagram-3';
            if (t.includes('batch')) return 'bi-collection';

            if (t.includes('unit cost')) return 'bi-tag';
            if (t.includes('cost component')) return 'bi-puzzle';
            if (t.includes('batch costing')) return 'bi-receipt';
            if (t.includes('variance control')) return 'bi-sliders2';
            if (t.includes('variance')) return 'bi-bar-chart';
            if (t.includes('loss')) return 'bi-graph-down';
            if (t.includes('cost')) return 'bi-currency-dollar';

            if (t.includes('correction')) return 'bi-wrench-adjustable';
            if (t.includes('attendance')) return 'bi-person-check';
            if (t.includes('overtime')) return 'bi-clock-history';
            if (t.includes('leave')) return 'bi-calendar-minus';
            if (t.includes('compensation')) return 'bi-cash-coin';
            if (t.includes('workforce setup') || (t.includes('setup') && t.includes('workforce'))) return 'bi-sliders';
            if (t.includes('setup')) return 'bi-sliders';
            if (t.includes('employee')) return 'bi-person-badge';
            if (t.includes('finalization') || t.includes('finalize')) return 'bi-check-all';
            if (t.includes('processing')) return 'bi-gear-wide-connected';
            if (t.includes('review')) return 'bi-search';
            if (t.includes('payroll')) return 'bi-cash-stack';

            if (t.includes('ai hub') || t.includes('assistant hub') || t.includes('hub')) return 'bi-robot';
            if (t.includes('inventory assistant') || (t.includes('inventory') && t.includes('assistant'))) return 'bi-box-seam';
            if (t.includes('production assistant')) return 'bi-gear';
            if (t.includes('quality assistant')) return 'bi-patch-check';
            if (t.includes('cost assistant')) return 'bi-currency-dollar';
            if (t.includes('payroll assistant')) return 'bi-cash-stack';
            if (t.includes('management assistant') || t.includes('management')) return 'bi-briefcase';
            if (t.includes('safety')) return 'bi-shield-check';
            if (t.includes('evidence')) return 'bi-file-earmark-text';
            if (t.includes('audit')) return 'bi-journal-check';
            if (t.includes('assistant')) return 'bi-chat-dots';

            if (t.includes('overview') || t.includes('factory')) return 'bi-grid-1x2';
            if (t.includes('alert')) return 'bi-exclamation-triangle';
            if (t.includes('action')) return 'bi-lightning-charge';
            if (t.includes('trend')) return 'bi-graph-up';
            if (t.includes('workforce')) return 'bi-people';
            if (t.includes('inventory')) return 'bi-box-seam';
            if (t.includes('quality') || t.includes('qc')) return 'bi-patch-check';
            if (t.includes('production')) return 'bi-gear';
            if (t.includes('report')) return 'bi-file-earmark-bar-graph';
            if (t.includes('ai summary') || t.includes('ai')) return 'bi-robot';

            return 'bi-circle'; // last resort (almost never)
        };

        links.forEach(a => {
            const t = document.createElement('a');
            const on = a.getAttribute('href') === activeHref;
            t.className = 'modnav__tab' + (on ? ' is-active' : '');
            t.href = a.getAttribute('href');

            const label = (a.dataset.short || a.textContent || '').trim();
            const icon = iconFor(a);
            t.innerHTML = `<i class="bi ${icon}" aria-hidden="true"></i><span>${label}</span>`;

            if (on) t.setAttribute('aria-current', 'page');
            strip.appendChild(t);
        });

        nav.replaceChildren(strip);
        bar.hidden = false;
        strip.querySelector('.is-active')?.scrollIntoView({ block: 'nearest', inline: 'nearest' });
    }
    /* ── 6. Placeholder menu items ("Soon") ──────────────── */
    function markPlaceholders() {
        $$('.sidebar-nav a.nav-link[href="#"]').forEach(a => {
            a.classList.add('is-soon');
            a.setAttribute('aria-disabled', 'true');
            a.tabIndex = -1;
            a.title = 'Coming soon';
        });
    }

    /* ── 7. Search shortcut + AI button ──────────────────── */
    function initSearch() {
        const kbd = document.querySelector('.topbar-search-kbd');
        if (kbd && !/Mac|iPhone|iPad/i.test(navigator.platform || navigator.userAgent)) kbd.textContent = 'Ctrl K';

        const searchInput = document.getElementById('globalSearch');
        const searchForm = document.getElementById('globalSearchForm');

        // ✅ FIX: Search Form Submit (Enter dabane par)
        if (searchForm && searchInput) {
            searchForm.addEventListener('submit', (e) => {
                // Hamesha pehle default submit roko
                e.preventDefault();

                const query = searchInput.value.trim();

                // Agar query khali hai, toh focus karo
                if (!query) {
                    searchInput.focus();
                    return;
                }

                // URL encode karo (spaces ko %20 mein badlo)
                const url = `/Search?q=${encodeURIComponent(query)}`;

                // Agar SPA Router available hai, toh use karo
                if (window.FoodOpsSPA && typeof window.FoodOpsSPA.navigate === 'function') {
                    window.FoodOpsSPA.navigate(url);
                } else {
                    // Warna normal redirect karo
                    window.location.href = url;
                }
            });
        }

        // Ctrl+K shortcut
        document.addEventListener('keydown', e => {
            if ((e.metaKey || e.ctrlKey) && e.key.toLowerCase() === 'k') {
                e.preventDefault();
                searchInput?.focus();
                searchInput?.select();
            } else if (e.key === 'Escape' && searchInput && document.activeElement === searchInput) {
                searchInput.blur();
            }
        });

        // ✅ AI Button (Topbar wala)
        const aiBtn = document.querySelector('.topbar-search-ai');
        if (aiBtn) {
            aiBtn.addEventListener('click', e => {
                e.preventDefault();
                e.stopPropagation();

                const aiUrl = '/AIAssistant';
                if (window.FoodOpsSPA && typeof window.FoodOpsSPA.navigate === 'function') {
                    window.FoodOpsSPA.navigate(aiUrl);
                } else {
                    window.location.href = aiUrl;
                }
            });
        }
    }
    /* ── 8. Filters (group-wise selection, persist, event) ─ */
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
            document.dispatchEvent(new CustomEvent('foodops:filters', { detail: { ...state } }));   // grids/dashboard sun sakte hain
        });

        render();
        window.FoodOps.filters = { get: () => ({ ...state }) };
    }

    /* ── 9. Table row click (SPA navigate) ───────────────── */
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
        updateBreadcrumb();
    });

    window.FoodOps = Object.assign(window.FoodOps || {}, { nav: { setActive, updateBreadcrumb } });
})();