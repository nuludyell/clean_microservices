using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Commands;

public sealed record CreateDiscountCommand(
    string ProductName,
    string Description,
    double Amount) : IRequest<CouponModel>;