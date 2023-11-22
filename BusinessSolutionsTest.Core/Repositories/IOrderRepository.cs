using BusinessSolutionsTest.Core.Models;

namespace BusinessSolutionsTest.Core.Repositories;

public interface IOrderRepository
{
    /// <summary>
    /// Извлекает уникальные номера заказов из коллекции заказов
    /// </summary>
    /// <returns>IEnumerable с уникальными номерами заказов</returns>
    Task<IEnumerable<string>> GetDistinctOrderNumbers();
    
    /// <summary>
    /// Извлекает уникальные имена поставщиков из коллекции заказов
    /// </summary>
    /// <returns>IEnumerable уникальных имен поставщиков</returns>
    Task<IEnumerable<string>> GetDistinctProviderNames();
    
    /// <summary>
    /// Извлекает уникальные имена элементов заказа из коллекции заказов
    /// </summary>
    /// <returns>IEnumerable уникальных имен элементов заказа</returns>
    Task<IEnumerable<string>> GetDistinctOrderItemNames();
    
    /// <summary>
    /// Извлекает уникальные единицы позиции заказа из коллекции заказов
    /// </summary>
    /// <returns>IEnumerable уникальных единиц позиции заказа</returns>
    Task<IEnumerable<string>> GetDistinctOrderItemUnits();
    
    /// <summary>
    /// Находит заказ по его уникальному идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор заказа</param>
    /// <returns>Найденный заказ</returns>
    Task<Order?>? GetOrderById(int id);
    
    /// <summary>
    /// Добавляет или обновляет запись в репозитории для указанного заказа
    /// </summary>
    /// <param name="entity">Заказ, который необходимо сохранить или обновить</param>
    Task SaveOrder(Order? entity);
    
    /// <summary>
    /// Удаляет запись из репозитория на основе уникального идентификатора заказа
    /// </summary>
    /// <param name="id">Уникальный идентификатор заказа, подлежащего удалению</param>
    Task DeleteOrder(int id);
    /// <summary>
    /// Получает отфильтрованную коллекцию заказов на основе указанных критериев
    /// </summary>
    /// <param name="startDate">Дата начала периода, за который должны быть получены заказы</param>
    /// <param name="endDate">Дата окончания периода, за который должны быть получены заказы</param>
    /// <param name="orderNumbers">Коллекция номеров заказов для фильтрации. Если значение равно null, фильтрация по номерам заказов применяться не будет</param>
    /// <param name="orderItemNames">Коллекция названий элементов заказа для фильтрации. Если значение равно null, фильтрация по названиям элементов заказа применяться не будет</param>
    /// <param name="orderItemUnits">Коллекция позиций элемента заказа для фильтрации. Если значение равно null, фильтрация по единицам позиции заказа применяться не будет</param>
    /// <param name="providers">Коллекция имен поставщиков для фильтрации. Если значение равно null, фильтрация по поставщикам применяться не будет</param>
    /// <returns>Отфильтрованная коллекция заказов</returns>
    Task<IEnumerable<Order>> GetOrdersByFilters(DateTime startDate, DateTime endDate, IEnumerable<string>? orderNumbers,
        IEnumerable<string>? orderItemNames, IEnumerable<string>? orderItemUnits, IEnumerable<string>? providers);

}