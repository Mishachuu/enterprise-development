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
    private readonly ClientService _clientService = clientService;

    /// <summary>
    /// получить список всех клиентов
    /// </summary>
    /// <returns>список клиентов в виде ClientDto</returns>
    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> GetClients()
    {
        var clients = await _clientService.GetAllClients();
        return Ok(clients);
    }

    /// <summary>
    /// получить клиента по его идентификатору
    /// </summary>
    /// <param name="id">идентификатор клиента</param>
    /// <returns>клиент в виде ClientDto</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClientDto>> GetClient(int id)
    {
        var clients = await _clientService.GetClientsByPredicate(c => c.ClientId == id);
        if (clients == null || clients.Count == 0)
        {
            return NotFound();
        }

        return Ok(clients.First());
    }

    /// <summary>
    /// добавить нового клиента
    /// </summary>
    /// <param name="clientDto">объект клиента в виде ClientDto</param>
    /// <returns>результат выполнения операции</returns>
    [HttpPost]
    public async Task<ActionResult> AddClient([FromBody] ClientDto clientDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _clientService.AddClient(clientDto);
        return Ok();
    }

    /// <summary>
    /// обновить существующего клиента
    /// </summary>
    /// <param name="id">идентификатор клиента</param>
    /// <param name="clientDto">объект клиента с обновленными данными</param>
    /// <returns>результат выполнения операции</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateClient(int id, [FromBody] ClientDto clientDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _clientService.UpdateClient(id, clientDto);
        return NoContent();
    }

    /// <summary>
    /// удалить клиента по его идентификатору
    /// </summary>
    /// <param name="id">идентификатор клиента</param>
    /// <returns>результат выполнения операции</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteClient(int id)
    {
        var clients = await _clientService.GetClientsByPredicate(c => c.ClientId == id);
        if (clients == null || clients.Count == 0)
        {
            return NotFound();
        }

        await _clientService.DeleteClient(id);
        return NoContent();
    }
}
