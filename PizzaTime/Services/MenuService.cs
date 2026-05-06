using PizzaTime.Interfaces;
using PizzaTime.Models;

namespace PizzaTime.Services
{
    /// <summary>
    /// Сервис меню пиццерии — хранит и предоставляет список доступных пицц.
    /// В реальном приложении данные загружались бы из базы данных,
    /// здесь для простоты меню задаётся прямо в коде (in-memory).
    /// </summary>
    public class MenuService : IMenuService
    {
        // Внутренний список пицц — наполняется в конструкторе
        private readonly List<Pizza> _menu;

        /// <summary>
        /// Инициализирует меню набором стандартных пицц.
        /// Каждая пицца создаётся с названием, составом, размером, ценой
        /// и временем приготовления в секундах (для симуляции кухни).
        /// </summary>
        public MenuService()
        {
            _menu = new List<Pizza>
            {
                // Классика — простая пицца с базиликом, самая быстрая в приготовлении
                new Pizza("Маргарита",
                    new List<string> { "томатный соус", "моцарелла", "базилик" },
                    PizzaSize.Medium, 490, 3),

                // Популярная пицца с острой колбасой
                new Pizza("Пепперони",
                    new List<string> { "томатный соус", "моцарелла", "пепперони" },
                    PizzaSize.Medium, 590, 4),

                // Премиум-пицца с четырьмя видами сыра
                new Pizza("Четыре сыра",
                    new List<string> { "сливочный соус", "моцарелла", "пармезан", "горгонзола", "чеддер" },
                    PizzaSize.Medium, 650, 4),

                // Нестандартное сочетание — курица с ананасом
                new Pizza("Гавайская",
                    new List<string> { "томатный соус", "моцарелла", "курица", "ананас" },
                    PizzaSize.Medium, 560, 3),

                // Большая пицца с соусом BBQ и говядиной — дольше всего готовится
                new Pizza("Барбекю",
                    new List<string> { "соус BBQ", "моцарелла", "говядина", "лук", "перец" },
                    PizzaSize.Large, 720, 5),
            };
        }

        /// <summary>Возвращает полный список пицц из меню</summary>
        public List<Pizza> GetMenu() => _menu;

        /// <summary>
        /// Ищет пиццу по Id. Возвращает null если пицца с таким Id не найдена.
        /// Используется при создании заказа для получения объектов Pizza по выбранным Id.
        /// </summary>
        public Pizza? GetById(int id) => _menu.FirstOrDefault(p => p.Id == id);

        /// <summary>
        /// Выводит меню в консоль в читаемом формате:
        /// Id, название, размер, цена и состав каждой пиццы.
        /// </summary>
        public void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== МЕНЮ ПИЦЦЕРИИ ===");
            foreach (var pizza in _menu)
            {
                Console.WriteLine($"  [{pizza.Id}] {pizza}");
                // Выводим состав через запятую
                Console.WriteLine($"       Состав: {string.Join(", ", pizza.Ingredients)}");
            }
            Console.WriteLine();
        }
    }
}
