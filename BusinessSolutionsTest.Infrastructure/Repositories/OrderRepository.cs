using System.Linq.Expressions;
using BusinessSolutionsTest.Core.Models;
using BusinessSolutionsTest.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BusinessSolutionsTest.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<string>> GetDistinctOrderNumbers() =>
        await _context.Orders.Select(o => o.Number).Distinct().ToListAsync();

    public async Task<IEnumerable<string>> GetDistinctProviderNames() =>
        await _context.Providers.Select(p => p.Name).Distinct().ToListAsync();

    public async Task<IEnumerable<string>> GetDistinctOrderItemNames() =>
        await _context.OrderItems.Select(oi => oi.Name).Distinct().ToListAsync();

    public async Task<IEnumerable<string>> GetDistinctOrderItemUnits() =>
        await _context.OrderItems.Select(oi => oi.Unit).Distinct().ToListAsync();

    public async Task<Order?> GetOrderById(int id) => await _context.Orders.Include(o => o.Provider)
        .Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);

    public async Task SaveOrder(Order? order)
    {
        if (order.Id > 0)
            _context.Update(order);
        else
        {
            order.Provider = GetProviderByName(order.Provider.Name);
            _context.Add(order);
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrder(int id)
    {
        var order = await GetOrderById(id);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByFilters(DateTime startDate, DateTime endDate, IEnumerable<string>? orderNumbers,
        IEnumerable<string>? orderItemNames, IEnumerable<string>? orderItemUnits, IEnumerable<string>? providers)
    {
        Expression<Func<Order, bool>> predicate = o =>
            o.Date >= startDate && o.Date <= endDate &&
            (orderNumbers == null || orderNumbers.Contains(o.Number)) &&
            (providers == null || providers.Contains(o.Provider.Name)) &&
            (orderItemNames == null || o.OrderItems.Any(oi => orderItemNames.Contains(oi.Name))) &&
            (orderItemUnits == null || o.OrderItems.Any(oi => orderItemUnits.Contains(oi.Unit)));


        return await _context.Orders
            .Include(o => o.Provider)
            .Include(o => o.OrderItems)
            .Where(predicate).ToListAsync();
    }

    private Provider GetProviderByName(string name) => _context.Providers.FirstOrDefault(p => p.Name == name);
}