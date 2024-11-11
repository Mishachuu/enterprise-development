using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Api.Services;
using RealEstateAgency.Domain;

namespace RealEstateAgency.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnalyticsController(AnalyticsService analyticsService) : ControllerBase
{
    /// <summary>
    /// Вывести сведения о всех клиентах, ищущих недвижимость заданного типа, упорядоченные по ФИО.
    /// </summary>
    /// <param name="type">
    /// Значения для параметра <c>type</c> могут быть следующими:
    /// <list type="bullet">
    /// <item>
    /// <term> Residential</term>
    /// </item>
    /// <item>
    /// <term> Uninhabitable</term>
    /// </item>
    /// </list>
    /// </param>
    [HttpGet("clients-by-realestate-type")]
    public async Task<ActionResult<List<ClientDto>>> GetClientsByRealEstateType(string type)
    {
        if (!Enum.TryParse<RealEstate.PropertyType>(type, true, out var propertyType))
        {
            return BadRequest("Неверный тип недвижимости.");
        }

        try
        {
            var result = await analyticsService.GetClientsByPropertyTypeAsync(propertyType);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка при выполнении запроса.");
        }
    }

    /// <summary>
    /// вывести всех продавцов, оставивших заявки за заданный период.
    /// </summary>
    [HttpGet("sellers-by-period")]
    public async Task<ActionResult<List<ClientDto>>> GetSellersByPeriod(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("Начальная дата не может быть позже конечной.");
        }
        try
        {
            var result = await analyticsService.GetSellersByPeriod(startDate, endDate);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка при выполнении запроса.");

        }
    }

    /// <summary>
    /// вывести сведения о продавцах и объектах недвижимости, соответствующих определенной заявке покупателя.
    /// </summary>
    [HttpGet("matching-sellers-for-buyer/{buyerOrderId}")]
    public async Task<ActionResult<SellerRealEstateDto>> GetSellersForBuyerOrder(int buyerOrderId)
    {
        try
        {
            var result = await analyticsService.GetSellersForBuyerOrder(buyerOrderId);
            return Ok(result);
        }
        catch (ArgumentException)
        {
            return NotFound("Заказ покупателя не найден.");

        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка при выполнении запроса.");

        }
    }

    /// <summary>
    /// вывести информацию о количестве заявок по каждому типу недвижимости.
    /// </summary>
    [HttpGet("order-count-by-type")]
    public async Task<ActionResult<List<RealEstateOrderCountDto>>> GetOrderCountByRealEstateType()
    {
        try
        {
            var result = await analyticsService.GetOrderCountByRealEstateType();
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка при выполнении запроса.");
        }
    }

    /// <summary>
    /// вывести топ 5 клиентов по количеству заявок на покупку.
    /// </summary>
    [HttpGet("top-purchasers")]
    public async Task<ActionResult<List<ClientOrderCountDto>>> GetTop5Purchasers()
    {
        try
        {
            var result = await analyticsService.GetTop5Purchasers();
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка при выполнении запроса.");
        }
    }

    /// <summary>
    /// вывести топ 5 клиентов по количеству заявок на продажу.
    /// </summary>
    [HttpGet("top-sellers")]
    public async Task<ActionResult<List<ClientOrderCountDto>>> GetTop5Sellers()
    {
        try
        {
            var result = await analyticsService.GetTop5Sellers();
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка при выполнении запроса.");

        }
    }

    /// <summary>
    /// вывести информацию о клиентах, открывших заявки с минимальной стоимостью.
    /// </summary>
    [HttpGet("min-price-orders")]
    public async Task<ActionResult<List<ClientOrderPriceDto>>> GetClientsWithMinOrderPrice()
    {
        try
        {
            var result = await analyticsService.GetClientsWithMinOrderPrice();
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка при выполнении запроса.");
        }
    }
}