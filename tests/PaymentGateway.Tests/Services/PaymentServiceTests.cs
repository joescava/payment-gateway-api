using Xunit;
using PaymentGateway.Application.Services;
using PaymentGateway.Infrastructure.MockAcquirer;
using PaymentGateway.Persistence.Repositories;
using PaymentGateway.Application.DTOs;

namespace PaymentGateway.Tests.Services;

public class PaymentServiceTests
{
    private PaymentService CreateService()
    {
        var acquirer = new FakeAcquirer();
        var repository = new InMemoryPaymentRepository();
        return new PaymentService(acquirer, repository);
    }

    [Fact]
    public void ProcessPayment_ReturnsResponseWithStatus()
    {
        var service = CreateService();

        var request = new PaymentRequestDto(100, "USD", "4111111111111111", 12, 2026, "123");
        var response = service.ProcessPayment(request);

        Assert.NotEqual(Guid.Empty, response.PaymentId);
        Assert.Contains(response.Status, new[] { "Approved", "Declined" });
    }

    [Fact]
    public void GetPaymentStatus_ReturnsCorrectStatus()
    {
        var service = CreateService();

        var request = new PaymentRequestDto(50, "USD", "4111111111111", 10, 2027, "456");
        var result = service.ProcessPayment(request);

        var status = service.GetPaymentStatus(result.PaymentId);
        Assert.Equal(result.Status, status);
    }

    [Theory]
    [InlineData(-10, "USD", "4111111111111", 12, 2026, "123", "Amount must be greater than zero.")]
    [InlineData(10, "", "4111111111111", 12, 2026, "123", "Currency is required.")]
    [InlineData(10, "USD", "1234567", 12, 2026, "123", "Card number must be between 13 and 19 digits.")]
    [InlineData(10, "USD", "11111111111111111111", 12, 2026, "123", "Card number must be between 13 and 19 digits.")]
    [InlineData(10, "USD", "4111111111111", 0, 2026, "123", "Expiry month must be between 1 and 12.")]
    [InlineData(10, "USD", "4111111111111", 12, 2010, "123", "Expiry year must be current or in the future.")]
    [InlineData(10, "USD", "4111111111111", 12, 2026, "abc", "CVV must be 3 or 4 digits.")]
    [InlineData(10, "USD", "4111111111111", 12, 2026, "1", "CVV must be 3 or 4 digits.")]
    [InlineData(10, "USD", "4111111111111", 12, 2026, "12345", "CVV must be 3 or 4 digits.")]
    public void ProcessPayment_ThrowsException_ForInvalidInput(
        decimal amount, string currency, string cardNumber,
        int expiryMonth, int expiryYear, string cvv, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new PaymentRequestDto(amount, currency, cardNumber, expiryMonth, expiryYear, cvv)
        );
        Assert.Equal(expectedMessage, ex.Message);
    }
}