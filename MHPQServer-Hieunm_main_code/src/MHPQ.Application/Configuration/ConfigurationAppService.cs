using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using MHPQ.Configuration.Dto;

namespace MHPQ.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : MHPQAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
