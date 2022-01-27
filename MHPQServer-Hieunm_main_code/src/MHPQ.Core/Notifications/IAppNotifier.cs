using System.Threading.Tasks;
using Abp;
using Abp.Notifications;
using MHPQ.Authorization.Users;
using MHPQ.MultiTenancy;

namespace MHPQ.Notifications
{
    public interface IAppNotifier
    {
        Task WelcomeToTheApplicationAsync(User user);

        Task NewUserRegisteredAsync(User user);

        Task NewTenantRegisteredAsync(Tenant tenant);

        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);
    }
}
