using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.Authorization.Users;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface ICitizenAppService : IApplicationService
    {
        Task<object> GetAllCitizenAsync();
       // Task<object> CreateListCitizenAsync(List<CitizenDto> input);
        Task<object> GetCitizenByIdAsync(long id);

        Task<object> GetAllAccountTenant();
        Task<object> CreateOrUpdateCitizen(CitizenDto input);
        Task<object> DeleteCitizen(long id);

        Task<object> GetAllSmarthomeTenant();
        Task<object> GetAllSmarthomeTenantByUser(long UserId);
    }
    public class CitizenAppService : MHPQAppServiceBase, ICitizenAppService
    {
        private readonly IRepository<Citizen, long> _citizenRepos;
        private readonly IRepository<User, long> _userRepos;
        private readonly IRepository<SmartHome, long> _smartHomeRepos;
        private readonly IRepository<HouseOwner, long> _memberRepos;
        public CitizenAppService(
          IRepository<User, long> userRepos,
          IRepository<Citizen, long> citizenRepos,
          IRepository<SmartHome, long> smartHomeRepos,
          IRepository<HouseOwner, long> memberRepos
            ) 
        {
            _userRepos = userRepos;
            _citizenRepos = citizenRepos;
            _smartHomeRepos = smartHomeRepos;
            _memberRepos = memberRepos;
        }

        //public async Task<object> CreateListCitizenAsync(List<CitizenDto> input)
        //{
        //    throw new System.NotImplementedException();
        //}

        [Obsolete]
        public async Task<object> CreateOrUpdateCitizen(CitizenDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                input.TenantId = AbpSession.TenantId;
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _citizenRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);

                        //call back
                        await _citizenRepos.UpdateAsync(updateData);
                    }
                    mb.statisticMetris(t1, 0, "Ud_citizen");

                    var data = DataResult.ResultSucces(updateData, "Update success !");
                    return data;
                }
                else
                {
                    var insertInput = input.MapTo<Citizen>();
                    long id = await _citizenRepos.InsertAndGetIdAsync(insertInput);
                    insertInput.Id = id;
                    mb.statisticMetris(t1, 0, "is_citizen");
                    var data = DataResult.ResultSucces(insertInput, "Insert success !");
                    return data;
                }

            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> DeleteCitizen(long id)
        {
            try
            {

                 await _citizenRepos.DeleteAsync(id);
                var data = DataResult.ResultSucces("Delete success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetAllCitizenAsync()
        {
            try
            {
                using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
                {
                    var query = (from ci in _citizenRepos.GetAll()
                                 join us in _userRepos.GetAll() on ci.AccountId equals us.Id into tb_us
                                 from us in tb_us.DefaultIfEmpty()
                                 select new CitizenDto()
                                 {
                                     Id = ci.Id,
                                     PhoneNumber = ci.PhoneNumber != null? ci.PhoneNumber: us.PhoneNumber,
                                     Nationality = ci.Nationality,
                                     FullName = ci.FullName,
                                     IdentityNumber = ci.IdentityNumber,
                                     ImageUrl = ci.ImageUrl != null? ci.ImageUrl : us.ImageUrl,
                                     Email = ci.Email != null ? ci.Email : us.EmailAddress,
                                     HomeAddress = ci.HomeAddress,
                                     Address = ci.Address,
                                     DateOfBirth = ci.DateOfBirth,
                                     AccountId = ci.AccountId,
                                     Gender = ci.Gender
                                 });

                    var result = await query.ToListAsync();
                    var data = DataResult.ResultSucces(result, "Get success!");
                    return data;
                }
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetAllAccountTenant()
        {
            try
            {
                using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
                {
                    var query = (from us in _userRepos.GetAll()
                                 //join ci in _citizenRepos.GetAll() on us.Id equals ci.AccountId into tb_ci
                                 //from ci in tb_ci.DefaultIfEmpty()
                                 where us.TenantId > 0
                                 select new AccountDto()
                                 {
                                     FullName = us.FullName,
                                     ImageUrl = us.ImageUrl,
                                     Id = us.Id,
                                     UserName = us.UserName

                                 }).AsQueryable();

                    var result = await query.ToListAsync();
                    var data = DataResult.ResultSucces(result, "Get success!");
                    return data;
                }
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetCitizenByIdAsync(long id)
        {
            try
            {

                var result = await _citizenRepos.GetAsync(id);
                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetAllSmarthomeTenant()
        {
            try
            {



                var result = await _smartHomeRepos.GetAllListAsync();

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }

        public async Task<object> GetAllSmarthomeTenantByUser(long userId)
        {
            try
            {

                var query = (from sm in _smartHomeRepos.GetAll()
                             join mb in _memberRepos.GetAll() on sm.SmartHomeCode equals mb.SmartHomeCode into tb_mb
                             from mb in tb_mb.DefaultIfEmpty()
                             where sm.TenantId == AbpSession.TenantId && mb.UserId == userId
                             select new SmarthomeTenantDto
                             {
                                 Id = sm.Id,
                                 ImageUrl = sm.ImageUrl,
                                 Name = sm.Name,
                                 SmarthomeCode = sm.SmartHomeCode
                             }).AsQueryable();

                var result = await query.ToListAsync();

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Exception !");
                Logger.Fatal(e.Message);
                return data;
            }
        }
    }
}
