using PaymentGateway.Domain.Entities;
using System.Collections.Concurrent;

namespace PaymentGateway.Persistence.Repositories;

public class InMemoryPaymentRepository
{
    private static readonly ConcurrentDictionary<Guid, Payment> _payments = new();

    public void Save(Payment payment) => _payments[payment.Id] = payment;
    public Payment? GetById(Guid id) => _payments.TryGetValue(id, out var payment) ? payment : null;
}