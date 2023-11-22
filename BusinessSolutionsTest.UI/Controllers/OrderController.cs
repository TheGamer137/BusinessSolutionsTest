using AutoMapper;
using BusinessSolutionsTest.Core.Models;
using BusinessSolutionsTest.Core.Repositories;
using BusinessSolutionsTest.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessSolutionsTest.UI.Controllers;

public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderController(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    private async Task<OrderViewModel> LoadOrderViewModel(OrderViewModel model)
    {
        model.Providers = new SelectList(await _orderRepository.GetDistinctProviderNames());
        model.OrderNumbers = new SelectList(await _orderRepository.GetDistinctOrderNumbers());
        model.OrderItemUnits = new SelectList(await _orderRepository.GetDistinctOrderItemUnits());
        model.OrderItemNames = new SelectList(await _orderRepository.GetDistinctOrderItemNames());
        return model;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        DateTime today = DateTime.Now;
        DateTime tenDaysAgo = today.AddDays(-10);
        var orders = await _orderRepository.GetOrdersByFilters(tenDaysAgo, today, null,
            null, null, null);
        OrderViewModel model = new OrderViewModel
        {
            Orders = new List<Order>()
        };
        model.Orders = orders;
        return View(await LoadOrderViewModel(model));
    }

    [HttpPost]
    public async Task<IActionResult> Index(OrderViewModel model)
    {
        if (ModelState.IsValid)
        {
            var dateParts = model.DateRange.Split(" — ");
            var startDate = DateTime.Parse(dateParts[0]);
            var endDate = DateTime.Parse(dateParts[1]);
            model.Orders = await _orderRepository.GetOrdersByFilters(startDate, endDate, model.SelectedOrderNumbers,
                model.SelectedOrderItemNames, model.SelectedOrderItemUnits, model.SelectedProviders);
            return PartialView("_ordersListPartial", await LoadOrderViewModel(model));
        }
        return View(await LoadOrderViewModel(model));
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderRepository.GetOrderById(id);
        SaveOrderViewModel model = _mapper.Map<SaveOrderViewModel>(order);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Save(int? id)
    {
        SaveOrderViewModel model;

        if (id.HasValue)
        {
            var order = await _orderRepository.GetOrderById(id.Value);
            model = _mapper.Map<SaveOrderViewModel>(order);
        }
        else
        {
            model = new SaveOrderViewModel
            {
                OrderItems = new List<OrderItem> { new() }
            };
        }
        model.Providers = new SelectList(await _orderRepository.GetDistinctProviderNames());

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(SaveOrderViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Order order;
                if (model.Id > 0)
                {
                    order = await _orderRepository.GetOrderById(model.Id);
                    _mapper.Map(model, order);
                }
                else
                    order = _mapper.Map<Order>(model);
                await _orderRepository.SaveOrder(order);
                return RedirectToAction("Index", "Order");
            }
        }
        catch (DbUpdateException ex)
        {
            TempData["ErrorMessage"] = "Связка номера заказа и поставщика уже есть в базе <br />"+ex.Message;
        }
        model.Providers = new SelectList(await  _orderRepository.GetDistinctProviderNames());
        model.OrderItems = new List<OrderItem> { new() };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _orderRepository.DeleteOrder(id);
        return RedirectToAction("Index", "Order");
    }
}