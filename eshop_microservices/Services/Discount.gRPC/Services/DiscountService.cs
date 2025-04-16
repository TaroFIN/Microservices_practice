using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Discount.Grpc;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        //TODO: GetDiscount from DB
        var coupon = await dbContext.Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName, context.CancellationToken);

        if (coupon is null)
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount" };

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if(coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon is null"));

        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation("Discount is successfully created. ProductName: {ProductName}, Amount: {Amount}",
            coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon is null"));

        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation("Discount is successfully updated. ProductName: {ProductName}, Amount: {Amount}",
            coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName, context.CancellationToken);
        if (coupon is null) throw new RpcException(new Status(StatusCode.NotFound, "Coupon is null"));

        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation("Discount is successfully deleted. ProductName: {ProductName}, Amount: {Amount}",
            coupon.ProductName, coupon.Amount);

        return new DeleteDiscountResponse { Success = true };
    }
}

