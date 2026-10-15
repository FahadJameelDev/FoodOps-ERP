/* ============================================================
   FoodOps ERP — SPA Router v4.1
   Links · Forms (JSON / 422) · Refresh region · Abort · Back/Forward
   ============================================================ */
(function () {
    'use strict';

    const cfg = { content: '.page-content', loadingClass: 'spa-loading', loadingDelay: 150 };
    const AJAX = { 'X-Requested-With': 'XMLHttpRequest' };

    const nav = () => window.FoodOps?.nav;
    const ui = () => window.FoodOps?.ui;
    const $content = () => document.querySelector(cfg.content);

    let currentUrl = location.pathname + location.search;
    let controller = null;
    let progressEl = null;

    /* ── Helpers ───────────────────────────────────── */
    const esc = s => { const d = document.createElement('div'); d.textContent = s ?? ''; return d.innerHTML; };

    function progress(on) {
        if (!progressEl) {
            progressEl = document.createElement('div');
            progressEl.className = 'spa-progress';
            document.body.appendChild(progressEl);
        }
        if (on) { progressEl.classList.remove('done'); progressEl.classList.add('active'); }
        else {
            progressEl.classList.remove('active'); progressEl.classList.add('done');
            setTimeout(() => progressEl.classList.remove('done'), 350);
        }
    }

    // Agar server ne galti se poora layout bhej diya to sirf content nikalo
    function unwrapLayout(html) {
        if (!/id=["']sidebar["']/.test(html)) return html;
        console.warn('[SPA] Full layout received — _ViewStart / Layout=null check karo.');
        const doc = new DOMParser().parseFromString(html, 'text/html');
        return doc.querySelector(cfg.content)?.innerHTML ?? html;
    }

    function runScripts(container) {
        container.querySelectorAll('script').forEach(old => {
            const s = document.createElement('script');
            [...old.attributes].forEach(a => s.setAttribute(a.name, a.value));
            s.textContent = old.textContent;
            old.replaceWith(s);
        });
    }

    function errorHtml(message, url) {
        return `<div class="spa-error">
            <i class="bi bi-exclamation-triangle"></i>
            <h3>Couldn't load this page</h3>
            <p>${esc(message)}</p>
            <button type="button" class="btn btn-primary" data-spa-retry="${encodeURIComponent(url)}">Try again</button>
        </div>`;
    }

    function afterRender(box, url) {
        const title = box.querySelector('.page-title')?.textContent.trim();
        nav()?.setActive(url);
        nav()?.updateBreadcrumb(title);
        const label = document.querySelector('.sidebar-nav a.nav-link.active')?.textContent.trim();
        const t = title || label;
        document.title = t ? `${t} - FoodOps ERP` : 'FoodOps ERP';
        runScripts(box);
        ui()?.parseValidation(box);
        box.focus({ preventScroll: true });
        document.dispatchEvent(new CustomEvent('foodops:navigated', { detail: { url } }));
    }

    /* ── Page load ─────────────────────────────────── */
    async function loadPage(url, push = true) {
        const box = $content();
        if (!box) { location.href = url; return; }

        controller?.abort();
        const ctrl = controller = new AbortController();
        const timer = setTimeout(() => box.classList.add(cfg.loadingClass), cfg.loadingDelay);
        progress(true);
        nav()?.setActive(url);                               // instant feedback

        try {
            const res = await fetch(url, {
                headers: { ...AJAX, Accept: 'text/html' },
                credentials: 'same-origin',
                signal: ctrl.signal
            });
            if (res.redirected && /\/account\/login/i.test(res.url)) { location.href = res.url; return; }
            if (!res.ok) {
                throw new Error(res.status === 404 ? 'Page not found.'
                    : res.status === 403 ? 'You do not have access to this page.'
                        : `Server error (${res.status}).`);
            }
            const html = unwrapLayout(await res.text());
            if (ctrl.signal.aborted) return;

            box.innerHTML = html;
            box.scrollTop = 0;
            const clean = url.split('#')[0];
            if (push && clean !== currentUrl) history.pushState({ url: clean }, '', clean);
            currentUrl = clean;
            afterRender(box, clean);
        } catch (err) {
            if (err.name === 'AbortError') return;
            nav()?.setActive(currentUrl);                    // highlight wapas
            box.innerHTML = errorHtml(err.message || 'Something went wrong.', url);
        } finally {
            clearTimeout(timer);
            if (controller === ctrl) {
                box.classList.remove(cfg.loadingClass);
                progress(false);
                controller = null;
            }
        }
    }

    /* ── Refresh sirf ek region (grid) ─────────────── */
    async function refresh(selector, url = currentUrl) {
        const target = selector && document.querySelector(selector);
        if (!target) return loadPage(url, false);
        try {
            const res = await fetch(url, { headers: { ...AJAX, Accept: 'text/html' }, credentials: 'same-origin' });
            if (!res.ok) throw new Error('refresh failed');
            const doc = new DOMParser().parseFromString(unwrapLayout(await res.text()), 'text/html');
            const fresh = doc.querySelector(selector);
            if (!fresh) return loadPage(url, false);
            target.replaceWith(fresh);
            document.dispatchEvent(new CustomEvent('foodops:refreshed', { detail: { selector } }));
        } catch {
            return loadPage(url, false);
        }
    }

    /* ── Link clicks ───────────────────────────────── */
    function onClick(e) {
        if (e.defaultPrevented || e.button !== 0 || e.metaKey || e.ctrlKey || e.shiftKey || e.altKey) return;

        const retry = e.target.closest('[data-spa-retry]');
        if (retry) { loadPage(decodeURIComponent(retry.dataset.spaRetry), false); return; }

        const a = e.target.closest('a[href]');
        if (!a) return;
        const href = a.getAttribute('href');

        if (href === '#' || href === '') { e.preventDefault(); return; }            // placeholder links
        if (href.startsWith('#') || /^(javascript:|mailto:|tel:)/i.test(href)) return;
        if (a.target === '_blank' || a.hasAttribute('download') || a.hasAttribute('data-no-spa') ||
            a.hasAttribute('data-modal') || a.hasAttribute('data-bs-toggle') || a.hasAttribute('data-bs-dismiss')) return;
        if (a.closest('#foodops-modal-overlay, .fo-confirm-overlay')) return;
        if (a.origin !== location.origin) return;

        e.preventDefault();
        loadPage(a.pathname + a.search, true);
    }

    /* ── Browser back / forward ────────────────────── */
    function onPop() {
        const url = location.pathname + location.search;
        if (url === currentUrl) return;                      // sirf #hash badla
        ui()?.closeModal();
        loadPage(url, false);
    }

    /* ── Forms: form[data-spa-form] ────────────────── */
    function setBusy(form, btn, on) {
        if (on) form.dataset.busy = '1'; else delete form.dataset.busy;
        if (btn) { btn.disabled = on; btn.classList.toggle('is-loading', on); }
    }

    // ✅ REPLACED: onSaved
    async function onSaved(form, message) {
        const sel = form.dataset.refresh;
        const inModal = !!form.closest('#foodops-modal-overlay');
        const back = form.dataset.reloadUrl || currentUrl;

        // modal: jis page par khula tha usi ko refresh; direct form page: list par wapas navigate
        const refreshing = inModal
            ? (sel ? refresh(sel, currentUrl) : loadPage(currentUrl, false))
            : loadPage(back, true);
        await ui()?.tick(message || 'Saved successfully');
        if (inModal) ui()?.closeModal();
        await refreshing;
    }

    async function handleFormResponse(res, form) {
        const ct = res.headers.get('content-type') || '';

        if (res.redirected && /\/account\/login/i.test(res.url)) { location.href = res.url; return; }

        if (ct.includes('application/json')) {
            const data = await res.json().catch(() => ({}));
            if (res.ok && data.success !== false) return onSaved(form, data.message);
            ui()?.toast(data.message || 'Could not save. Please review and try again.', 'error');
            return;
        }

        // purana pattern: controller RedirectToAction => fetch redirect follow karta hai
        if (res.ok && res.redirected) return onSaved(form);

        // HTML wapas aaya => validation errors (200 ho ya 400/422)
        if (ct.includes('text/html') && (res.ok || res.status === 400 || res.status === 422)) {
            const html = await res.text();
            if (form.closest('#foodops-modal-body')) {
                ui()?.setModalContent(html);
            } else {
                const box = $content();
                box.innerHTML = unwrapLayout(html);
                ui()?.parseValidation(box);
                box.querySelector('.input-validation-error')?.focus();
            }
            return;
        }

        ui()?.toast(res.status === 400 || res.status === 403
            ? 'Session expired. Please refresh the page and try again.'
            : 'Server error. Please try again.', 'error');
    }

    async function onSubmit(e) {
        const form = e.target.closest ? e.target.closest('form[data-spa-form]') : null;
        if (!form) return;
        e.preventDefault();
        if (form.dataset.busy) return;

        // client-side validation (jquery unobtrusive)
        const j = window.jQuery;
        if (j && j.validator && j(form).data('validator') && !j(form).valid()) {
            form.querySelector('.input-validation-error')?.focus();
            return;
        }

        const btn = form.querySelector('[type="submit"]');
        setBusy(form, btn, true);
        try {
            const token = form.querySelector('input[name="__RequestVerificationToken"]')?.value || ui()?.csrf();
            const res = await fetch(form.action, {
                method: (form.method || 'POST').toUpperCase(),
                body: new FormData(form),
                headers: { ...AJAX, Accept: 'application/json, text/html', ...(token ? { RequestVerificationToken: token } : {}) },
                credentials: 'same-origin'
            });
            await handleFormResponse(res, form);
        } catch {
            ui()?.toast('Network error. Please check your connection and try again.', 'error');
        } finally {
            setBusy(form, btn, false);
        }
    }

    /* ── Public API + start ────────────────────────── */
    window.FoodOpsSPA = {
        navigate: url => loadPage(url, true),
        reload: () => loadPage(currentUrl, false),
        refresh
    };

    document.addEventListener('DOMContentLoaded', () => {
        history.replaceState({ url: currentUrl }, '', currentUrl);
        document.addEventListener('click', onClick);
        document.addEventListener('submit', onSubmit);
        window.addEventListener('popstate', onPop);
    });
})();