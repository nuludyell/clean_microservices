using MediatR;
namespace Ordering.Application.Commands;

public sealed record UpdateOrderCommand(
    int Id,
    string Username,
    double TotalPrice,
    string FirstName,
    string LastName,
    string EmailAddess,
    string AddressLine,
    string Country,
    string State,
    string Zipcode,
    string CardName,
    string CardNumber,
    string Expiration,
    string Cvv,
    int? PaymentMethod) : IRequest<Unit>
{
}
