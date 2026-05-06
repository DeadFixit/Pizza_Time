namespace PizzaTime.Models
{
    /// <summary>
    /// Сущность "Пользователь" — клиент пиццерии.
    /// Содержит минимально необходимые данные: имя и телефон для идентификации при выдаче заказа.
    /// </summary>
    public class User
    {
        /// <summary>Уникальный идентификатор пользователя, генерируется автоматически</summary>
        public int Id { get; }

        /// <summary>Имя клиента — отображается на табло когда заказ готов</summary>
        public string Name { get; }

        /// <summary>Номер телефона клиента для связи и уведомлений</summary>
        public string Phone { get; }

        // Статический счётчик для автоматической генерации уникальных Id
        private static int _idCounter = 1;

        /// <summary>
        /// Создаёт нового пользователя с заданным именем и телефоном.
        /// Id присваивается автоматически.
        /// </summary>
        public User(string name, string phone)
        {
            Id = _idCounter++;
            Name = name;
            Phone = phone;
        }

        /// <summary>
        /// Текстовое представление пользователя.
        /// Пример: "Илья Смирнов (+7-900-111-22-33)"
        /// </summary>
        public override string ToString() => $"{Name} ({Phone})";
    }
}
