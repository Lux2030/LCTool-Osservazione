using LCTool.Osservazione.DTOs;
using LCTool.Osservazione.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LCTool.Osservazione.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/osservazione")]
    public class OsservazioneController : ControllerBase
    {
        private readonly IOsservazioneService _svc;
        public OsservazioneController(IOsservazioneService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());

       
        [HttpGet("next-numero")]
        public async Task<IActionResult> GetNextNumero()
            => Ok(new { numero = await _svc.GetNextNumeroAsync() });

        [HttpGet("lookup/rilevata-da")]
        public async Task<IActionResult> GetRilevataDa()
            => Ok(await _svc.GetRilevataDaOptionsAsync());

        [HttpGet("lookup/stati")]
        public async Task<IActionResult> GetStati()
            => Ok(await _svc.GetStatiOptionsAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OsservazioneCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _svc.CreateAsync(dto, GetAuthenticatedUser());
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OsservazioneCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _svc.UpdateAsync(id, dto, GetAuthenticatedUser());
            return result is null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _svc.DeleteAsync(id) ? NoContent() : NotFound();

       
        private string GetAuthenticatedUser()
        {
            var name = User?.Identity?.Name;
            if (User?.Identity?.IsAuthenticated != true || string.IsNullOrWhiteSpace(name))
                return "Utente non autenticato";

            var idx = name.IndexOf('\\');
            if (idx >= 0 && idx < name.Length - 1)
                name = name[(idx + 1)..];

            return name;
        }
    }
}
