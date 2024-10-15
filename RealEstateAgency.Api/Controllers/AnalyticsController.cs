using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Api.Services;
using RealEstateAgency.Domain;

namespace RealEstateAgency.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController(ClientService clientService, OrderService orderService, RealEstateService realEstateService) : ControllerBase
    {
        private readonly ClientService _clientService = clientService;
        private readonly OrderService _orderService = orderService;

        /// <summary>
        /// вывести сведения о всех клиентах, ищущих недвижимость заданного типа, упорядочить по ФИО.
        /// </summary>
        [HttpGet("clients-by-realestate-type")]
        public async Task<ActionResult<List<ClientDto>>> GetClientsByRealEstateType(string type)
        {
            if (!Enum.TryParse<RealEstate.PropertyType>(type, true, out var propertyType))
                return BadRequest("Неверный тип свойства");

            var orders = await _orderService.GetOrdersByPredicate(o => o.Type == Order.PurchaseOrSale.Purchase && o.Item.Type == propertyType);

            if (orders.Count == 0)
                return Ok(new List<ClientDto>());

            var clientIds = orders.Select(o => o.ClientId).Distinct().ToList();
            var clients = await _clientService.GetClientsByPredicate(c => clientIds.Contains(c.ClientId));

            var sortedClients = clients.OrderBy(c => c.FirstAndLastName).ToList();

            return Ok(sortedClients);
        }

        /// <summary>
        /// вывести всех продавцов, оставивших заявки за заданный период.
        /// </summary>
        [HttpGet("sellers-by-period")]
        public async Task<ActionResult<List<ClientDto>>> GetSellersByPeriod(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByPredicate(o => o.Type == Order.PurchaseOrSale.Sale && o.Time >= startDate && o.Time <= endDate);

            if (orders.Count == 0)
                return Ok(new List<ClientDto>());

            var clientIds = orders.Select(o => o.ClientId).Distinct().ToList();
            var sellers = await _clientService.GetClientsByPredicate(c => clientIds.Contains(c.ClientId));

            return Ok(sellers);
        }

        /// <summary>
        /// вывести сведения о продавцах и объектах недвижимости, соответствующих определенной заявке покупателя.
        /// </summary>
        [HttpGet("matching-sellers-for-buyer/{buyerOrderId}")]
        public async Task<ActionResult<List<ClientDto>>> GetSellersForBuyerOrder(int buyerOrderId)
        {
            var buyerOrders = await _orderService.GetOrdersByPredicate(o => o.Id == buyerOrderId && o.Type == Order.PurchaseOrSale.Purchase);
            var buyerOrder = buyerOrders.FirstOrDefault();

            if (buyerOrder == null)
                return NotFound("Заказ покупателя не найден.");

            var sellers = await _clientService.GetClientsByPredicate(c =>
                _orderService.GetOrdersByPredicate(o =>
                    o.Type == Order.PurchaseOrSale.Sale &&
                    o.Id == buyerOrder.RealEstateId &&
                    o.Price == buyerOrder.Price).Result.Any(o => o.ClientId == c.ClientId)
            );

            return Ok(sellers);
        }

        /// <summary>
        /// вывести информацию о количестве заявок по каждому типу недвижимости.
        /// </summary>
        [HttpGet("order-count-by-type")]
        public async Task<ActionResult<Dictionary<string, int>>> GetOrderCountByRealEstateType()
        {
            var orders = await _orderService.GetAllOrders();
            if (orders.Count == 0)
                return Ok(new Dictionary<string, int>());

            var orderCountByType = orders
                .GroupBy(o => o.RealEstateId.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            return Ok(orderCountByType);
        }

        /// <summary>
        /// вывести топ 5 клиентов по количеству заявок (отдельно на покупку и продажу).
        /// </summary>
        [HttpGet("top-clients")]
        public async Task<ActionResult<Dictionary<string, List<ClientDto>>>> GetTop5ClientsByOrderCount()
        {
            var orders = await _orderService.GetAllOrders();

            var topPurchasersIds = orders
                .Where(o => o.Type == Order.PurchaseOrSale.Purchase)
                .GroupBy(o => o.ClientId)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToList();
            var topPurchasers = await _clientService.GetClientsByPredicate(c => topPurchasersIds.Contains(c.ClientId));

            var topSellersIds = orders
                .Where(o => o.Type == Order.PurchaseOrSale.Sale)
                .GroupBy(o => o.ClientId)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToList();
            var topSellers = await _clientService.GetClientsByPredicate(c => topSellersIds.Contains(c.ClientId));

            return Ok(new Dictionary<string, List<ClientDto>>
            {
                { "TopPurchasers", topPurchasers },
                { "TopSellers", topSellers }
            });
        }

        /// <summary>
        /// вывести информацию о клиентах, открывших заявки с минимальной стоимостью.
        /// </summary>
        [HttpGet("min-price-orders")]
        public async Task<ActionResult<List<ClientDto>>> GetClientsWithMinOrderPrice()
        {
            var orders = await _orderService.GetAllOrders();
            if (orders.Count == 0)
                return Ok(new List<ClientDto>());

            var minPrice = orders.Min(o => o.Price);

            var clientIds = orders
                .Where(o => o.Price == minPrice)
                .Select(o => o.ClientId)
                .Distinct()
                .ToList();
            var clientsWithMinPrice = await _clientService.GetClientsByPredicate(c => clientIds.Contains(c.ClientId));

            return Ok(clientsWithMinPrice);
        }
    }
}
