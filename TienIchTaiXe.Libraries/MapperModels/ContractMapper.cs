using TienIchTaiXe.Libraries.Entities;
using TienIchTaiXe.Libraries.Models;

namespace TienIchTaiXe.Libraries.MapperModels;

public static class ContractMapper
{
    public static TienIchTaiXe.Libraries.Models.Contract ContractDtoToEntity(this ContractDto src, string shiftworkId)
    {
        return new TienIchTaiXe.Libraries.Models.Contract
        {
            ctId = string.IsNullOrWhiteSpace(src.ctId) ? Guid.NewGuid().ToString() : src.ctId,
            numberCar = src.numberCar,
            ctKey = src.ctKey,
            ctAmount = src.ctAmount,
            ctDefaultDistance = src.ctDefaultDistance,
            ctOverDistance = src.ctOverDistance,
            ctSurcharge = src.ctSurcharge,
            ctPromotion = src.ctPromotion,
            totalPrice = src.totalPrice,
            userId = src.userId,
            createdAt = src.createdAt,
            shiftworkId = shiftworkId
        };
    }
}
