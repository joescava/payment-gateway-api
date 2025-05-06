using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Infrastructure.MockAcquirer;

public class FakeAcquirer : IAcquirer
{
    public bool AuthorizePayment(Payment payment)
    {
        return new Random().Next(2) == 0;
    }
}