using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.DTOs;
using PaymentGateway.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PaymentGateway.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public IActionResult ProcessPayment([FromBody] PaymentRequestDto request)
    {
        var result = _paymentService.ProcessPayment(request);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetStatus(Guid id)
    {
        var status = _paymentService.GetPaymentStatus(id);
        return Ok(status);
    }
}