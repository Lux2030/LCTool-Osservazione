let ownerList = [];
let currentStep = 1;

document.addEventListener('DOMContentLoaded', init);

async function init() {
    suppressAutocomplete();
    bindEvents();
    setMaxDataApertura();
    setMinDateChiusura();
    await loadDropdowns();

    if (window.OBS_EDIT_ID) {
        await loadOsservazioneEsistente(window.OBS_EDIT_ID);
    } else {
        await caricaProssimoNumero();
    }
}

function bindEvents() {
    document.getElementById('btnStep1Next').addEventListener('click', step1Next);
    document.getElementById('btnStep2Back').addEventListener('click', () => goToStep(1));
    document.getElementById('btnStep2Next').addEventListener('click', step2Next);
    document.getElementById('btnStep3Back').addEventListener('click', () => goToStep(2));
    document.getElementById('btnSalva').addEventListener('click', salva);

    document.getElementById('ownerInput').addEventListener('keydown', e => {
        if (e.key === 'Enter' || e.key === ',') {
            e.preventDefault();
            const v = e.target.value.trim();
            if (v && !ownerList.includes(v)) { ownerList.push(v); renderOwnerTags(); }
            e.target.value = '';
        }
    });
    document.getElementById('ownerTags').addEventListener('click', e => {
        if (e.target.classList.contains('tag-remove')) {
            ownerList.splice(+e.target.dataset.i, 1);
            renderOwnerTags();
        }
    });
    document.getElementById('ownerWrap').addEventListener('click', () =>
        document.getElementById('ownerInput').focus());
}

function suppressAutocomplete() {
    document.querySelectorAll('input').forEach(el => {
        if (el.type === 'hidden' || el.type === 'checkbox') return;
        el.setAttribute('autocomplete', 'new-password');
    });
}

function setMaxDataApertura()  { document.getElementById('dataApertura').setAttribute('max', todayISO()); }
function setMinDateChiusura()  { document.getElementById('dataPrevistaChiusura').setAttribute('min', todayISO()); }

async function loadDropdowns() {
    try {
        const stati = await (await fetch(API_OBS_STATI)).json();
        const sel = document.getElementById('stato');
        stati.forEach(v => {
            const o = document.createElement('option');
            o.value = String(v.nome ?? v); o.textContent = String(v.nome ?? v);
            sel.appendChild(o);
        });
    } catch { toast('Errore caricamento stati', 'error'); }

    try {
        const rilev = await (await fetch(API_OBS_RILEVATA_DA)).json();
        const sel = document.getElementById('rilevataDa');
        rilev.forEach(v => {
            const o = document.createElement('option');
            o.value = v.valore; o.textContent = v.valore;
            sel.appendChild(o);
        });
    } catch { toast('Errore caricamento rilevata da', 'error'); }
}

async function caricaProssimoNumero(tentativo = 0) {
    try {
        const d = await (await fetch(API_OBS_NEXT_NUMERO)).json();
        document.getElementById('obsNumeroDisplay').textContent = d.numero;
        document.getElementById('numeroBadge').style.display = '';
        document.getElementById('numeroObsDisplay').value = d.numero;
    } catch {
        if (tentativo < 5) setTimeout(() => caricaProssimoNumero(tentativo + 1), 800);
    }
}

function step1Next() {
    if (!validateStep1()) {
        toast('Compila tutti i campi obbligatori (*)', 'error');
        setChipError(1);
        document.querySelector('#step1 .field-error')?.scrollIntoView({ behavior: 'smooth', block: 'center' });
        return;
    }
    goToStep(2);
}

function step2Next() {
    if (!validateStep2()) {
        toast('Compila tutti i campi obbligatori (*)', 'error');
        setChipError(2);
        document.querySelector('#step2 .field-error')?.scrollIntoView({ behavior: 'smooth', block: 'center' });
        return;
    }
    populateRiepilogo();
    goToStep(3);
}

