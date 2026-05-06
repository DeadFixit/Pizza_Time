namespace PizzaTime.Models
{
    /// <summary>
    /// Размер пиццы — определяет физический размер и влияет на цену
    /// </summary>
    public enum PizzaSize
    {
        Small,   // Маленькая
        Medium,  // Средняя
        Large    // Большая
    }

    /// <summary>
    /// Сущность "Пицца" — описывает конкретную позицию в меню пиццерии.
    /// Хранит всю информацию о блюде: название, состав, размер, цену и время готовки.
    /// </summary>
    public class Pizza
    {
        /// <summary>Уникальный идентификатор пиццы, генерируется автоматически</summary>
        public int Id { get; }

        /// <summary>Название пиццы, например "Маргарита"</summary>
        public string Name { get; }

        /// <summary>Список ингредиентов, из которых состоит пицца</summary>
        public List<string> Ingredients { get; }

        /// <summary>Размер пиццы (маленькая / средняя / большая)</summary>
        public PizzaSize Size { get; }

        /// <summary>Цена пиццы в рублях</summary>
        public decimal Price { get; }

        /// <summary>
        /// Время приготовления в секундах.
        /// Используется сервисом заказов для симуляции работы кухни.
        /// </summary>
        public int CookingTimeSeconds { get; }

        // Статический счётчик для автоматической генерации уникальных Id
        private static int _idCounter = 1;

        /// <summary>
        /// Создаёт новую пиццу с заданными параметрами.
        /// Id присваивается автоматически и инкрементируется при каждом создании.
        /// </summary>
        public Pizza(string name, List<string> ingredients, PizzaSize size, decimal price, int cookingTimeSeconds)
        {
            Id = _idCounter++;          // автоматически присваиваем следующий Id
            Name = name;
            Ingredients = ingredients;
            Size = size;
            Price = price;
            CookingTimeSeconds = cookingTimeSeconds;
        }

        /// <summary>
        /// Текстовое представление пиццы для вывода в консоль и меню.
        /// Пример: "Маргарита (Средняя) — 490 руб."
        /// </summary>
        public override string ToString()
        {
            // Переводим enum-значение размера в читаемую строку на русском
            string sizeLabel = Size switch
            {
                PizzaSize.Small  => "Маленькая",
                PizzaSize.Medium => "Средняя",
                PizzaSize.Large  => "Большая",
                _                => "—"
            };
            return $"{Name} ({sizeLabel}) — {Price:F0} руб.";
        }
    }
}
