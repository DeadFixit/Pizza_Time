using PizzaTime.Models;
using PizzaTime.Services;

namespace PizzaTime
{
    /// <summary>
    /// Класс "Пиццерия" — реализует паттерн Фасад (Facade).
    /// Скрывает внутреннюю сложность (сервисы, табло, меню) за единым простым интерфейсом.
    /// Внешний код (Program.cs) работает только с этим классом,
    /// не зная ничего о MenuService, OrderService и прочих внутренностях.
    /// </summary>
    public class Pizzeria
    {
        /// <summary>Название пиццерии, отображается в приветствии</summary>
        public string Name { get; }

        /// <summary>Адрес пиццерии, отображается в приветствии</summary>
        public string Address { get; }

        // Внутренние сервисы — скрыты от внешнего кода (private)
        private readonly MenuService _menuService;
        private readonly OrderService _orderService;
        private readonly DisplayBoard _board;

        /// <summary>
        /// Создаёт пиццерию и инициализирует все необходимые сервисы.
        /// Зависимости создаются здесь же — для учебного проекта это приемлемо.
        /// В production-приложении использовался бы DI-контейнер (например, Microsoft.Extensions.DI).
        /// </summary>
        public Pizzeria(string name, string address)
        {
            Name = name;
            Address = address;

            // Создаём табло — оно используется и OrderService, поэтому создаётся первым
            _board = new DisplayBoard();

            // Создаём сервис уведомлений — в данном случае консольный
            var notificationService = new ConsoleNotificationService();

            // Создаём сервисы, передавая им необходимые зависимости
            _menuService = new MenuService();
            _orderService = new OrderService(notificationService, _board);
        }

        /// <summary>
        /// Выводит приветственный баннер с названием и адресом пиццерии.
        /// Используется при запуске приложения.
        /// </summary>
        public void PrintWelcome()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine($"║  🍕  Добро пожаловать в {Name,-11}║");
            Console.WriteLine($"║  📍  {Address,-30}║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.ResetColor();
        }

        /// <summary>Выводит меню пиццерии в консоль (делегирует в MenuService)</summary>
        public void ShowMenu() => _menuService.PrintMenu();

        /// <summary>
        /// Оформляет заказ для клиента по списку Id пицц из меню.
        /// Пиццы с несуществующими Id игнорируются.
        /// Если ни одна пицца не найдена — выбрасывает исключение.
        /// </summary>
        public Order PlaceOrder(User customer, List<int> pizzaIds)
        {
            // Преобразуем список Id в список объектов Pizza через MenuService
            var pizzas = pizzaIds
                .Select(id => _menuService.GetById(id))  // ищем пиццу по Id
                .Where(p => p != null)                    // отбрасываем null (не найденные)
                .Cast<Pizza>()                            // явное приведение типа
                .ToList();

            if (pizzas.Count == 0)
                throw new ArgumentException("Ни одна пицца из списка не найдена в меню.");

            // Делегируем создание заказа в OrderService
            return _orderService.PlaceOrder(customer, pizzas);
        }

        /// <summary>
        /// Запускает полный цикл приготовления заказа:
        /// передаёт на кухню (CookOrder) и уведомляет о готовности (NotifyReady).
        /// Имитирует работу кухни с задержкой.
        /// </summary>
        public void ProcessOrder(Order order)
        {
            _orderService.CookOrder(order);    // кухня готовит
            _orderService.NotifyReady(order);  // заказ готов — имя на табло
        }

        /// <summary>
        /// Фиксирует выдачу заказа клиенту.
        /// Убирает запись с табло и меняет статус на PickedUp.
        /// </summary>
        public void CustomerPickUp(Order order)
        {
            _orderService.PickUpOrder(order);
        }

        /// <summary>Выводит историю всех заказов (делегирует в OrderService)</summary>
        public void ShowAllOrders() => _orderService.PrintAllOrders();

        /// <summary>
        /// Возвращает список всех заказов — используется в Program.cs
        /// для фильтрации заказов по статусу (Pending, Ready и т.д.)
        /// </summary>
        public List<Order> GetAllOrders() => _orderService.GetAllOrders();

        /// <summary>Показывает текущее состояние табло готовых заказов</summary>
        public void ShowBoard() => _board.Show();
    }
}
