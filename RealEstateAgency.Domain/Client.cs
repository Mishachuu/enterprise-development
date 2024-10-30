using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.Domain;

/// <summary>
/// Клиент агентства недвижимости
/// </summary>
public class Client
{
    /// <summary>
    /// идентификатор клиента
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// фио клиента
    /// </summary>
    public required string FirstAndLastName { get; set; }

    /// <summary>
    /// паспорт
    /// </summary>
    public required string Pasport { get; set; }

    /// <summary>
    /// номер телефона
    /// </summary>
    public string? NumberPhone { get; set; }

    /// <summary>
    /// адрес прописки
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// почта
    /// </summary>
    public string? Email { get; set; }

    public required List<Order> Orders { get; set; }
}
