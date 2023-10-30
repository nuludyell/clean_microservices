using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Queries;

public sealed record GetDiscountQuery(string ProductName) : IRequest<CouponModel>;