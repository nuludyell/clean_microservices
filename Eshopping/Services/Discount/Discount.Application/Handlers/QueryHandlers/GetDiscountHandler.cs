using AutoMapper;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers.QueryHandlers;

public class GetDiscountHandler : IRequestHandler<GetDiscountQuery, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public GetDiscountHandler(
        IDiscountRepository discountRepository,
        IMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _discountRepository.GetDiscountAsync(request.ProductName);

        if (coupon == null)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound,
                $"Discount with the product name = {request.ProductName} not found."));
        }

        // TODO: Exercise Follow Product Mapper kind of example
        //var couponModel = _mapper.Map<CouponModel>(coupon);
        var couponModel = new CouponModel
        {
            Id = coupon.Id,
            Amount = coupon.Amount,
            Description = coupon.Description,
            ProductName = coupon.ProductName
        };

        return couponModel;
    }
}
