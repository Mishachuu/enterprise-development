namespace RealEstateAgency.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ClientsController(ClientService clientService) : ControllerBase
{
    /// <summary>
    /// получить список всех клиентов
    /// </summary>
    /// <returns>список клиентов в виде ClientGetDto</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<ActionResult<List<ClientGetDto>>> GetClients()
    {
        var clients = await clientService.GetAllClients();
        return Ok(clients);
    }

    /// <summary>
    /// получить клиента по его идентификатору
    /// </summary>
    /// <param name="id">идентификатор клиента</param>
    /// <returns>клиент в виде ClientGetDto</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<ClientGetDto>> GetClient(int id)
    {
        var clients = await clientService.GetClientsByPredicate(c => c.Id == id);
        if (clients == null || clients.Count == 0)
        {
            return NotFound("Клиент не найден");
        }

        return Ok(clients.First());
    }

    /// <summary>
    /// добавить нового клиента
    /// </summary>
    /// <param name="clientDto">объект клиента в виде ClientDto</param>
    /// <returns>результат выполнения операции</returns>
    [HttpPost]
    [ProducesResponseType(400)]
    [ProducesResponseType(200)]
    public async Task<ActionResult> AddClient([FromBody] ClientDto clientDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Данные не корректны.");
        }

        await clientService.AddClient(clientDto);
        return Ok();
    }

    /// <summary>
    /// обновить существующего клиента
    /// </summary>
    /// <param name="id">идентификатор клиента</param>
    /// <param name="clientDto">объект клиента с обновленными данными</param>
    /// <returns>результат выполнения операции</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> UpdateClient(int id, [FromBody] ClientDto clientDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Данные не корректны.");
        }
        await clientService.UpdateClient(id, clientDto);
        return NoContent();
    }

    /// <summary>
    /// удалить клиента по его идентификатору
    /// </summary>
    /// <param name="id">идентификатор клиента</param>
    /// <returns>результат выполнения операции</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> DeleteClient(int id)
    {
        try
        {
            var clients = await clientService.GetClientsByPredicate(c => c.Id == id);
            if (clients == null || clients.Count == 0)
            {
                return NotFound("Клиент не найден.");
            }

            await clientService.DeleteClient(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка при выполнении запроса.");

        }
    }
}
