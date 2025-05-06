namespace PaymentGateway.Domain.Entities;

public class Payment
{
    public Guid Id { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public string CardNumber { get; }
    public string Status { get; }

    public Payment(Guid id, decimal amount, string currency, string cardNumber, string status)
    {
        Id = id;
        Amount = amount;
        Currency = currency;
        CardNumber = cardNumber;
        Status = status;
    }
}