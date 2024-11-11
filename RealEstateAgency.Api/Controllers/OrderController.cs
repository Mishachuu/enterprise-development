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
    /// <summary>
    /// получить список всех заказов
    /// </summary>
    /// <returns>список заказов в виде OrderDto</returns>
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
        var orders = await orderService.GetAllOrders();
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
        var orders = await orderService.GetOrdersByPredicate(o => o.Id == id);
        if (orders == null || orders.Count == 0)
        {
            return NotFound("Заказ не найден");
        }
        return Ok(orders.First());
    }

    /// <summary>
    /// Добавить новый заказ.
    /// </summary>
    /// <param name="orderDto">
    /// Объект заказа в виде OrderDto. 
    /// Значения для свойства <c>Type</c> могут быть следующими:
    /// <list type="bullet">
    /// <item>
    /// <term> Purchase</term>
    /// </item>
    /// <item>
    /// <term> Sale</term>
    /// </item>
    /// </list>
    /// </param>
    /// <returns>Результат выполнения операции</returns>

    [HttpPost]
    public async Task<ActionResult> AddOrder([FromBody] OrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Данные не корректны.");
        }
        await orderService.AddOrder(orderDto);
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
            return BadRequest("Данные не корректны.");
        }
        await orderService.UpdateOrder(id, orderDto);
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
        try
        {
            var orders = await orderService.GetOrdersByPredicate(o => o.Id == id);
            if (orders == null || orders.Count == 0)
            {
                return NotFound("Заказ не найден.");
            }

            await orderService.DeleteOrder(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка при выполнении запроса.");
        }
    }
}
