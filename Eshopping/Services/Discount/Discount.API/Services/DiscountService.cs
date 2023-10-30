using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(
        IMediator mediator,
        ILogger<DiscountService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetDiscountQuery(request.ProductName));
        _logger.LogInformation($"Discount is retrieved for the Product Name: {request.ProductName} and Amount: {result.Amount}.");

        return result;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new CreateDiscountCommand(
            request.Coupon.ProductName,
            request.Coupon.Description,
            request.Coupon.Amount));
        _logger.LogInformation($"Discount was successfully created for the Product Name: {result.ProductName}");

        return result;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new UpdateDiscountCommand(
            request.Coupon.Id,
            request.Coupon.ProductName,
            request.Coupon.Description,
            request.Coupon.Amount));
        _logger.LogInformation($"Discount was successfully updated for the Product Name: {result.ProductName}");

        return result;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await _mediator.Send(new DeleteDiscountCommand(request.ProductName));

        return new DeleteDiscountResponse
        {
            Success = deleted
        };
    }
}