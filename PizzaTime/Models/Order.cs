namespace PizzaTime.Models
{
    /// <summary>
    /// Статус заказа — отражает текущий этап обработки заказа в пиццерии.
    /// Заказ последовательно проходит все стадии от принятия до выдачи клиенту.
    /// </summary>
    public enum OrderStatus
    {
        Pending,    // Принят — заказ зарегистрирован, ещё не передан на кухню
        Cooking,    // Готовится — кухня приступила к приготовлению
        Ready,      // Готов — пицца готова, имя клиента высвечивается на табло
        PickedUp    // Забран — клиент получил заказ, цикл завершён
    }

    /// <summary>
    /// Сущность "Заказ" — центральная связующая сущность между клиентом и пиццерией.
    /// Хранит информацию о клиенте, составе заказа, текущем статусе и времени создания.
    /// </summary>
    public class Order
    {
        /// <summary>Уникальный номер заказа — отображается на табло и в квитанции</summary>
        public int Id { get; }

        /// <summary>Клиент, сделавший заказ</summary>
        public User Customer { get; }

        /// <summary>Список пицц в заказе (может быть несколько)</summary>
        public List<Pizza> Pizzas { get; }

        /// <summary>Текущий статус заказа — изменяется по мере прохождения этапов</summary>
        public OrderStatus Status { get; private set; }

        /// <summary>Дата и время создания заказа</summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Итоговая сумма заказа — вычисляется автоматически как сумма цен всех пицц.
        /// Используется только для чтения (нет сеттера).
        /// </summary>
        public decimal TotalPrice => Pizzas.Sum(p => p.Price);

        // Статический счётчик для автоматической нумерации заказов
        private static int _idCounter = 1;

        /// <summary>
        /// Создаёт новый заказ для клиента с указанным набором пицц.
        /// Начальный статус — Pending (принят).
        /// </summary>
        public Order(User customer, List<Pizza> pizzas)
        {
            Id = _idCounter++;
            Customer = customer;
            Pizzas = pizzas;
            Status = OrderStatus.Pending; // заказ только что принят
            CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Изменяет статус заказа. Вызывается сервисом OrderService
        /// при переходе между этапами (принят → готовится → готов → забран).
        /// </summary>
        public void SetStatus(OrderStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Человекочитаемое название текущего статуса на русском языке.
        /// Используется при выводе информации о заказе.
        /// </summary>
        public string StatusLabel => Status switch
        {
            OrderStatus.Pending  => "Принят",
            OrderStatus.Cooking  => "Готовится",
            OrderStatus.Ready    => "Готов! Заберите заказ",
            OrderStatus.PickedUp => "Забран",
            _                    => "—"
        };

        /// <summary>
        /// Краткое текстовое представление заказа для вывода в списке.
        /// Пример: "Заказ #1 | Илья Смирнов | Готовится | 1080 руб."
        /// </summary>
        public override string ToString() =>
            $"Заказ #{Id} | {Customer.Name} | {StatusLabel} | {TotalPrice:F0} руб.";
    }
}
