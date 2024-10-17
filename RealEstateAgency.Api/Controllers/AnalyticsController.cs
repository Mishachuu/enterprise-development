using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Api.Services;

namespace RealEstateAgency.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnalyticsController(AnalyticsService analyticsService) : ControllerBase
{
    private readonly AnalyticsService _analyticsService = analyticsService;

    /// <summary>
    /// вывести сведения о всех клиентах, ищущих недвижимость заданного типа, упорядочить по ФИО.
    /// </summary>
    [HttpGet("clients-by-realestate-type")]
    public async Task<ActionResult<List<ClientDto>>> GetClientsByRealEstateType(string type)
    {
        var result = await _analyticsService.GetClientsByRealEstateType(type);
        return Ok(result);
    }

    /// <summary>
    /// вывести всех продавцов, оставивших заявки за заданный период.
    /// </summary>
    [HttpGet("sellers-by-period")]
    public async Task<ActionResult<List<ClientDto>>> GetSellersByPeriod(DateTime startDate, DateTime endDate)
    {
        var result = await _analyticsService.GetSellersByPeriod(startDate, endDate);
        return Ok(result);
    }

    /// <summary>
    /// вывести сведения о продавцах и объектах недвижимости, соответствующих определенной заявке покупателя.
    /// </summary>
    [HttpGet("matching-sellers-for-buyer/{buyerOrderId}")]
    public async Task<ActionResult<SellerRealEstateDto>> GetSellersForBuyerOrder(int buyerOrderId)
    {
        var result = await _analyticsService.GetSellersForBuyerOrder(buyerOrderId);
        return Ok(result);
    }

    /// <summary>
    /// вывести информацию о количестве заявок по каждому типу недвижимости.
    /// </summary>
    [HttpGet("order-count-by-type")]
    public async Task<ActionResult<List<RealEstateOrderCountDto>>> GetOrderCountByRealEstateType()
    {
        var result = await _analyticsService.GetOrderCountByRealEstateType();
        return Ok(result);
    }

    /// <summary>
    /// вывести топ 5 клиентов по количеству заявок на покупку.
    /// </summary>
    [HttpGet("top-purchasers")]
    public async Task<ActionResult<List<ClientOrderCountDto>>> GetTop5Purchasers()
    {
        var result = await _analyticsService.GetTop5Purchasers();
        return Ok(result);
    }

    /// <summary>
    /// вывести топ 5 клиентов по количеству заявок на продажу.
    /// </summary>
    [HttpGet("top-sellers")]
    public async Task<ActionResult<List<ClientOrderCountDto>>> GetTop5Sellers()
    {
        var result = await _analyticsService.GetTop5Sellers();
        return Ok(result);
    }

    /// <summary>
    /// вывести информацию о клиентах, открывших заявки с минимальной стоимостью.
    /// </summary>
    [HttpGet("min-price-orders")]
    public async Task<ActionResult<List<ClientOrderPriceDto>>> GetClientsWithMinOrderPrice()
    {
        var result = await _analyticsService.GetClientsWithMinOrderPrice();
        return Ok(result);
    }
}