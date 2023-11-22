using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BusinessSolutionsTest.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessSolutionsTest.UI.ViewModels;

public class OrderViewModel
{
    [DisplayName("Период")]
    [Required(ErrorMessage = "Введите период начала и конца")]
    public string DateRange { get; set; }
    
    public IList<string>? SelectedProviders { get; set; }
    public IList<string>? SelectedOrderNumbers { get; set; }
    public IList<string>? SelectedOrderItemNames { get; set; }
    public IList<string>? SelectedOrderItemUnits { get; set; }
    
    [DisplayName("Провайдеры")]
    public SelectList? Providers { get; set; }
    [DisplayName("Номера заказов")]
    public SelectList? OrderNumbers { get; set; }
    [DisplayName("Поставщики")]
    public SelectList? OrderItemUnits { get; set; }
    [DisplayName("Названия элементов заказа")]
    public SelectList? OrderItemNames { get; set; }
    public IEnumerable<Order>? Orders { get; set; }
}