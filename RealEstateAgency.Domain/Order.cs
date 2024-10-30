using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Domain;

/// <summary>
/// заявка клиента
/// </summary>
public class Order
{
    /// <summary>
    /// идентификатор заявки
    /// </summary>
    /// 
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// время заявки
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// тип заявки (покупка или продажа)
    /// </summary>
    public PurchaseOrSale Type { get; set; }

    /// <summary>
    /// цена недвижимости
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// объект недвижимости
    /// </summary>

    [ForeignKey("RealEstateId")]
    public required RealEstate RealEstate { get; set; }
    public int RealEstateId { get; set; }


    /// <summary>
    /// тип заявки
    /// </summary>
    public enum PurchaseOrSale
    {
        Purchase,
        Sale
    }

    public required Client Client { get; set; }
    public int ClientId { get; set; }

}
