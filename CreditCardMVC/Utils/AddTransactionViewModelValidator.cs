using FluentValidation;
using CreditCardMVC.Models.ViewModels;

public class AddTransactionViewModelValidator : AbstractValidator<AddTransactionViewModel>
{
    public AddTransactionViewModelValidator()
    {
        RuleFor(x => x.CardHolderID)
            .GreaterThan(0).WithMessage("El ID del titular de la tarjeta debe ser mayor a 0.");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("La fecha de transacción es obligatoria.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(255).WithMessage("La descripción no puede exceder los 255 caracteres.");

        RuleFor(x => x.TransactionType)
            .NotEmpty().WithMessage("El tipo de transacción es obligatorio.")
            .Must(type => type == "Compra" || type == "Pago")
            .WithMessage("El tipo de transacción debe ser 'Compra' o 'Pago'.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");
    }
}
