using LCTool.Osservazione.Data;
using LCTool.Osservazione.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LCTool.Osservazione.Services
{
    public class OsservazioneService : IOsservazioneService
    {
        private readonly AppDbContext _db;
        public OsservazioneService(AppDbContext db) => _db = db;

        public async Task<List<OsservazioneDto>> GetAllAsync()
            => (await _db.Osservazioni.OrderByDescending(o => o.CreatedAt).ToListAsync())
               .Select(ToDto).ToList();

        public async Task<OsservazioneDto?> GetByIdAsync(int id)
        {
            var e = await _db.Osservazioni.FindAsync(id);
            return e is null ? null : ToDto(e);
        }

        public async Task<string> GetNextNumeroAsync()
        {
            var last = await _db.Osservazioni
                .OrderByDescending(o => o.Id)
                .FirstOrDefaultAsync();

            if (last is null) return "1";

            if (int.TryParse(last.Numero, out int lastNum))
                return (lastNum + 1).ToString();

            int count = await _db.Osservazioni.CountAsync();
            return (count + 1).ToString();
        }

        public async Task<OsservazioneDto> CreateAsync(OsservazioneCreateDto dto, string utente)
        {
            var e = new Models.Osservazione
            {
                Numero                = await GetNextNumeroAsync(),
                Stato                 = dto.Stato,
                IsoRiferimento        = dto.IsoRiferimento,
                ItemRiferimento       = dto.ItemRiferimento,
                RilevataDa            = dto.RilevataDa,
                DataApertura          = dto.DataApertura,
                DataPrevistaChiusura  = dto.DataPrevistaChiusura,
                DataEffettivaChiusura = dto.DataEffettivaChiusura,
                Descrizione           = dto.Descrizione,
                Causa                 = dto.Causa,
                AzioniCorrettive      = dto.AzioniCorrettive,
                DocSgqModificati      = dto.DocSgqModificati,
                Owner                 = dto.Owner,
                DataRevisione         = dto.DataRevisione,
                NoteRevisione         = dto.NoteRevisione,
                CreatedAt             = DateTime.UtcNow,
                UpdatedAt             = DateTime.UtcNow,
                CreatoDa              = utente,
                ModificatoDa          = utente,
                RevisionatoDa         = HasRevisione(dto.DataRevisione, dto.NoteRevisione) ? utente : null
            };
            _db.Osservazioni.Add(e);
            await _db.SaveChangesAsync();
            return ToDto(e);
        }

        public async Task<OsservazioneDto?> UpdateAsync(int id, OsservazioneCreateDto dto, string utente)
        {
            var e = await _db.Osservazioni.FindAsync(id);
            if (e is null) return null;

            
            var revisionePrima    = HasRevisione(e.DataRevisione, e.NoteRevisione);
            var revisioneDopo     = HasRevisione(dto.DataRevisione, dto.NoteRevisione);
            var revisioneCambiata = e.DataRevisione != dto.DataRevisione
                                 || e.NoteRevisione != dto.NoteRevisione;

            e.Stato                 = dto.Stato;
            e.IsoRiferimento        = dto.IsoRiferimento;
            e.ItemRiferimento       = dto.ItemRiferimento;
            e.RilevataDa            = dto.RilevataDa;
            e.DataApertura          = dto.DataApertura;
            e.DataPrevistaChiusura  = dto.DataPrevistaChiusura;
            e.DataEffettivaChiusura = dto.DataEffettivaChiusura;
            e.Descrizione           = dto.Descrizione;
            e.Causa                 = dto.Causa;
            e.AzioniCorrettive      = dto.AzioniCorrettive;
            e.DocSgqModificati      = dto.DocSgqModificati;
            e.Owner                 = dto.Owner;
            e.DataRevisione         = dto.DataRevisione;
            e.NoteRevisione         = dto.NoteRevisione;
            e.UpdatedAt             = DateTime.UtcNow;
            e.ModificatoDa          = utente;

            if (revisioneDopo && (revisioneCambiata || !revisionePrima))
                e.RevisionatoDa = utente;

            await _db.SaveChangesAsync();
            return ToDto(e);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _db.Osservazioni.FindAsync(id);
            if (e is null) return false;
            _db.Osservazioni.Remove(e);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<RilevataDaDto>> GetRilevataDaOptionsAsync()
            => await _db.OsservazioniRilevataDa
                .Where(r => r.FlagAttivo)
                .OrderBy(r => r.Id)
                .Select(r => new RilevataDaDto { Id = r.Id, Valore = r.Valore })
                .ToListAsync();

        public async Task<IEnumerable<object>> GetStatiOptionsAsync()
        {
            return await _db.OsservazioniStati
                .Where(s => s.FlagAttivo)
                .OrderBy(s => s.Id)
                .Select(s => new { s.Id, s.Nome })
                .ToListAsync<object>();
        }


        private static bool HasRevisione(DateTime? dataRevisione, string? noteRevisione)
            => (dataRevisione.HasValue && dataRevisione.Value.Year > 1900)
               || !string.IsNullOrWhiteSpace(noteRevisione);

        private static OsservazioneDto ToDto(Models.Osservazione o) => new()
        {
            Id                    = o.Id,
            Numero                = o.Numero,
            Stato                 = o.Stato,
            IsoRiferimento        = o.IsoRiferimento,
            ItemRiferimento       = o.ItemRiferimento,
            RilevataDa            = o.RilevataDa,
            DataApertura          = o.DataApertura,
            DataPrevistaChiusura  = o.DataPrevistaChiusura,
            DataEffettivaChiusura = o.DataEffettivaChiusura,
            Descrizione           = o.Descrizione,
            Causa                 = o.Causa,
            AzioniCorrettive      = o.AzioniCorrettive,
            DocSgqModificati      = o.DocSgqModificati,
            Owner                 = o.Owner,
            DataRevisione         = o.DataRevisione,
            NoteRevisione         = o.NoteRevisione,
            CreatedAt             = o.CreatedAt,
            UpdatedAt             = o.UpdatedAt,
            CreatoDa              = o.CreatoDa,
            ModificatoDa          = o.ModificatoDa,
            RevisionatoDa         = o.RevisionatoDa
        };
    }
}
