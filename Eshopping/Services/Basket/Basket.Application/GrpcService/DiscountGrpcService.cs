using Discount.Grpc.Protos;

namespace Basket.Application.GrpcService;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<CouponModel> GetDiscountAsync(string productName)
    {
        return await _discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest
        {
            ProductName = productName
        });
    }
}
