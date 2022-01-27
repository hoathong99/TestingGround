using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq;
using Abp.Organizations;
using MHPQ.Authorization.Roles;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHPQ.Authorization.Users
{
    public class UserStore : AbpUserStore<Role, User>
    {
        private readonly IRepository<Role> _roleRepository;
        private IRepository<UserPermissionSetting, long> _userPermissionSettingRepository;
        public UserStore(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserClaim, long> userClaimRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository) 
            : base(unitOfWorkManager,
                  userRepository,
                  roleRepository,
                  userRoleRepository,
                  userLoginRepository,
                  userClaimRepository,
                  userPermissionSettingRepository,
                  userOrganizationUnitRepository,
                  organizationUnitRoleRepository
                  )
        {
            _roleRepository = roleRepository;
            _userPermissionSettingRepository = userPermissionSettingRepository;
        }

        public async Task<List<User>> GetAllUserTenantAsync()
        {   
            var users = await UserRepository.GetAllListAsync();
            return users;
        }

        public async Task<List<User>> GetAllCitizenManagerTenantAsync()
        {
            //var userPermiss = _userPermissionSettingRepository.
            var roleCitizenManager = await _roleRepository.FirstOrDefaultAsync(x => x.Name == StaticRoleNames.Tenants.CitizenManager);
            if(roleCitizenManager != null)
            {
                var users = await UserRepository.GetAllIncluding(x => x.Roles).Where(y => y.Roles.Any(m => m.RoleId == roleCitizenManager.Id)).ToListAsync();
                return users;
            }
            else
            {
                return null;
            }
        
        }
    }
}
