using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BusinessSolutionsTest.Core.CustomValidators;
using BusinessSolutionsTest.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessSolutionsTest.UI.ViewModels;

public class SaveOrderViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Введите номер заказа")]
    [OrderItemListValidation("OrderItems")]
    [DisplayName("Номер заказа")]
    public string OrderNumber { get; set; }
    
    [Required(ErrorMessage = "Введите дату")]
    [DataType(DataType.DateTime)]
    [DisplayName("Дата заказа")]
    public DateTime OrderDate { get; set; }
    
    [Required(ErrorMessage = "Нужно добавить хотя бы один элемент заказа")]
    public List<OrderItem> OrderItems { get; set; }
    
    [DisplayName("Поставщик")]
    public string SelectedProvider { get; set; } 
    public SelectList? Providers { get; set; }
}