using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Api.Services;
using RealEstateAgency.Domain;

namespace RealEstateAgency.Api.Controllers
{
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
        /// <term>Residential</term>
        /// </item>
        /// <item>
        /// <term>Uninhabitable</term>
        /// </item>
        /// </list>
        /// </param>
        /// <returns>Список клиентов в виде <see cref="ClientDto"/>.</returns>
        [HttpGet("clients-by-realestate-type")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
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
        /// Вывести всех продавцов, оставивших заявки за заданный период.
        /// </summary>
        /// <param name="startDate">Начальная дата периода.</param>
        /// <param name="endDate">Конечная дата периода.</param>
        /// <returns>Список продавцов в виде <see cref="ClientDto"/>.</returns>
        [HttpGet("sellers-by-period")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
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
        /// Вывести сведения о продавцах и объектах недвижимости, соответствующих определенной заявке покупателя.
        /// </summary>
        /// <param name="buyerOrderId">Идентификатор заявки покупателя.</param>
        /// <returns>Данные продавца и соответствующего объекта недвижимости в виде <see cref="SellerRealEstateDto"/>.</returns>
        [HttpGet("matching-sellers-for-buyer/{buyerOrderId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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
        /// Вывести информацию о количестве заявок по каждому типу недвижимости.
        /// </summary>
        /// <returns>Список количества заявок по типам недвижимости в виде <see cref="RealEstateOrderCountDto"/>.</returns>
        [HttpGet("order-count-by-type")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
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
        /// Вывести топ 5 клиентов по количеству заявок на покупку.
        /// </summary>
        /// <returns>Список топ 5 клиентов в виде <see cref="ClientOrderCountDto"/>.</returns>
        [HttpGet("top-purchasers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
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
        /// Вывести топ 5 клиентов по количеству заявок на продажу.
        /// </summary>
        /// <returns>Список топ 5 клиентов в виде <see cref="ClientOrderCountDto"/>.</returns>
        [HttpGet("top-sellers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
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
        /// Вывести информацию о клиентах, открывших заявки с минимальной стоимостью.
        /// </summary>
        /// <returns>Список клиентов с минимальной стоимостью заявок в виде <see cref="ClientOrderPriceDto"/>.</returns>
        [HttpGet("min-price-orders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
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
}
