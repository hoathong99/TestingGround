using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using MHPQ.Authorization;
using MHPQ.Authorization.Roles;
using MHPQ.Authorization.Users;
using MHPQ.Editions;
using MHPQ.MultiTenancy.Dto;
using Microsoft.AspNetCore.Identity;

namespace MHPQ.MultiTenancy
{
    [AbpAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantAppService : AsyncCrudAppService<Tenant, TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>, ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;

        public TenantAppService(
            IRepository<Tenant, int> repository,
            TenantManager tenantManager,
            EditionManager editionManager,
            UserManager userManager,
            RoleManager roleManager,
            IPermissionManager permissionManager,
            IAbpZeroDbMigrator abpZeroDbMigrator)
            : base(repository)
        {
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _permissionManager = permissionManager;
        }

        public override async Task<TenantDto> CreateAsync(CreateTenantDto input)
        {
            try
            {
                CheckCreatePermission();

                // Create tenant
                var tenant = ObjectMapper.Map<Tenant>(input);
                tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                    ? null
                    : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

                var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
                if (defaultEdition != null)
                {
                    tenant.EditionId = defaultEdition.Id;
                }

                await _tenantManager.CreateAsync(tenant);
                await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

                // Create tenant database
                _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

                // We are working entities of new tenant, so changing tenant filter
                using (CurrentUnitOfWork.SetTenantId(tenant.Id))
                {
                    // Create static roles for new tenant
                    CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                    await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids
                                                                //role user default

                    //user default
                    var userRole = _roleManager.Roles.FirstOrDefault(r => r.Name == StaticRoleNames.Tenants.DefaultUser);
                    if (userRole == null)
                    {
                        userRole =  _roleManager.CreateRole(new Role(tenant.Id, StaticRoleNames.Tenants.DefaultUser, StaticRoleNames.Tenants.DefaultUser) { IsStatic = true });
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var permissionUser = _permissionManager.GetPermission(PermissionNames.Pages_User_Detail);
                    await _roleManager.GrantPermissionAsync(userRole, permissionUser);
                    //citizen manager
                    var citizenManagerRole = _roleManager.Roles.FirstOrDefault(r => r.Name == StaticRoleNames.Tenants.CitizenManager);
                    if (citizenManagerRole == null)
                    {
                        citizenManagerRole = _roleManager.CreateRole(new Role(tenant.Id, StaticRoleNames.Tenants.CitizenManager, StaticRoleNames.Tenants.CitizenManager) { IsStatic = true });
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var permissionCitizenManager = _permissionManager.GetPermission(PermissionNames.Pages_SmartCommunity_Citizen);
                    await _roleManager.GrantPermissionAsync(citizenManagerRole, permissionCitizenManager);

                    // Grant all permissions to admin role
                    var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                    await _roleManager.GrantAllPermissionsAsync(adminRole);

                    // Create admin user for the tenant
                    var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress);
                    await _userManager.InitializeOptionsAsync(tenant.Id);
                    CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));
                    await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

                    // Assign admin user to role!
                    CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
                    await CurrentUnitOfWork.SaveChangesAsync();
                }

                return MapToEntityDto(tenant);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        protected override IQueryable<Tenant> CreateFilteredQuery(PagedTenantResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.TenancyName.Contains(input.Keyword) || x.Name.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override void MapToEntity(TenantDto updateInput, Tenant entity)
        {
            // Manually mapped since TenantDto contains non-editable properties too.
            entity.Name = updateInput.Name;
            entity.TenancyName = updateInput.TenancyName;
            entity.IsActive = updateInput.IsActive;
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var tenant = await _tenantManager.GetByIdAsync(input.Id);
            await _tenantManager.DeleteAsync(tenant);
        }

        private void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}

