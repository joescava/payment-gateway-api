using PaymentGateway.Application.DTOs;

namespace PaymentGateway.Application.Interfaces;

public interface IPaymentService
{
    PaymentResponseDto ProcessPayment(PaymentRequestDto request);
    string GetPaymentStatus(Guid id);
}