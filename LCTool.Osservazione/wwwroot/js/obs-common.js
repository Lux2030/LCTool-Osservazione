window.APP_BASE_URL = window.APP_BASE_URL || "/";

window.appUrl = function (path) {
    return new URL(
        String(path).replace(/^\/+/, ""),
        new URL(window.APP_BASE_URL, window.location.origin)
    ).toString();
};

window.API_OBS              = appUrl('api/osservazione');
window.API_OBS_STATI        = appUrl('api/osservazione/lookup/stati');
window.API_OBS_RILEVATA_DA  = appUrl('api/osservazione/lookup/rilevata-da');
window.API_OBS_NEXT_NUMERO  = appUrl('api/osservazione/next-numero');
window.API_OBS_BY_ID        = (id) => appUrl(`api/osservazione/${id}`);

window.todayISO = function () {
    return new Date().toISOString().split('T')[0];
};

window.fmtDate = function (iso) {
    if (!iso) return '—';
    try {
        const d = new Date(iso);
        if (isNaN(d.getTime()) || d.getFullYear() < 1900) return '—';
        return d.toLocaleDateString('it-IT');
    } catch { return '—'; }
};

window.escHtml = function (s) {
    return String(s || '')
        .replace(/&/g, '&amp;').replace(/</g, '&lt;')
        .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
};

window.toast = function (msg, type) {
    const t = document.getElementById('toast');
    if (!t) return;
    t.textContent = msg;
    t.className = 'toast show ' + (type || '');
    setTimeout(() => { t.className = 'toast'; }, 3200);
};
