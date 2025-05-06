namespace PaymentGateway.Application.DTOs;
public class PaymentResponseDto
{
    public Guid PaymentId { get; set; }
    public string Status { get; set; }
    public string MaskedCardNumber { get; set; }

    public PaymentResponseDto(Guid paymentId, string status, string maskedCardNumber)
    {
        PaymentId = paymentId;
        Status = status;
        MaskedCardNumber = maskedCardNumber;
    }
}