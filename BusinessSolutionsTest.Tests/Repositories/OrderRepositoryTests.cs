using BusinessSolutionsTest.Core.Models;
using BusinessSolutionsTest.Core.Repositories;
using BusinessSolutionsTest.Infrastructure;
using BusinessSolutionsTest.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BusinessSolutionsTest.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public OrderRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        #region DistinctTests

        [Fact]
        public async Task GetDistinctOrderNumbers_Success_Test()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            //Act
            var result = await repository.GetDistinctOrderNumbers();
            //Assert
            result.Should().BeEquivalentTo("13", "14");
            result.Should().HaveCount(2);
        }
        [Fact]
        public async Task GetDistinctProviderNames_Success_Test()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            //Act
            var result = await repository.GetDistinctProviderNames();
            //Assert
            result.Should().BeEquivalentTo("testProvider1", "testProvider2");
            result.Should().HaveCount(2);
        }
        [Fact]
        public async Task GetDistinctOrderItemNames_Success_Test()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            //Act
            var result = await repository.GetDistinctOrderItemNames();
            //Assert
            result.Should().BeEquivalentTo("testItem1", "testItem2", "testItem3");
            result.Should().HaveCount(3);
        }
        [Fact]
        public async Task GetDistinctOrderItemUnits_Success_Test()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            //Act
            var result = await repository.GetDistinctOrderItemUnits();
            //Assert
            result.Should().BeEquivalentTo("testUnit1", "testUnit2", "testUnit3");
            result.Should().HaveCount(3);
        }

        #endregion

        #region FilterTests

        [Fact]
        public async Task GetOrdersByFilters_Returns_ListByDate()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            //Act
            var result = await repository.GetOrdersByFilters(DateTime.Parse("15.10.2023"), DateTime.Parse("02.11.2023"), null, null, null, null);
            //Assert
            result.Should().HaveCount(2);
        }
        [Fact]
        public async Task GetOrdersByFilters_Returns_ListByDateAndOneFilter()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            var numbers = await repository.GetDistinctOrderNumbers();
            //Act
            var result = await repository.GetOrdersByFilters(DateTime.Parse("01.10.2023"), DateTime.Parse("01.11.2023"), numbers, null, null, null);
            //Assert
            result.Should().HaveCount(3);
        }
        [Fact]
        public async Task GetOrdersByFilters_Returns_ListByDateAndTwoFilters()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            var providers = await repository.GetDistinctProviderNames();
            var numbers = await repository.GetDistinctOrderNumbers();
            //Act
            var result = await repository.GetOrdersByFilters(DateTime.Parse("01.10.2023"), DateTime.Parse("16.10.2023"), numbers, null, null, providers);
            //Assert
            result.Should().HaveCount(2);
        }
        [Fact]
        public async Task GetOrdersByFilters_Returns_ListByDateAndThreeFilters()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            var providers = await repository.GetDistinctProviderNames();
            var numbers = await repository.GetDistinctOrderNumbers();
            var units = await repository.GetDistinctOrderItemUnits();
            //Act
            var result = await repository.GetOrdersByFilters(DateTime.Parse("01.10.2023"), DateTime.Parse("16.10.2023"), numbers, null, units, providers);
            //Assert
            result.Should().HaveCount(2);
        }
        [Fact]
        public async Task GetOrdersByFilters_Returns_ListByDateAndFourFilters()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            var providers = await repository.GetDistinctProviderNames();
            var numbers = await repository.GetDistinctOrderNumbers();
            var units = await repository.GetDistinctOrderItemUnits();
            var names = await repository.GetDistinctOrderItemNames();
            //Act
            var result = await repository.GetOrdersByFilters(DateTime.Parse("01.11.2023"), DateTime.Parse("02.11.2023"), numbers, names, units, providers);
            //Assert
            result.Should().HaveCount(1);
        }
        [Fact]
        public async Task GetOrdersByFilters_Returns_EmptyList_NoMatches()
        {
            // Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
    
            // Act
            var result = await repository.GetOrdersByFilters(DateTime.Parse("01.01.2023"), DateTime.Parse("02.01.2023"), null, null, null, null);

            // Assert
            result.Should().BeEmpty();
        }
        #endregion
        [Fact]
        public async Task SaveOrder_CreatesNewOrder()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            Order? order = new Order
            {
                Date = DateTime.Parse("01.06.2023"),
                Number = "asd",
                OrderItems = new List<OrderItem>
                {
                    new()
                    {
                        Name = "qwe",
                        OrderId = 4,
                        Quantity = 111,
                        Unit = "kjlkjl"
                    }
                },
                Provider = new Provider
                {
                    Name = "testProvider2"
                }
            };
            //Act
            await repository.SaveOrder(order);
            var result = await repository.GetOrdersByFilters(DateTime.Parse("01.01.2023"), DateTime.Now,
                null, null, null, null);
            //Assert
            result.Should().HaveCount(4);
        }
        [Fact]
        public async Task SaveOrder_UpdatesOrder()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());
            var existingOrder = await repository.GetOrderById(2);
            existingOrder.Number = "asd";
            existingOrder.OrderItems = new List<OrderItem>
            {
                new()
                {
                    Name = "qwe",
                    Quantity = 111,
                    Unit = "kjlkjl"
                }
            };
            //Act
            await repository.SaveOrder(existingOrder);
            var updatedOrder = await repository.GetOrderById(2);
            //Assert
            updatedOrder.Number.Should().Be("asd");
            updatedOrder.OrderItems.Should().HaveCount(1);
        }
        [Fact]
        public async Task GetOrderById_Success_Test()
        {
            // Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());

            // Act
            var result = await repository.GetOrderById(1);

            // Assert
            result.Number.Should().Be("13");
        }
        [Fact]
        public async Task DeleteOrder_Success_Test()
        {
            // Arrange
            IOrderRepository repository = new OrderRepository(GetDbContext());

            // Act
            await repository.DeleteOrder(1);
            var result = await repository.GetOrdersByFilters(DateTime.Parse("01.01.2023"), DateTime.Parse("01.12.2023"),
                null, null, null, null);
            // Assert
            result.Should().HaveCount(2);
        }
        private AppDbContext GetDbContext()
        {
            var context = new AppDbContext(_options);
            context.AddRange(GetOrdersForTest());
            context.SaveChanges();
            return context;
        }

        private List<Order> GetOrdersForTest()
        {
            return new List<Order>
            {
                new()
                {
                    Id = 1,
                    Date = DateTime.Parse("01.10.2023"),
                    Number = "13",
                    OrderItems = new List<OrderItem>
                    {
                        new()
                        {
                            Id = 1,
                            Name = "testItem1",
                            OrderId = 1,
                            Quantity = 123,
                            Unit = "testUnit1"
                        }
                    },
                    Provider = new Provider
                    {
                        Id = 1,
                        Name = "testProvider1"
                    }
                },
                new()
                {
                    Id = 2,
                    Date = DateTime.Parse("01.11.2023"),
                    Number = "14",
                    OrderItems = new List<OrderItem>
                    {
                        new()
                        {
                            Id = 2,
                            Name = "testItem2",
                            OrderId = 2,
                            Quantity = 456,
                            Unit = "testUnit2"
                        },
                        new()
                        {
                            Id = 3,
                            Name = "testItem3",
                            OrderId = 2,
                            Quantity = 789,
                            Unit = "testUnit3"
                        }
                    },
                    Provider = new Provider
                    {
                        Id = 2,
                        Name = "testProvider2"
                    }
                },
                new()
                {
                    Id = 3,
                    Date = DateTime.Parse("15.10.2023"),
                    Number = "13",
                    OrderItems = new List<OrderItem>
                    {
                        new()
                        {
                            Id = 4,
                            Name = "testItem2",
                            OrderId = 2,
                            Quantity = 1234,
                            Unit = "testUnit1"
                        },
                        new()
                        {
                            Id = 5,
                            Name = "testItem1",
                            OrderId = 2,
                            Quantity = 6789,
                            Unit = "testUnit2"
                        }
                    },
                    Provider = new Provider
                    {
                        Id = 3,
                        Name = "testProvider1"
                    }
                }
            };
        }
    }
}