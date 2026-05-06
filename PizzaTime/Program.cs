using PizzaTime;
using PizzaTime.Models;

// Устанавливаем кодировку UTF-8 чтобы корректно отображались
// кириллица и символы рамок (╔, ║, ╚ и т.д.) в консоли Windows
Console.OutputEncoding = System.Text.Encoding.UTF8;

// ─── Инициализация ──────────────────────────────────────────────
// Создаём единственный экземпляр пиццерии — фасад над всей логикой
var pizzeria = new Pizzeria("Pizza Time", "ул. Итальянская, 5");
pizzeria.PrintWelcome();

// ─── Главный цикл приложения ────────────────────────────────────
// Программа работает до тех пор пока пользователь не выберет "Выход"
bool running = true;
while (running)
{
    // Выводим главное меню действий
    PrintMainMenu();

    // Читаем ввод пользователя и убираем лишние пробелы
    string? input = Console.ReadLine()?.Trim();

    switch (input)
    {
        case "1":
            // Показать меню пиццерии — список всех доступных пицц
            pizzeria.ShowMenu();
            break;

        case "2":
            // Сценарий: новый клиент хочет сделать заказ
            MakeOrder(pizzeria);
            break;

        case "3":
            // Сценарий: кухня приготовила заказ — передаём его в статус "Готов"
            CookNextOrder(pizzeria);
            break;

        case "4":
            // Показать электронное табло с готовыми к выдаче заказами
            pizzeria.ShowBoard();
            break;

        case "5":
            // Сценарий: клиент подошёл и забирает свой готовый заказ
            PickUpOrder(pizzeria);
            break;

        case "6":
            // Показать историю всех заказов за сессию
            pizzeria.ShowAllOrders();
            break;

        case "0":
            // Выход из программы
            Console.WriteLine("\nСпасибо за посещение Pizza Time! 🍕");
            running = false;
            break;

        default:
            // Пользователь ввёл что-то непонятное
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⚠ Неверный ввод. Выберите пункт от 0 до 6.");
            Console.ResetColor();
            break;
    }

    // Небольшая пауза перед следующей итерацией — чтобы пользователь успел
    // прочитать результат действия перед повторным выводом меню
    if (running)
    {
        Console.WriteLine("\nНажмите Enter для продолжения...");
        Console.ReadLine();
    }
}

// ════════════════════════════════════════════════════════════════
// ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ (локальные функции)
// ════════════════════════════════════════════════════════════════

/// <summary>
/// Выводит главное меню с перечнем доступных действий.
/// Вызывается в начале каждой итерации главного цикла.
/// </summary>
void PrintMainMenu()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("╔══════════════════════════════╗");
    Console.WriteLine("║        ГЛАВНОЕ МЕНЮ          ║");
    Console.WriteLine("╠══════════════════════════════╣");
    Console.WriteLine("║  1. Показать меню пиццерии   ║");
    Console.WriteLine("║  2. Сделать заказ            ║");
    Console.WriteLine("║  3. Приготовить заказ        ║");
    Console.WriteLine("║  4. Показать табло           ║");
    Console.WriteLine("║  5. Забрать заказ            ║");
    Console.WriteLine("║  6. Все заказы (история)     ║");
    Console.WriteLine("║  0. Выход                    ║");
    Console.WriteLine("╚══════════════════════════════╝");
    Console.ResetColor();
    Console.Write("Ваш выбор: ");
}

