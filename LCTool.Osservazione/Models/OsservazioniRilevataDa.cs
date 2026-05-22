namespace LCTool.Osservazione.Models
{
    public class OsservazioniRilevataDa
    {
        public int Id { get; set; }
        public string Valore { get; set; } = string.Empty;
        public bool FlagAttivo { get; set; }
    }
}