namespace PaymentGateway.Application.DTOs;

public class PaymentRequestDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string CVV { get; set; }

    public PaymentRequestDto(decimal amount, string currency, string cardNumber, int expiryMonth, int expiryYear, string cvv)
    {

        if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.");
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required.");
        if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 13 || cardNumber.Length > 19)
            throw new ArgumentException("Card number must be between 13 and 19 digits.");
        if (expiryMonth is < 1 or > 12) throw new ArgumentException("Expiry month must be between 1 and 12.");
        if (expiryYear < DateTime.UtcNow.Year)
            throw new ArgumentException("Expiry year must be current or in the future.");
        if (string.IsNullOrWhiteSpace(cvv) || !cvv.All(char.IsDigit) || cvv.Length is < 3 or > 4)
            throw new ArgumentException("CVV must be 3 or 4 digits.");
            
        Amount = amount;
        Currency = currency;
        CardNumber = cardNumber;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        CVV = cvv;
    }
}