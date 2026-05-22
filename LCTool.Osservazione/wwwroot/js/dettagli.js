document.addEventListener('DOMContentLoaded', async () => {
    const id = window.OBS_ID;
    if (!id) { toast('ID non specificato', 'error'); return; }
    try {
        const res = await fetch(API_OBS_BY_ID(id));
        if (!res.ok) throw new Error('Osservazione non trovata');
        renderDettagli(await res.json());
    } catch (err) {
        document.getElementById('dettagliTitolo').textContent = 'Errore';
        document.getElementById('dettagliContent').innerHTML =
            `<p style="color:red">${escHtml(err.message)}</p>`;
    }
});

function renderDettagli(o) {
    document.getElementById('dettagliTitolo').textContent = `Dettagli — ${escHtml(o.numero)}`;

    document.getElementById('dettagliContent').innerHTML = `
        <div class="dettagli-section">
            <div class="dettagli-section-title">Intestazione</div>
            <div class="dettagli-grid">
                <div class="dettagli-kv"><span class="dk">Numero</span><span class="dv">${escHtml(o.numero)}</span></div>
                <div class="dettagli-kv"><span class="dk">Stato</span><span class="dv">${escHtml(o.stato || '—')}</span></div>
                <div class="dettagli-kv"><span class="dk">ISO / Norma</span><span class="dv">${escHtml(o.isoRiferimento || '—')}</span></div>
                <div class="dettagli-kv"><span class="dk">Item di Riferimento</span><span class="dv">${escHtml(o.itemRiferimento || '—')}</span></div>
                <div class="dettagli-kv"><span class="dk">Rilevata da</span><span class="dv">${escHtml(o.rilevataDa || '—')}</span></div>
                <div class="dettagli-kv"><span class="dk">Data Apertura</span><span class="dv">${fmtDate(o.dataApertura)}</span></div>
                <div class="dettagli-kv"><span class="dk">Data Prevista Chiusura</span><span class="dv">${fmtDate(o.dataPrevistaChiusura)}</span></div>
                <div class="dettagli-kv"><span class="dk">Data Effettiva Chiusura</span><span class="dv">${fmtDate(o.dataEffettivaChiusura)}</span></div>
                <div class="dettagli-kv"><span class="dk">Creato da</span><span class="dv">${escHtml(o.creatoDa || '—')}</span></div>
                <div class="dettagli-kv"><span class="dk">Revisionato da</span><span class="dv">${escHtml(o.revisionatoDa || '—')}</span></div>
            </div>
        </div>
        <div class="dettagli-section">
            <div class="dettagli-section-title">Dettagli CNC</div>
            <div class="dettagli-grid">
                <div class="dettagli-kv full"><span class="dk">Descrizione</span><span class="dv">${escHtml(o.descrizione || '—')}</span></div>
                <div class="dettagli-kv full"><span class="dk">Causa</span><span class="dv">${escHtml(o.causa || '—')}</span></div>
                <div class="dettagli-kv"><span class="dk">Owner</span><span class="dv">${escHtml(o.owner || '—')}</span></div>
                <div class="dettagli-kv full"><span class="dk">Azioni Correttive</span><span class="dv">${escHtml(o.azioniCorrettive || '—')}</span></div>
                <div class="dettagli-kv full"><span class="dk">Doc SGQ Modificati</span><span class="dv">${escHtml(o.docSgqModificati || '—')}</span></div>
            </div>
        </div>
        ${(o.dataRevisione && new Date(o.dataRevisione).getFullYear() > 1900) || o.noteRevisione ? `
        <div class="dettagli-section">
            <div class="dettagli-section-title">Revisione</div>
            <div class="dettagli-grid">
                <div class="dettagli-kv"><span class="dk">Data Revisione</span><span class="dv">${fmtDate(o.dataRevisione)}</span></div>
                <div class="dettagli-kv full"><span class="dk">Note Revisione</span><span class="dv">${escHtml(o.noteRevisione || '—')}</span></div>
            </div>
        </div>` : ''}`;
}
