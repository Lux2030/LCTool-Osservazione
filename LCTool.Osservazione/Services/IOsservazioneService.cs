using LCTool.Osservazione.DTOs;

namespace LCTool.Osservazione.Services
{
    public interface IOsservazioneService
    {
        Task<List<OsservazioneDto>>  GetAllAsync();
        Task<OsservazioneDto?>       GetByIdAsync(int id);
        Task<string>                 GetNextNumeroAsync();
        Task<OsservazioneDto>        CreateAsync(OsservazioneCreateDto dto, string utente);
        Task<OsservazioneDto?>       UpdateAsync(int id, OsservazioneCreateDto dto, string utente);
        Task<bool>                   DeleteAsync(int id);
        Task<List<RilevataDaDto>>    GetRilevataDaOptionsAsync();
        Task<IEnumerable<object>>    GetStatiOptionsAsync();
    }
}