function goToStep(n) {
    currentStep = n;
    [1, 2, 3].forEach(i => {
        document.getElementById(`step${i}`).style.display = (i === n) ? '' : 'none';
        const chip = document.querySelector(`.stepchip[data-step="${i}"]`);
        chip.classList.remove('active', 'done', 'error');
        if (i === n) chip.classList.add('active');
        if (i < n)  chip.classList.add('done');
    });
    window.scrollTo({ top: 0, behavior: 'smooth' });
}
window.goToStep = goToStep;

function setChipError(step) {
    const chip = document.querySelector(`.stepchip[data-step="${step}"]`);
    if (chip) { chip.classList.remove('active', 'done'); chip.classList.add('error'); }
}

function clearErrors() {
    document.querySelectorAll('.field-error').forEach(el => el.classList.remove('field-error'));
    document.querySelectorAll('.field-error-msg').forEach(el => el.remove());
}

function showError(wrap, msg) {
    if (!wrap) return;
    wrap.classList.add('field-error');
    if (!wrap.querySelector('.field-error-msg')) {
        const span = document.createElement('span');
        span.className = 'field-error-msg'; span.textContent = msg;
        wrap.appendChild(span);
    }
}

function getWrap(id) {
    const el = document.getElementById(id);
    return el ? el.closest('div') : null;
}

function validateStep1() {
    clearErrors(); let ok = true;
    [['stato', 'Stato'], ['isoRiferimento', 'ISO / Norma'], ['itemRiferimento', 'Item di Riferimento'],
     ['rilevataDa', 'Rilevata da'], ['dataApertura', 'Data Apertura'],
     ['dataPrevistaChiusura', 'Data Prevista Chiusura']].forEach(([id, lbl]) => {
        const el = document.getElementById(id);
        if (!el || !el.value.trim()) { showError(getWrap(id), `${lbl} è obbligatorio`); ok = false; }
    });
    const da = document.getElementById('dataApertura').value;
    if (da && da > todayISO()) { showError(getWrap('dataApertura'), 'La Data Apertura non può essere futura'); ok = false; }
    return ok;
}

function validateStep2() {
    clearErrors(); let ok = true;
    [['descrizione', 'Descrizione'], ['causa', 'Causa']].forEach(([id, lbl]) => {
        const el = document.getElementById(id);
        if (!el || !el.value.trim()) { showError(getWrap(id), `${lbl} è obbligatorio`); ok = false; }
    });
    if (!ownerList.length) {
        showError(document.getElementById('ownerWrap').closest('div'), 'Owner è obbligatorio');
        ok = false;
    }
    return ok;
}

function populateRiepilogo() {
    const g = id => document.getElementById(id), gv = id => g(id)?.value || '—';
    g('r_numero')   .textContent = g('numeroObsDisplay').value || '—';
    g('r_stato')    .textContent = gv('stato');
    g('r_iso')      .textContent = gv('isoRiferimento');
    g('r_item')     .textContent = gv('itemRiferimento');
    g('r_rilevata') .textContent = gv('rilevataDa');
    g('r_dataAp')   .textContent = fmtDate(gv('dataApertura'));
    g('r_dataPrev') .textContent = fmtDate(gv('dataPrevistaChiusura'));
    g('r_dataEff')  .textContent = fmtDate(gv('dataEffettivaChiusura'));
    g('r_desc')     .textContent = gv('descrizione');
    g('r_causa')    .textContent = gv('causa');
    g('r_owner')    .textContent = ownerList.length ? ownerList.join(', ') : '—';
    g('r_azioni')   .textContent = gv('azioniCorrettive');
    g('r_docsgq')   .textContent = gv('docSgqModificati');
    g('r_dataRev')  .textContent = fmtDate(gv('dataRevisione'));
    g('r_noteRev')  .textContent = gv('noteRevisione');

    const hasRev = gv('dataRevisione') !== '—' || gv('noteRevisione') !== '—';
    document.getElementById('riepilogoRevizioneSection').style.display = hasRev ? '' : 'none';
}

