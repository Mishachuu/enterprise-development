namespace RealEstateAgency.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Api.Services;

[Route("api/[controller]")]
[ApiController]
public class RealEstatesController(RealEstateService realEstateService) : ControllerBase
{
    private readonly RealEstateService _realEstateService = realEstateService;

    /// <summary>
    /// получить список всех объектов недвижимости
    /// </summary>
    /// <returns>список объектов недвижимости в виде RealEstateDto</returns>
    [HttpGet]
    public async Task<ActionResult<List<RealEstateDto>>> GetRealEstates()
    {
        var realEstates = await _realEstateService.GetAllRealEstates();
        return Ok(realEstates);
    }

    /// <summary>
    /// получить объект недвижимости по его идентификатору
    /// </summary>
    /// <param name="id">идентификатор объекта недвижимости</param>
    /// <returns>объект недвижимости в виде RealEstateDto</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<RealEstateDto>> GetRealEstate(int id)
    {
        var realEstates = await _realEstateService.GetRealEstatesByPredicate(r => r.Id == id);
        if (realEstates == null || realEstates.Count == 0)
        {
            return NotFound($"Объект недвижимости с идентификатором {id} не найден.");
        }

        return Ok(realEstates.First());
    }

    /// <summary>
    /// добавить новый объект недвижимости
    /// </summary>
    /// <param name="realEstateDto">объект недвижимости в виде RealEstateDto</param>
    /// <returns>созданный объект недвижимости</returns>
    [HttpPost]
    public async Task<ActionResult> AddRealEstate([FromBody] RealEstateDto realEstateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _realEstateService.AddRealEstate(realEstateDto);
        return CreatedAtAction(nameof(GetRealEstate), new { id = realEstateDto.Id }, realEstateDto);
    }

    /// <summary>
    /// обновить данные существующего объекта недвижимости
    /// </summary>
    /// <param name="id">идентификатор объекта недвижимости</param>
    /// <param name="realEstateDto">объект недвижимости с обновленными данными</param>
    /// <returns>результат выполнения операции</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateRealEstate(int id, [FromBody] RealEstateDto realEstateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != realEstateDto.Id)
        {
            return BadRequest("Идентификатор в URL и в теле запроса не совпадают.");
        }

        var realEstates = await _realEstateService.GetRealEstatesByPredicate(r => r.Id == id);
        if (realEstates == null || realEstates.Count == 0)
        {
            return NotFound($"Объект недвижимости с идентификатором {id} не найден.");
        }

        await _realEstateService.UpdateRealEstate(realEstateDto);
        return NoContent();
    }

    /// <summary>
    /// удалить объект недвижимости по его идентификатору
    /// </summary>
    /// <param name="id">идентификатор объекта недвижимости</param>
    /// <returns>результат выполнения операции</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRealEstate(int id)
    {
        var realEstates = await _realEstateService.GetRealEstatesByPredicate(r => r.Id == id);
        if (realEstates == null || realEstates.Count == 0)
        {
            return NotFound($"Объект недвижимости с идентификатором {id} не найден.");
        }
        await _realEstateService.DeleteRealEstate(id);
        return NoContent();
    }
}
