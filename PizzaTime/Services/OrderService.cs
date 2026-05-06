using PizzaTime.Interfaces;
using PizzaTime.Models;

namespace PizzaTime.Services
{
    /// <summary>
    /// Сервис управления заказами — главная бизнес-логика пиццерии.
    /// Отвечает за полный цикл обработки заказа:
    /// принятие → приготовление → уведомление → выдача.
    ///
    /// Зависит от INotificationService (для уведомлений) и DisplayBoard (для табло),
    /// которые передаются через конструктор — это позволяет легко подменять реализации.
    /// </summary>
    public class OrderService : IOrderService
    {
        // Хранилище всех заказов в памяти (история)
        private readonly List<Order> _orders = new();

        // Сервис уведомлений — используется для оповещения клиента
        private readonly INotificationService _notificationService;

        // Табло — обновляется когда заказ готов или забран
        private readonly DisplayBoard _board;

        /// <summary>
        /// Конструктор с внедрением зависимостей (Dependency Injection).
        /// Принимает сервис уведомлений и табло — это позволяет тестировать
        /// OrderService независимо от конкретных реализаций этих зависимостей.
        /// </summary>
        public OrderService(INotificationService notificationService, DisplayBoard board)
        {
            _notificationService = notificationService;
            _board = board;
        }

        /// <summary>
        /// Принимает новый заказ от клиента:
        /// 1. Создаёт объект Order со статусом Pending
        /// 2. Сохраняет в список заказов
        /// 3. Выводит подтверждение с составом и суммой
        /// 4. Отправляет уведомление клиенту
        /// </summary>
        public Order PlaceOrder(User customer, List<Pizza> pizzas)
        {
            // Создаём заказ и сразу сохраняем в историю
            var order = new Order(customer, pizzas);
            _orders.Add(order);

            // Выводим подтверждение зелёным цветом
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✔ Заказ #{order.Id} принят от {customer.Name}!");
            Console.ResetColor();

            // Детализируем состав заказа
            Console.WriteLine($"   Состав заказа:");
            foreach (var pizza in pizzas)
                Console.WriteLine($"   • {pizza}");
            Console.WriteLine($"   Итого: {order.TotalPrice:F0} руб.");

            // Уведомляем клиента о приёме заказа
            _notificationService.Notify($"Ваш заказ #{order.Id} принят! Ожидайте.");

            return order;
        }

        /// <summary>
        /// Имитирует процесс приготовления заказа на кухне:
        /// 1. Меняет статус на Cooking
        /// 2. Рассчитывает общее время готовки как сумму времён всех пицц
        /// 3. Симулирует ожидание (Thread.Sleep — ускорено в 2 раза для демо)
        /// </summary>
        public void CookOrder(Order order)
        {
            // Переводим заказ в статус "Готовится"
            order.SetStatus(OrderStatus.Cooking);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n🍕 Заказ #{order.Id} ({order.Customer.Name}) — готовится...");
            Console.ResetColor();

            // Суммируем время приготовления всех пицц в заказе
            int totalSeconds = order.Pizzas.Sum(p => p.CookingTimeSeconds);
            Console.WriteLine($"   Расчётное время: ~{totalSeconds} сек.");

            // Симуляция: каждую итерацию спим 500мс и рисуем точку
            // (в реальном приложении здесь был бы async/await с реальной задержкой)
            for (int i = 0; i < totalSeconds; i++)
            {
                Thread.Sleep(500); // ускорено для демо: 0.5с вместо 1с
                Console.Write(".");
            }
            Console.WriteLine(); // перевод строки после точек
        }

        /// <summary>
        /// Уведомляет клиента о готовности заказа:
        /// 1. Меняет статус на Ready
        /// 2. Добавляет заказ на табло
        /// 3. Отправляет уведомление с именем клиента
        /// 4. Отображает актуальное состояние табло
        /// </summary>
        public void NotifyReady(Order order)
        {
            // Переводим заказ в статус "Готов"
            order.SetStatus(OrderStatus.Ready);

            // Добавляем на табло — клиент увидит своё имя
            _board.AddOrder(order);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n🔔 Заказ #{order.Id} ГОТОВ!");
            Console.ResetColor();

            // Персональное уведомление клиенту
            _notificationService.Notify($"{order.Customer.Name}, ваш заказ #{order.Id} готов! Подойдите на кассу.");

            // Показываем обновлённое табло
            _board.Show();
        }

        /// <summary>
        /// Обрабатывает выдачу заказа клиенту:
        /// 1. Проверяет что заказ действительно готов
        /// 2. Меняет статус на PickedUp
        /// 3. Убирает заказ с табло
        /// 4. Показывает обновлённое табло
        /// </summary>
        public void PickUpOrder(Order order)
        {
            // Защита: нельзя забрать заказ который ещё не готов
            if (order.Status != OrderStatus.Ready)
            {
                Console.WriteLine($"⚠ Заказ #{order.Id} ещё не готов.");
                return;
            }

            // Переводим в финальный статус
            order.SetStatus(OrderStatus.PickedUp);

            // Убираем с табло — место освобождается для следующего заказа
            _board.RemoveOrder(order);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✔ {order.Customer.Name} забрал заказ #{order.Id}. Приятного аппетита!");
            Console.ResetColor();

            // Показываем табло — имя клиента исчезает
            _board.Show();
        }

        /// <summary>Возвращает список всех заказов для истории и отчётности</summary>
        public List<Order> GetAllOrders() => _orders;

        /// <summary>
        /// Выводит в консоль список всех заказов с их текущим статусом.
        /// Используется для общего обзора работы пиццерии.
        /// </summary>
        public void PrintAllOrders()
        {
            Console.WriteLine("\n=== ВСЕ ЗАКАЗЫ ===");
            if (_orders.Count == 0)
            {
                Console.WriteLine("  Заказов нет.");
                return;
            }
            // Каждый заказ выводится через свой ToString()
            foreach (var order in _orders)
                Console.WriteLine($"  {order}");
            Console.WriteLine();
        }
    }
}
