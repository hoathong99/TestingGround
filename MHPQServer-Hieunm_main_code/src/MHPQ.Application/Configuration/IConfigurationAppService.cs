using System.Threading.Tasks;
using MHPQ.Configuration.Dto;

namespace MHPQ.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