function renderOwnerTags() {
    const c = document.getElementById('ownerTags');
    c.innerHTML = '';
    ownerList.forEach((name, i) => {
        const tag = document.createElement('span');
        tag.className = 'tag';
        tag.innerHTML = `${escHtml(name)} <span class="tag-remove" data-i="${i}">×</span>`;
        c.appendChild(tag);
    });
    document.getElementById('ownerValue').value = ownerList.join(',');
}

function collectForm() {
    const g = id => document.getElementById(id);
    return {
        stato:                 g('stato').value,
        isoRiferimento:        g('isoRiferimento').value.trim() || null,
        itemRiferimento:       g('itemRiferimento').value.trim() || null,
        rilevataDa:            g('rilevataDa').value || null,
        dataApertura:          g('dataApertura').value || null,
        dataPrevistaChiusura:  g('dataPrevistaChiusura').value || null,
        dataEffettivaChiusura: g('dataEffettivaChiusura').value || null,
        descrizione:           g('descrizione').value.trim() || null,
        causa:                 g('causa').value.trim() || null,
        azioniCorrettive:      g('azioniCorrettive').value.trim() || null,
        docSgqModificati:      g('docSgqModificati').value.trim() || null,
        owner:                 ownerList.length ? ownerList.join(', ') : null,
        dataRevisione:         g('dataRevisione').value || null,
        noteRevisione:         g('noteRevisione').value.trim() || null
    };
}

async function loadOsservazioneEsistente(id) {
    try {
        const res = await fetch(API_OBS_BY_ID(id));
        if (!res.ok) throw new Error('Osservazione non trovata');
        const o = await res.json();
        populateForm(o);
        document.getElementById('revizioneSection').style.display = '';
        document.getElementById('riepilogoRevizioneSection').style.display = '';
        goToStep(1);
    } catch (err) { toast('Errore: ' + err.message, 'error'); }
}

function populateForm(o) {
    const g = id => document.getElementById(id);
    g('obsId').value = o.id;
    g('stato').value = o.stato || '';
    g('isoRiferimento').value = o.isoRiferimento || '';
    g('itemRiferimento').value = o.itemRiferimento || '';
    g('rilevataDa').value = o.rilevataDa || '';
    g('dataApertura').value = o.dataApertura ? o.dataApertura.slice(0, 10) : '';
    g('dataPrevistaChiusura').value = o.dataPrevistaChiusura ? o.dataPrevistaChiusura.slice(0, 10) : '';
    g('dataEffettivaChiusura').value = o.dataEffettivaChiusura ? o.dataEffettivaChiusura.slice(0, 10) : '';
    g('descrizione').value = o.descrizione || '';
    g('causa').value = o.causa || '';
    g('azioniCorrettive').value = o.azioniCorrettive || '';
    g('docSgqModificati').value = o.docSgqModificati || '';
    g('dataRevisione').value = o.dataRevisione ? o.dataRevisione.slice(0, 10) : '';
    g('noteRevisione').value = o.noteRevisione || '';
    ownerList = o.owner ? o.owner.split(',').map(s => s.trim()).filter(Boolean) : [];
    renderOwnerTags();
    g('obsNumeroDisplay').textContent = o.numero;
    g('numeroBadge').style.display = '';
    g('numeroObsDisplay').value = o.numero;
    clearErrors();
}

async function salva() {
    const id = document.getElementById('obsId').value;
    const data = collectForm();
    try {
        const res = id
            ? await fetch(API_OBS_BY_ID(id), { method: 'PUT',  headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) })
            : await fetch(API_OBS,            { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data) });
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        const saved = await res.json();
        document.getElementById('successoTitolo').textContent =
            id ? 'Revisione salvata con successo' : `${saved.numero}^ Osservazione compilata con successo`;
        document.querySelectorAll('.steps, #obsForm, #numeroBadge').forEach(el => el && (el.style.display = 'none'));
        document.getElementById('panelSuccesso').style.display = '';
    } catch (err) { toast('Errore: ' + err.message, 'error'); }
}
