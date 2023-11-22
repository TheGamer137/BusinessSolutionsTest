using BusinessSolutionsTest.Core.Repositories;
using FakeItEasy;
using AutoMapper;
using BusinessSolutionsTest.Core.Models;
using BusinessSolutionsTest.UI.Controllers;
using BusinessSolutionsTest.UI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace BusinessSolutionsTest.Tests.Controllers;

public class OrderControllerTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderControllerTests()
    {
        _orderRepository = A.Fake<IOrderRepository>();
        _mapper = A.Fake<IMapper>();
    }

    #region Index

    [Fact]
    public async Task Index_ReturnsViewWithModel()
    {
        //Arrange
        A.CallTo(() => _orderRepository.GetOrdersByFilters(A<DateTime>._, A<DateTime>._, A<IEnumerable<string>>._,
            A<IEnumerable<string>>._, A<IEnumerable<string>>._, A<IEnumerable<string>>._)).Returns(new List<Order>());
        var controller = new OrderController(_orderRepository, _mapper);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<OrderViewModel>(viewResult.Model);
        Assert.NotNull(model.Orders);
        Assert.Equal(0, model.Orders.Count());
    }

    [Fact]
    public async Task IndexPost_ReturnsPartialViewWithModel()
    {
        //Arrange
        var model = new OrderViewModel();
        model.DateRange = "2023-01-01 — 2023-01-10";
        A.CallTo(() => _orderRepository.GetOrdersByFilters(A<DateTime>._, A<DateTime>._, A<IEnumerable<string>>._,
            A<IEnumerable<string>>._, A<IEnumerable<string>>._, A<IEnumerable<string>>._)).Returns(new List<Order>());
        var controller = new OrderController(_orderRepository, _mapper);
        //Act
        var result = await controller.Index(model);
        //Assert
        var partialViewResult = Assert.IsType<PartialViewResult>(result);
        var partialModel = Assert.IsAssignableFrom<OrderViewModel>(partialViewResult.Model);
        Assert.NotNull(partialModel.Orders);
    }

    #endregion

    #region Save

    [Fact]
    public async Task SavePost_RedirectsToIndexOnSuccess()
    {
        //Arrange
        var controller = new OrderController(_orderRepository, _mapper);
        var model = new SaveOrderViewModel();

        A.CallTo(() => _mapper.Map<Order>(A<SaveOrderViewModel>._)).Returns(new Order());
        A.CallTo(() => _orderRepository.SaveOrder(A<Order>._)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.Save(model);
        //Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal("Order", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task SavePost_ReturnsViewException()
    {
        //Arrange
        var controller = new OrderController(_orderRepository, _mapper);
        var model = new SaveOrderViewModel();
        var errorMessage = "Test Error Message";

        A.CallTo(() => _mapper.Map<Order>(A<SaveOrderViewModel>._)).Returns(new Order());
        A.CallTo(() => _orderRepository.SaveOrder(A<Order>._)).Throws(new DbUpdateException(errorMessage));
        controller.TempData = new TempDataDictionary(new DefaultHttpContext(), A.Fake<ITempDataProvider>());

        // Act
        var result = await controller.Save(model);

        // Assert
        Assert.Equal("Связка номера заказа и поставщика уже есть в базе <br />"+errorMessage, controller.TempData["ErrorMessage"]);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task DeleteOrder_RedirectsToIndexOnSuccess()
    {
        //Arrange
        var controller = new OrderController(_orderRepository, _mapper);
        var orderId = 1;

        A.CallTo(() => _orderRepository.DeleteOrder(orderId)).Returns(Task.CompletedTask);

        // Act
        var result = await controller.DeleteOrder(orderId);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal("Order", redirectToActionResult.ControllerName);
    }

    #endregion
}