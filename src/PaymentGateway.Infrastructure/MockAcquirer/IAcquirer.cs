using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Infrastructure.MockAcquirer;

public interface IAcquirer
{
    bool AuthorizePayment(Payment payment);
}