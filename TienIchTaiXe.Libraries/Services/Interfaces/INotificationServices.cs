using TienIchTaiXe.Libraries.Models.GGSheets;

namespace TienIchTaiXe.Libraries.Services.Interfaces;

public interface INotificationServices
{
    Task<List<GGNotification>> GetsGGNotification();
    Task<string> PostGGNotification(GGNotification dt);
}
