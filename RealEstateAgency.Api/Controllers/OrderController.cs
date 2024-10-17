namespace RealEstateAgency.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(OrderService orderService) : ControllerBase
{
    private readonly OrderService _orderService = orderService;

    /// <summary>
    /// получить список всех заказов
    /// </summary>
    /// <returns>список заказов в виде OrderDto</returns>
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
        var orders = await _orderService.GetAllOrders();
        return Ok(orders);
    }

    /// <summary>
    /// получить заказ по его идентификатору
    /// </summary>
    /// <param name="id">идентификатор заказа</param>
    /// <returns>заказ в виде OrderDto</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var orders = await _orderService.GetOrdersByPredicate(o => o.Id == id);
        if (orders == null || orders.Count == 0)
        {
            return NotFound();
        }

        return Ok(orders.First());
    }

    /// <summary>
    /// добавить новый заказ
    /// </summary>
    /// <param name="orderDto">объект заказа в виде OrderDto</param>
    /// <returns>результат выполнения операции</returns>
    [HttpPost]
    public async Task<ActionResult> AddOrder([FromBody] OrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _orderService.AddOrder(orderDto);
        return Ok();
    }

    /// <summary>
    /// обновить данные существующего заказа
    /// </summary>
    /// <param name="id">идентификатор заказа</param>
    /// <param name="orderDto">объект заказа с обновленными данными</param>
    /// <returns>результат выполнения операции</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _orderService.UpdateOrder(id, orderDto);
        return NoContent();
    }

    /// <summary>
    /// удалить заказ по его идентификатору
    /// </summary>
    /// <param name="id">идентификатор заказа</param>
    /// <returns>результат выполнения операции</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var orders = await _orderService.GetOrdersByPredicate(o => o.Id == id);
        if (orders == null || orders.Count == 0)
        {
            return NotFound();
        }

        await _orderService.DeleteOrder(id);
        return NoContent();
    }
}
