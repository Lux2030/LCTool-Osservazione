namespace LCTool.Osservazione.Models
{
    public class Osservazione
    {
        public int Id { get; set; }

        
        public string Numero { get; set; } = string.Empty;

        // 1^SCHERMATA: INTESTAZIONE
        public string  Stato                  { get; set; } = "Aperto"; 
        public string? IsoRiferimento         { get; set; }              
        public string? ItemRiferimento        { get; set; }              
        public string? RilevataDa             { get; set; }              
        public DateTime  DataApertura          { get; set; }            
        public DateTime? DataPrevistaChiusura  { get; set; }             
        public DateTime? DataEffettivaChiusura { get; set; }            

        // 2^ SCHERMATA: DETTAGLI CNC
        public string? Descrizione      { get; set; }   
        public string? Causa            { get; set; }   
        public string? AzioniCorrettive { get; set; }   
        public string? DocSgqModificati { get; set; }   
        public string? Owner            { get; set; }   
        public DateTime? DataRevisione  { get; set; }   
        public string? NoteRevisione    { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        
        public string? CreatoDa      { get; set; }
        public string? ModificatoDa  { get; set; }
        public string? RevisionatoDa { get; set; }
    }
}
