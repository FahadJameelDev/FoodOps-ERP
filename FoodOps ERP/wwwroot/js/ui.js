/* ============================================================
   FoodOps ERP — UI Kit v7.1
   Toast · Success Tick · Modal · Confirm · Action · Repeater
   ============================================================ */
(function () {
    'use strict';

    const $ = (s, r = document) => r.querySelector(s);
    const csrfToken = () => document.querySelector('meta[name="csrf-token"]')?.content || '';

    /* ── Toast ─────────────────────────────────────── */
    const TOAST_ICONS = {
        success: 'bi-check-circle-fill', error: 'bi-x-circle-fill',
        warning: 'bi-exclamation-triangle-fill', info: 'bi-info-circle-fill'
    };

    function showToast(message, type = 'info', ms = 4000) {
        let box = $('#foodops-toast-container');
        if (!box) {
            box = document.createElement('div');
            box.id = 'foodops-toast-container';
            box.className = 'foodops-toast-container';
            box.setAttribute('aria-live', 'polite');
            document.body.appendChild(box);
        }
        const t = document.createElement('div');
        t.className = `foodops-toast foodops-toast-${type}`;
        t.setAttribute('role', type === 'error' ? 'alert' : 'status');
        t.innerHTML = `<div class="foodops-toast-body"><i class="bi ${TOAST_ICONS[type] || TOAST_ICONS.info}"></i><span></span></div>`;
        t.querySelector('span').textContent = message;      // textContent => XSS safe
        box.appendChild(t);
        requestAnimationFrame(() => t.classList.add('show'));
        setTimeout(() => { t.classList.remove('show'); setTimeout(() => t.remove(), 250); }, ms);
    }

    /* ── Success tick (animated) ───────────────────── */
    function showSuccessTick(message, callback, ms = 1400) {
        $('#success-tick-overlay')?.remove();
        const o = document.createElement('div');
        o.id = 'success-tick-overlay';
        o.className = 'success-tick-overlay';
        o.setAttribute('role', 'status');
        o.innerHTML = `
            <div class="success-tick-box">
                <svg class="tick-svg" viewBox="0 0 52 52" aria-hidden="true">
                    <circle class="tick-circle" cx="26" cy="26" r="24"/>
                    <path class="tick-check" d="M15 27l8 8 15-17"/>
                </svg>
                <div class="success-tick-text"></div>
            </div>`;
        o.querySelector('.success-tick-text').textContent = message || 'Saved successfully';
        document.body.appendChild(o);
        requestAnimationFrame(() => o.classList.add('show'));
        return new Promise(resolve => setTimeout(() => {
            o.classList.remove('show');
            setTimeout(() => { o.remove(); if (typeof callback === 'function') callback(); resolve(); }, 220);
        }, ms));
    }

    /* ── Validation re-bind (injected forms) ───────── */
    function parseValidation(root) {
        const j = window.jQuery;
        if (!j || !j.validator || !j.validator.unobtrusive || !root) return;
        j(root).find('form').addBack('form').each(function () {
            j(this).removeData('validator').removeData('unobtrusiveValidation');
        });
        j.validator.unobtrusive.parse(root);
    }

    /* ── Focus trap ────────────────────────────────── */
    function trapFocus(e, root) {
        if (!root) return;
        const f = [...root.querySelectorAll(
            'a[href],button:not([disabled]),input:not([disabled]):not([type=hidden]),select:not([disabled]),textarea:not([disabled]),[tabindex]:not([tabindex="-1"])'
        )].filter(el => el.offsetParent !== null);
        if (!f.length) return;
        const first = f[0], last = f[f.length - 1];
        if (e.shiftKey && document.activeElement === first) { e.preventDefault(); last.focus(); }
        else if (!e.shiftKey && document.activeElement === last) { e.preventDefault(); first.focus(); }
    }

    /* ── Modal ─────────────────────────────────────── */
    let overlayEl = null;
    let lastFocus = null;

    function openModal(title, html, opts) {
        if (typeof opts !== 'object' || opts === null) opts = {};   // purana footerHtml argument ignore
        closeModal(true);
        lastFocus = document.activeElement;

        const o = document.createElement('div');
        o.id = 'foodops-modal-overlay';
        o.className = 'fo-modal-overlay';
        o.innerHTML = `
            <div class="fo-modal fo-modal--${opts.size || 'md'}" role="dialog" aria-modal="true" aria-labelledby="fo-modal-title">
                <div class="fo-modal__header">
                    <h3 class="fo-modal__title" id="fo-modal-title"><i class="bi ${opts.icon || 'bi-pencil-square'}"></i><span></span></h3>
                    <button type="button" class="fo-modal__close" data-modal-close aria-label="Close"><i class="bi bi-x-lg"></i></button>
                </div>
                <div class="fo-modal__content" id="foodops-modal-body"></div>
            </div>`;
        o.querySelector('.fo-modal__title span').textContent = title || '';
        document.body.appendChild(o);
        overlayEl = o;
        setModalContent(html);
        requestAnimationFrame(() => o.classList.add('show'));
        o.addEventListener('mousedown', e => { if (e.target === o) requestClose(); });
        return o;
    }

    function setModalContent(html) {
        const box = $('#foodops-modal-body');
        if (!box) return;
        html = html || '';
        if (!/fo-modal__body/.test(html)) html = `<div class="fo-modal__body">${html}</div>`;
        box.innerHTML = html;
        parseValidation(box);
        box.querySelectorAll('[data-repeater]').forEach(recalcRepeater);  // ✅ ADDED

        const form = box.querySelector('form');
        if (form) {
            if (box.querySelector('.input-validation-error')) form.dataset.dirty = '1';
            form.addEventListener('input', () => { form.dataset.dirty = '1'; }, { once: true });
        }
        const first = box.querySelector('.input-validation-error') ||
            box.querySelector('input:not([type=hidden]):not([disabled]), select, textarea');
        (first || $('.fo-modal__close', overlayEl))?.focus();
    }

    async function requestClose() {
        const dirty = overlayEl?.querySelector('form[data-dirty="1"]');
        if (dirty) {
            const ok = await confirmDialog({
                title: 'Discard changes?', message: 'You have unsaved changes. Close without saving?',
                confirmText: 'Discard', danger: true
            });
            if (!ok) return;
        }
        closeModal();
    }

    function closeModal(immediate) {
        const o = overlayEl;
        if (!o) return;
        overlayEl = null;
        o.classList.remove('show');
        if (immediate) { o.remove(); return; }
        setTimeout(() => { o.remove(); lastFocus?.focus?.(); }, 200);
    }

    /* ── Confirm dialog (Promise<boolean>) ─────────── */
    function confirmDialog({ title = 'Are you sure?', message = '', confirmText = 'Confirm', cancelText = 'Cancel', danger = false } = {}) {
        return new Promise(resolve => {
            const prev = document.activeElement;
            const o = document.createElement('div');
            o.className = 'fo-modal-overlay fo-confirm-overlay';
            o.innerHTML = `
                <div class="fo-modal fo-modal--sm" role="alertdialog" aria-modal="true" aria-labelledby="fo-confirm-title" aria-describedby="fo-confirm-msg">
                    <div class="fo-confirm__body">
                        <div class="fo-confirm__icon ${danger ? 'is-danger' : ''}"><i class="bi ${danger ? 'bi-exclamation-triangle' : 'bi-question-circle'}"></i></div>
                        <h3 class="fo-confirm__title" id="fo-confirm-title"></h3>
                        <p class="fo-confirm__msg" id="fo-confirm-msg"></p>
                    </div>
                    <div class="fo-modal__footer">
                        <button type="button" class="btn btn-outline-secondary" data-act="cancel"></button>
                        <button type="button" class="btn ${danger ? 'btn-danger' : 'btn-primary'}" data-act="ok"></button>
                    </div>
                </div>`;
            o.querySelector('#fo-confirm-title').textContent = title;
            o.querySelector('#fo-confirm-msg').textContent = message;
            o.querySelector('[data-act="cancel"]').textContent = cancelText;
            o.querySelector('[data-act="ok"]').textContent = confirmText;

            const finish = v => {
                document.removeEventListener('keydown', onKey, true);
                o.classList.remove('show');
                setTimeout(() => { o.remove(); prev?.focus?.(); }, 180);
                resolve(v);
            };
            const onKey = e => {
                if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); finish(false); }
                else if (e.key === 'Tab') trapFocus(e, o.firstElementChild);
            };
            document.addEventListener('keydown', onKey, true);
            o.addEventListener('click', e => {
                const a = e.target.closest('[data-act]');
                if (a) finish(a.dataset.act === 'ok');
                else if (e.target === o) finish(false);
            });
            document.body.appendChild(o);
            requestAnimationFrame(() => o.classList.add('show'));
            o.querySelector('[data-act="ok"]').focus();
        });
    }

    /* ── Global handlers (delegation) ──────────────── */
    document.addEventListener('keydown', e => {
        if (!overlayEl || $('.fo-confirm-overlay')) return;
        if (e.key === 'Escape') { e.preventDefault(); requestClose(); }
        else if (e.key === 'Tab') trapFocus(e, $('.fo-modal', overlayEl));
    });

    document.addEventListener('click', async e => {
        // X / Cancel
        if (e.target.closest('[data-modal-close]')) { e.preventDefault(); requestClose(); return; }

        // [data-modal] link/button => form modal me kholo
        const t = e.target.closest('[data-modal]');
        if (t) {
            if (e.button !== 0 || e.metaKey || e.ctrlKey || e.shiftKey) return;   // new tab allowed
            const url = t.getAttribute('href') || t.dataset.modalUrl;
            if (!url) return;
            e.preventDefault();
            openModal(t.dataset.modalTitle || 'Details', '<div class="skeleton skeleton--form"></div>',
                { size: t.dataset.modalSize, icon: t.dataset.modalIcon });
            try {
                const res = await fetch(url, {
                    headers: { 'X-Requested-With': 'XMLHttpRequest', Accept: 'text/html' },
                    credentials: 'same-origin'
                });
                if (res.redirected && /\/account\/login/i.test(res.url)) { location.href = res.url; return; }
                if (!res.ok) throw new Error('HTTP ' + res.status);
                setModalContent(await res.text());
            } catch {
                closeModal(true);
                showToast('Could not load the form. Please try again.', 'error');
            }
            return;
        }

        // ✅ REPLACED: [data-action-url] / [data-delete-url] => confirm + POST
        const d = e.target.closest('[data-action-url],[data-delete-url]');
        if (d) {
            e.preventDefault();
            const isDelete = !d.dataset.actionUrl;
            const url = d.dataset.actionUrl || d.dataset.deleteUrl;
            const name = d.dataset.name ? `"${d.dataset.name}"` : 'This record';
            if (d.dataset.confirm !== 'false') {
                const ok = await confirmDialog({
                    title: d.dataset.confirmTitle || (isDelete ? 'Delete record?' : 'Please confirm'),
                    message: d.dataset.confirmMessage || (isDelete ? `${name} will be deleted. This cannot be undone.` : `${name}: continue?`),
                    confirmText: d.dataset.confirmText || (isDelete ? 'Delete' : 'Confirm'),
                    danger: isDelete || d.dataset.danger === 'true'
                });
                if (!ok) return;
            }
            d.classList.add('is-loading'); d.disabled = true;
            try {
                const res = await fetch(url, {
                    method: 'POST',
                    headers: { 'X-Requested-With': 'XMLHttpRequest', Accept: 'application/json', RequestVerificationToken: csrfToken() },
                    credentials: 'same-origin'
                });
                const data = await res.json().catch(() => ({}));
                if (res.ok && data.success !== false) {
                    showToast(data.message || 'Done', 'success');
                    const spa = window.FoodOpsSPA;
                    if (spa) d.dataset.refresh ? spa.refresh(d.dataset.refresh, d.dataset.reloadUrl) : spa.reload();
                } else {
                    showToast(data.message || 'Action failed.', 'error');
                }
            } catch {
                showToast('Network error. Please try again.', 'error');
            } finally {
                d.classList.remove('is-loading'); d.disabled = false;
            }
        }
    });

    /* ✅ ADDED: Grid search + repeater rows ───────── */
    const num = n => n.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });

    function recalcRepeater(box) {
        let total = 0;
        box.querySelectorAll('.repeater-row').forEach(row => {
            const line = (parseFloat(row.querySelector('[data-qty]')?.value) || 0) * (parseFloat(row.querySelector('[data-cost]')?.value) || 0);
            total += line;
            const out = row.querySelector('[data-line]'); if (out) out.textContent = num(line);
        });
        const t = box.querySelector('[data-repeater-total]'); if (t) t.textContent = num(total);
    }

    document.addEventListener('input', e => {
        const f = e.target.closest('[data-table-filter]');
        if (f) {
            const q = f.value.trim().toLowerCase();
            document.querySelectorAll(`${f.dataset.tableFilter} tbody tr`).forEach(tr => { tr.hidden = !!q && !tr.textContent.toLowerCase().includes(q); });
        }
        const box = e.target.closest('[data-repeater]');
        if (box) recalcRepeater(box);
    });

    document.addEventListener('click', e => {
        const add = e.target.closest('[data-repeater-add]');
        if (add) {
            const box = document.querySelector(add.dataset.repeaterAdd);
            const rows = box.querySelector('.repeater-rows');
            const i = Number(box.dataset.next || rows.children.length);
            box.dataset.next = String(i + 1);
            rows.insertAdjacentHTML('beforeend', box.querySelector('template').innerHTML.replaceAll('__i__', i));
            rows.lastElementChild.querySelector('select, input:not([type=hidden])')?.focus();
            return;
        }
        const rm = e.target.closest('[data-repeater-remove]');
        if (rm) {
            const box = rm.closest('[data-repeater]');
            if (box.querySelectorAll('.repeater-row').length > 1) { rm.closest('.repeater-row').remove(); recalcRepeater(box); }
            else showToast('At least one item is required.', 'warning');
        }
    });

    /* ── Formatters ────────────────────────────────── */
    function formatNumber(value, decimals) {
        if (value == null) return '0';
        return Number(value).toLocaleString('en-US', { minimumFractionDigits: decimals ?? 0, maximumFractionDigits: decimals ?? 0 });
    }
    function formatDate(dateString) {
        if (!dateString) return '';
        return new Date(dateString).toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: '2-digit' });
    }
    function timeAgo(dateString) {
        if (!dateString) return '';
        const seconds = Math.floor((new Date() - new Date(dateString)) / 1000);
        const units = { year: 31536000, month: 2592000, week: 604800, day: 86400, hour: 3600, minute: 60 };
        for (const [unit, sec] of Object.entries(units)) {
            const n = Math.floor(seconds / sec);
            if (n >= 1) return n + ' ' + unit + (n > 1 ? 's' : '') + ' ago';
        }
        return 'just now';
    }

    /* ── Exports ───────────────────────────────────── */
    window.FoodOps = Object.assign(window.FoodOps || {}, {
        ui: {
            toast: showToast, tick: showSuccessTick, openModal, setModalContent,
            closeModal: () => closeModal(), confirm: confirmDialog, parseValidation, csrf: csrfToken
        },
        fmt: { number: formatNumber, date: formatDate, timeAgo }
    });

    // purane code ke liye global aliases
    window.showToast = showToast;
    window.showSuccessTick = showSuccessTick;
    window.openModal = openModal;
    window.closeModal = () => closeModal();
    window.formatNumber = formatNumber;
    window.formatDate = formatDate;
    window.timeAgo = timeAgo;
})();