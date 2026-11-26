using FluentValidation;

namespace CourseShop.Application.Ordering.CreateOrder;

/// <summary>
/// Валідатор команди створення замовлення (FluentValidation).
/// Містить синтаксичні перевірки та приклад семантичного правила
/// (заборона дублювання товарів у межах одного замовлення).
/// </summary>
public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Замовлення має належати користувачу.");

        RuleFor(x => x.Items)
            .NotNull().NotEmpty().WithMessage("Замовлення має містити хоча б одну позицію.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductName)
                .NotEmpty().WithMessage("Назва товару обов'язкова.");
            item.RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("Ціна товару має бути більшою за нуль.");
            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Кількість має бути більшою за нуль.");
        });

        // Семантична перевірка: один товар не може повторюватися у замовленні.
        RuleFor(x => x.Items)
            .Must(items => items == null || items
                .GroupBy(i => i.ProductName, StringComparer.OrdinalIgnoreCase)
                .All(g => g.Count() == 1))
            .WithMessage("Товари у замовленні не повинні повторюватися.");
    }
}
