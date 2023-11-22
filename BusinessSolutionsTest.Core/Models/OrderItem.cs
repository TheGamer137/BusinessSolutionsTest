using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessSolutionsTest.Core.Models;

public class OrderItem
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Введите название заказа")]
    [DisplayName("Название элемента заказа")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Введите количество")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
    [DisplayName("Количество элемента заказа")]
    public decimal Quantity { get; set; }
    
    [Required(ErrorMessage = "Введите позицию")]
    [DisplayName("Позиция элемента заказа")]
    public string Unit { get; set; }
    public int OrderId { get; set; }
}