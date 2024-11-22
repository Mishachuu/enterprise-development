namespace RealEstateAgency.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Api.Services;

[Route("api/[controller]")]
[ApiController]
public class RealEstatesController(RealEstateService realEstateService) : ControllerBase
{
    /// <summary>
    /// получить список всех объектов недвижимости
    /// </summary>
    /// <returns>список объектов недвижимости в виде RealEstateGetDto</returns>
    [HttpGet]
    public async Task<ActionResult<List<RealEstateGetDto>>> GetRealEstates()
    {
        var realEstates = await realEstateService.GetAllRealEstates();
        return Ok(realEstates);
    }

    /// <summary>
    /// получить объект недвижимости по его идентификатору
    /// </summary>
    /// <param name="id">идентификатор объекта недвижимости</param>
    /// <returns>объект недвижимости в виде RealEstateGetDto</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<RealEstateDto>> GetRealEstate(int id)
    {
        var realEstates = await realEstateService.GetRealEstatesByPredicate(r => r.Id == id);
        if (realEstates == null || realEstates.Count == 0)
        {
            return NotFound($"Объект недвижимости с идентификатором {id} не найден.");
        }

        return Ok(realEstates.First());
    }

    /// <summary>
    /// Добавить новый объект недвижимости.
    /// </summary>
    /// <param name="realEstateDto">
    /// Значения для свойства <c>PropertyType</c> могут быть следующими:
    /// <list type="bullet">
    /// <item>
    /// <term> Residential</term>
    /// </item>
    /// <item>
    /// <term> Uninhabitable</term>
    /// </item>
    /// </list>
    /// </param>
    /// <returns> Созданный объект недвижимости</returns>
    [HttpPost]
    public async Task<ActionResult> AddRealEstate([FromBody] RealEstateDto realEstateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Данные не корректны.");
        }

        await realEstateService.AddRealEstate(realEstateDto);
        return Ok();
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
            return BadRequest("Данные не корректны.");
        }

        var realEstates = await realEstateService.GetRealEstatesByPredicate(r => r.Id == id);
        if (realEstates == null || realEstates.Count == 0)
        {
            return NotFound($"Объект недвижимости с идентификатором {id} не найден.");
        }

        await realEstateService.UpdateRealEstate(id, realEstateDto);
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
        try
        {
            var realEstates = await realEstateService.GetRealEstatesByPredicate(r => r.Id == id);
            if (realEstates == null || realEstates.Count == 0)
            {
                return NotFound($"Объект недвижимости с идентификатором {id} не найден.");
            }
            await realEstateService.DeleteRealEstate(id);
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Ошибка при выполнении запроса.");
        }
    }
}
