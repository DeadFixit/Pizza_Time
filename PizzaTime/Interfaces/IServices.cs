using PizzaTime.Models;

namespace PizzaTime.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса управления заказами.
    /// Описывает полный жизненный цикл заказа: от создания до выдачи клиенту.
    /// Реализуется классом OrderService.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>Принять новый заказ от клиента</summary>
        Order PlaceOrder(User customer, List<Pizza> pizzas);

        /// <summary>Передать заказ на кухню для приготовления</summary>
        void CookOrder(Order order);

        /// <summary>Уведомить клиента о готовности заказа и отобразить на табло</summary>
        void NotifyReady(Order order);

        /// <summary>Зафиксировать выдачу заказа клиенту и убрать с табло</summary>
        void PickUpOrder(Order order);

        /// <summary>Получить список всех заказов (для истории и отчётности)</summary>
        List<Order> GetAllOrders();
    }

    /// <summary>
    /// Интерфейс сервиса меню.
    /// Предоставляет доступ к списку доступных пицц.
    /// Реализуется классом MenuService.
    /// </summary>
    public interface IMenuService
    {
        /// <summary>Получить полный список пицц из меню</summary>
        List<Pizza> GetMenu();

        /// <summary>Найти пиццу по её идентификатору (возвращает null если не найдена)</summary>
        Pizza? GetById(int id);
    }

    /// <summary>
    /// Интерфейс сервиса уведомлений.
    /// Позволяет легко заменить способ уведомления клиента:
    /// консоль, SMS, push-уведомление и т.д. — без изменения бизнес-логики.
    /// Реализуется классом ConsoleNotificationService.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>Отправить уведомление с заданным текстом сообщения</summary>
        void Notify(string message);
    }
}
