namespace LCTool.Osservazione.DTOs
{
    public class OsservazioneDto
    {
        public int      Id                    { get; set; }
        public string   Numero                { get; set; } = string.Empty;
        public string   Stato                 { get; set; } = string.Empty;
        public string?  IsoRiferimento        { get; set; }
        public string?  ItemRiferimento       { get; set; }
        public string?  RilevataDa            { get; set; }
        public DateTime  DataApertura         { get; set; }
        public DateTime? DataPrevistaChiusura { get; set; }
        public DateTime? DataEffettivaChiusura{ get; set; }
        public string?  Descrizione           { get; set; }
        public string?  Causa                 { get; set; }
        public string?  AzioniCorrettive      { get; set; }
        public string?  DocSgqModificati      { get; set; }
        public string?  Owner                 { get; set; }
        public DateTime? DataRevisione        { get; set; }
        public string?  NoteRevisione         { get; set; }
        public DateTime CreatedAt             { get; set; }
        public DateTime UpdatedAt             { get; set; }

        // Le colonne CreatoDa, ModificatoDa e RevisionatoDa presenti nella tabella dbo.Osservazioni sostituiscono le colonne CreatedUser e UpdatedUser.
        public string?  CreatoDa              { get; set; }
        public string?  ModificatoDa          { get; set; }
        public string?  RevisionatoDa         { get; set; }
    }

    public class OsservazioneCreateDto
    {
        public string   Stato                 { get; set; } = "Aperto";
        public string?  IsoRiferimento        { get; set; }
        public string?  ItemRiferimento       { get; set; }
        public string?  RilevataDa            { get; set; }
        public DateTime  DataApertura         { get; set; }
        public DateTime? DataPrevistaChiusura { get; set; }
        public DateTime? DataEffettivaChiusura{ get; set; }
        public string?  Descrizione           { get; set; }
        public string?  Causa                 { get; set; }
        public string?  AzioniCorrettive      { get; set; }
        public string?  DocSgqModificati      { get; set; }
        public string?  Owner                 { get; set; }
        public DateTime? DataRevisione        { get; set; }
        public string?  NoteRevisione         { get; set; }

  
    }

    public class RilevataDaDto
    {
        public int    Id     { get; set; }
        public string Valore { get; set; } = string.Empty;
    }
}
