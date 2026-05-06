# Pizza Time 🍕

OOP экзаменационный проект — симуляция работы пиццерии на C#.

## Стек
- C# / .NET 8
- ООП: классы, интерфейсы, инкапсуляция
- Паттерны: Facade, Dependency Injection

## Архитектура
```
PizzaTime/
├── Models/
│   ├── Pizza.cs           — сущность пиццы
│   ├── User.cs            — сущность пользователя
│   ├── Order.cs           — заказ + enum OrderStatus
│   └── DisplayBoard.cs    — табло готовых заказов
├── Interfaces/
│   └── IServices.cs       — IOrderService, IMenuService, INotificationService
├── Services/
│   ├── MenuService.cs     — меню пиццерии
│   ├── OrderService.cs    — бизнес-логика заказов
│   └── ConsoleNotificationService.cs
├── Pizzeria.cs            — фасад
└── Program.cs             — консольный интерфейс
```

## Жизненный цикл заказа
`Pending → Cooking → Ready → PickedUp`

## Запуск
```bash
PizzaTime.sln - Запустить в visual studio, собрать и запустить проект
```

## Тестовый сценарий
1. Выбрать **2** — сделать заказ (имя, телефон, Id пицц через запятую)
2. Выбрать **3** — приготовить заказ (ввести номер заказа)
3. Выбрать **4** — убедиться что имя появилось на табло
4. Выбрать **5** — забрать заказ (имя исчезает с табло)