/// <summary>
/// Интерактивный сценарий оформления нового заказа:
/// 1. Вводим имя и телефон клиента
/// 2. Показываем меню
/// 3. Клиент выбирает пиццы по Id (можно несколько через запятую)
/// 4. Создаём заказ и выводим подтверждение
/// </summary>
void MakeOrder(Pizzeria p)
{
    Console.WriteLine("\n=== НОВЫЙ ЗАКАЗ ===");

    // Запрашиваем имя клиента — обязательное поле
    Console.Write("Имя клиента: ");
    string name = Console.ReadLine()?.Trim() ?? "Гость";
    if (string.IsNullOrWhiteSpace(name)) name = "Гость";

    // Запрашиваем телефон клиента
    Console.Write("Телефон: ");
    string phone = Console.ReadLine()?.Trim() ?? "—";
    if (string.IsNullOrWhiteSpace(phone)) phone = "—";

    // Создаём объект пользователя с введёнными данными
    var user = new User(name, phone);

    // Показываем меню чтобы клиент видел что можно заказать
    p.ShowMenu();

    // Просим ввести Id пицц через запятую, например: 1,2,3
    Console.Write("Введите Id пицц через запятую (например: 1,2): ");
    string? idsInput = Console.ReadLine();

    // Разбираем строку ввода: разделяем по запятой, парсим в int, убираем ошибочные
    var pizzaIds = (idsInput ?? "")
        .Split(',')                                          // разбиваем по запятой
        .Select(s => s.Trim())                               // убираем пробелы вокруг каждого Id
        .Where(s => int.TryParse(s, out _))                  // оставляем только числа
        .Select(int.Parse)                                   // конвертируем строки в int
        .ToList();

    if (pizzaIds.Count == 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("⚠ Не выбрано ни одной пиццы. Заказ отменён.");
        Console.ResetColor();
        return;
    }

    // Пробуем создать заказ — метод выбросит исключение если Id не найдены в меню
    try
    {
        p.PlaceOrder(user, pizzaIds);
    }
    catch (ArgumentException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"⚠ Ошибка: {ex.Message}");
        Console.ResetColor();
    }
}

/// <summary>
/// Интерактивный сценарий приготовления заказа (роль — сотрудник кухни):
/// 1. Показываем список заказов в статусе Pending
/// 2. Кухня выбирает номер заказа для приготовления
/// 3. Запускаем симуляцию готовки → заказ появляется на табло
/// </summary>
void CookNextOrder(Pizzeria p)
{
    Console.WriteLine("\n=== ПРИГОТОВИТЬ ЗАКАЗ ===");

    // Получаем заказы ожидающие приготовления (статус Pending)
    var pending = p.GetAllOrders()
        .Where(o => o.Status == OrderStatus.Pending)
        .ToList();

    if (pending.Count == 0)
    {
        Console.WriteLine("Нет заказов ожидающих приготовления.");
        return;
    }

    // Показываем список доступных для приготовления заказов
    Console.WriteLine("Заказы в очереди:");
    foreach (var o in pending)
        Console.WriteLine($"  {o}");

    // Кухня вводит номер заказа который берёт в работу
    Console.Write("Введите номер заказа для приготовления: ");
    if (!int.TryParse(Console.ReadLine()?.Trim(), out int orderId))
    {
        Console.WriteLine("⚠ Неверный номер.");
        return;
    }

    // Ищем заказ с введённым Id среди заказов в очереди
    var order = pending.FirstOrDefault(o => o.Id == orderId);
    if (order == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"⚠ Заказ #{orderId} не найден или уже готовится.");
        Console.ResetColor();
        return;
    }

    // Запускаем полный цикл готовки: Cooking → Ready → табло обновляется
    p.ProcessOrder(order);
}

/// <summary>
/// Интерактивный сценарий выдачи заказа клиенту:
/// 1. Показываем табло с готовыми заказами
/// 2. Клиент называет номер своего заказа
/// 3. Фиксируем выдачу — имя убирается с табло
/// </summary>
void PickUpOrder(Pizzeria p)
{
    Console.WriteLine("\n=== ВЫДАЧА ЗАКАЗА ===");

    // Показываем табло — клиент видит своё имя
    p.ShowBoard();

    // Получаем список готовых к выдаче заказов
    var ready = p.GetAllOrders()
        .Where(o => o.Status == OrderStatus.Ready)
        .ToList();

    if (ready.Count == 0)
    {
        Console.WriteLine("Нет заказов готовых к выдаче.");
        return;
    }

    // Клиент называет номер своего заказа
    Console.Write("Введите номер вашего заказа: ");
    if (!int.TryParse(Console.ReadLine()?.Trim(), out int orderId))
    {
        Console.WriteLine("⚠ Неверный номер.");
        return;
    }

    // Ищем заказ среди готовых
    var order = ready.FirstOrDefault(o => o.Id == orderId);
    if (order == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"⚠ Заказ #{orderId} не найден среди готовых.");
        Console.ResetColor();
        return;
    }

    // Выдаём заказ клиенту — статус становится PickedUp, имя уходит с табло
    p.CustomerPickUp(order);
}
