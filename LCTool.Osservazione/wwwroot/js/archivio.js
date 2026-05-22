let allObsList = [];

document.addEventListener('DOMContentLoaded', init);

async function init() {
    await popolaFiltriDropdown();
    await loadArchivio();
    document.getElementById('btnRicerca') .addEventListener('click', applicaFiltri);
    document.getElementById('btnPulisci') .addEventListener('click', pulisciFiltri);
    document.getElementById('btnAggiorna').addEventListener('click', loadArchivio);
}

async function popolaFiltriDropdown() {
    try {
        const stati = await (await fetch(API_OBS_STATI)).json();
        const sel = document.getElementById('filtroStato');
        stati.forEach(v => {
            const o = document.createElement('option');
            o.value = String(v.nome ?? v); o.textContent = String(v.nome ?? v);
            sel.appendChild(o);
        });
    } catch { }
    try {
        const rilev = await (await fetch(API_OBS_RILEVATA_DA)).json();
        const sel = document.getElementById('filtroRilevataDa');
        rilev.forEach(v => {
            const o = document.createElement('option');
            o.value = v.valore; o.textContent = v.valore;
            sel.appendChild(o);
        });
    } catch { }
}

async function loadArchivio() {
    const container = document.getElementById('archivioCards');
    container.innerHTML = '<p style="color:#999;text-align:center;padding:40px">Caricamento…</p>';
    try {
        const res = await fetch(API_OBS);
        allObsList = await res.json();
        const btn = document.getElementById('btnCounter');
        document.getElementById('obsCount').textContent = allObsList.length;
        btn.style.display = allObsList.length > 0 ? '' : 'none';
        applicaFiltri();
    } catch (err) {
        container.innerHTML = `<p style="color:red;padding:20px">Errore: ${escHtml(err.message)}</p>`;
    }
}

function hasFiltriCompilati() {
    return document.getElementById('filtroNumero').value.trim() !== ''
        || document.getElementById('filtroStato').value !== ''
        || document.getElementById('filtroRilevataDa').value !== ''
        || document.getElementById('filtroOwner').value.trim() !== ''
        || document.getElementById('filtroDataDal').value !== ''
        || document.getElementById('filtroDataAl').value !== '';
}

function applicaFiltri() {
    const numero  = document.getElementById('filtroNumero').value.trim().toLowerCase();
    const stato   = document.getElementById('filtroStato').value.toLowerCase();
    const rilev   = document.getElementById('filtroRilevataDa').value.toLowerCase();
    const owner   = document.getElementById('filtroOwner').value.trim().toLowerCase();
    const dataDal = document.getElementById('filtroDataDal').value;
    const dataAl  = document.getElementById('filtroDataAl').value;
    const filtriOn = hasFiltriCompilati();

    let filtrata = allObsList.filter(o => {
        if (numero && !String(o.numero).toLowerCase().includes(numero)) return false;
        if (stato  && (o.stato || '').toLowerCase() !== stato) return false;
        if (rilev  && (o.rilevataDa || '').toLowerCase() !== rilev) return false;
        if (owner  && !(o.owner || '').toLowerCase().includes(owner)) return false;
        if (dataDal && o.dataApertura && o.dataApertura.slice(0, 10) < dataDal) return false;
        if (dataAl  && o.dataApertura && o.dataApertura.slice(0, 10) > dataAl) return false;
        return true;
    });

    if (!filtriOn) filtrata = filtrata.slice(0, 5);

    const count = document.getElementById('risultatiCount');
    if (filtriOn) {
        count.textContent = filtrata.length === allObsList.length
            ? `${allObsList.length} Osservazioni totali`
            : `${filtrata.length} di ${allObsList.length} Osservazioni`;
    } else {
        count.textContent = `Ultime 5 di ${allObsList.length} Osservazioni (usa i filtri per vedere tutte)`;
    }
    renderCards(filtrata);
}

function pulisciFiltri() {
    ['filtroNumero', 'filtroOwner', 'filtroDataDal', 'filtroDataAl']
        .forEach(id => document.getElementById(id).value = '');
    document.getElementById('filtroStato').value = '';
    document.getElementById('filtroRilevataDa').value = '';
    applicaFiltri();
}

function renderCards(list) {
    const container = document.getElementById('archivioCards');
    if (!list.length) {
        container.innerHTML = '<p style="color:#999;text-align:center;padding:40px;grid-column:1/-1">Nessuna Osservazione trovata.</p>';
        return;
    }
    const statoClass = (s) => (s || '').toLowerCase() === 'chiuso' ? 'chiusa' : 'aperta';
    container.innerHTML = list.map(o => {
        const urlDettagli  = appUrl(`Dettagli?id=${encodeURIComponent(o.id)}`);
        const urlRevisiona = appUrl(`NuovaOsservazione?id=${encodeURIComponent(o.id)}`);
        return `<div class="nc-card">
            <div class="nc-card-top">
                <span class="nc-card-num">${escHtml(o.numero)}</span>
                <span class="nc-card-stato ${statoClass(o.stato)}">${escHtml(o.stato || '—')}</span>
            </div>
            <div class="nc-card-row"><span class="lbl">ISO / Norma</span><span class="val">${escHtml(o.isoRiferimento || '—')}</span></div>
            <div class="nc-card-row"><span class="lbl">Rilevata da</span><span class="val">${escHtml(o.rilevataDa || '—')}</span></div>
            <div class="nc-card-dates">
                <div class="nc-card-row"><span class="lbl">Data Apertura</span><span class="val">${fmtDate(o.dataApertura)}</span></div>
                <div class="nc-card-row"><span class="lbl">Owner</span><span class="val">${escHtml(o.owner || '—')}</span></div>
            </div>
            <div class="nc-card-row"><span class="lbl">Creato da</span><span class="val">${escHtml(o.creatoDa || '—')}</span></div>
            <div class="nc-card-row"><span class="lbl">Revisionato da</span><span class="val">${escHtml(o.revisionatoDa || '—')}</span></div>
            <div class="nc-card-footer">
                <a class="btn secondary" href="${urlDettagli}">🔍 Dettagli</a>
                <a class="btn primary"   href="${urlRevisiona}">✏️ Revisiona</a>
            </div>
        </div>`;
    }).join('');
}
