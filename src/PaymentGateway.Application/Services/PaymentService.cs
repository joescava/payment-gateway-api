using PaymentGateway.Application.DTOs;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Infrastructure.MockAcquirer;
using PaymentGateway.Persistence.Repositories;

namespace PaymentGateway.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IAcquirer _acquirer;
    private readonly InMemoryPaymentRepository _repository;

    public PaymentService(IAcquirer acquirer, InMemoryPaymentRepository repository)
    {
        _acquirer = acquirer;
        _repository = repository;
    }

    public PaymentResponseDto ProcessPayment(PaymentRequestDto request)
    {
        var payment = CreatePayment(request);
        _repository.Save(payment);

        return new PaymentResponseDto(
            payment.Id,
            payment.Status,
            MaskCardNumber(payment.CardNumber)
        );
    }

    public string GetPaymentStatus(Guid id)
    {
        var payment = _repository.GetById(id);
        return payment?.Status ?? "Not Found";
    }

    private Payment CreatePayment(PaymentRequestDto request)
    {
        var id = Guid.NewGuid();
        var isAuthorized = _acquirer.AuthorizePayment(
            new Payment(id, request.Amount, request.Currency, request.CardNumber, "Pending")
        );

        var finalStatus = isAuthorized ? "Approved" : "Declined";

        return new Payment(id, request.Amount, request.Currency, request.CardNumber, finalStatus);
    }

    private static string MaskCardNumber(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 4)
            return "****";
        return $"**** **** **** {cardNumber[^4..]}";
    }
}