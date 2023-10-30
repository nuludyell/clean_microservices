using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Commands;

public sealed record UpdateDiscountCommand(
    int Id,
    string ProductName,
    string Description,
    double Amount) : IRequest<CouponModel>;