namespace PizzaTime.Models
{
    /// <summary>
    /// Табло готовых заказов — имитирует электронное информационное табло в зале пиццерии.
    /// Когда заказ готов, его номер и имя клиента появляются на табло.
    /// Когда клиент забирает заказ — запись удаляется с табло.
    /// </summary>
    public class DisplayBoard
    {
        // Внутренний список заказов, ожидающих получения клиентами
        private readonly List<Order> _readyOrders = new();

        /// <summary>
        /// Список готовых заказов доступен только для чтения извне.
        /// Изменение возможно только через методы AddOrder / RemoveOrder.
        /// </summary>
        public IReadOnlyList<Order> ReadyOrders => _readyOrders.AsReadOnly();

        /// <summary>
        /// Добавляет заказ на табло когда он готов к выдаче.
        /// Дубликаты не добавляются — проверяется наличие заказа в списке.
        /// </summary>
        public void AddOrder(Order order)
        {
            if (!_readyOrders.Contains(order))
                _readyOrders.Add(order);
        }

        /// <summary>
        /// Убирает заказ с табло после того как клиент его забрал.
        /// </summary>
        public void RemoveOrder(Order order)
        {
            _readyOrders.Remove(order);
        }

        /// <summary>
        /// Выводит текущее состояние табло в консоль в виде визуальной рамки.
        /// Показывает номер заказа и имя клиента для каждого готового заказа.
        /// Если готовых заказов нет — выводит соответствующее сообщение.
        /// </summary>
        public void Show()
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║           ТАБЛО ГОТОВЫХ ЗАКАЗОВ      ║");
            Console.WriteLine("╠══════════════════════════════════════╣");

            if (_readyOrders.Count == 0)
            {
                // Нет ни одного готового заказа
                Console.WriteLine("║         Готовых заказов нет          ║");
            }
            else
            {
                // Выводим каждый готовый заказ: номер и имя клиента
                foreach (var order in _readyOrders)
                {
                    string line = $"  ► Заказ #{order.Id} — {order.Customer.Name}";
                    Console.WriteLine($"║ {line,-36} ║");
                }
            }

            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();
        }
    }
}
