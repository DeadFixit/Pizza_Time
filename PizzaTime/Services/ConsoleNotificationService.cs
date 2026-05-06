using PizzaTime.Interfaces;

namespace PizzaTime.Services
{
    /// <summary>
    /// Реализация сервиса уведомлений через консоль.
    /// Выводит сообщение цветным текстом (голубым), чтобы визуально отличаться
    /// от обычного вывода программы.
    ///
    /// В реальном приложении этот класс можно заменить на SmsNotificationService
    /// или PushNotificationService — благодаря интерфейсу INotificationService
    /// остальной код менять не придётся.
    /// </summary>
    public class ConsoleNotificationService : INotificationService
    {
        /// <summary>
        /// Выводит уведомление в консоль с пометкой [УВЕДОМЛЕНИЕ] голубым цветом.
        /// После вывода цвет консоли сбрасывается в стандартный.
        /// </summary>
        public void Notify(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan; // голубой для выделения
            Console.WriteLine($"[УВЕДОМЛЕНИЕ] {message}");
            Console.ResetColor(); // возвращаем стандартный цвет консоли
        }
    }
}
