using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using MHPQ.Authorization.Users;
using MHPQ.MultiTenancy;
using Microsoft.AspNetCore.Http;
using Abp.Domain.Repositories;
using Abp.Domain.Entities;
using System.Reflection;
using System.Collections.Generic;
using MHPQ.Services.Dto;
using MHPQ.Services;
using MHPQ.EntityDb;
using Abp.Domain.Entities.Auditing;

namespace MHPQ
{
    public abstract class MHPQDeviceServiceBase<T, TPrimaryKey> : ApplicationService where T : class, IDevice
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        public IRepository<T, long> _entityRepo;

        protected MHPQDeviceServiceBase(IRepository<T, long> entityRepo)
        {
            LocalizationSourceName = MHPQConsts.LocalizationSourceName;
            _entityRepo = entityRepo;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        public virtual async Task<object> GetAll()
        {
            try
            {
                var result = await _entityRepo.GetAllListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<object> GetBySmartHomeId(long smartHomeId)
        {
            try
            {
                var result = await _entityRepo.GetAllListAsync(et => et.SmartHomeId == smartHomeId);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<object> Create(T entity)
        {
            try
            {
                await _entityRepo.InsertAsync(entity);
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public virtual async Task<object> Update(T entity)
        {
            try
            {
                await _entityRepo.UpdateAsync(entity);
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public virtual async Task<object> Delete(T entity)
        {
            try
            {
                await _entityRepo.DeleteAsync(entity);
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
