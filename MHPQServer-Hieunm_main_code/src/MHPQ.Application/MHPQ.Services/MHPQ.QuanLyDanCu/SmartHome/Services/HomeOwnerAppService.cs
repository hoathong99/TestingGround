using Abp.Application.Services;
using Abp.Domain.Repositories;
using MHPQ.Authorization.Users;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public class HouseOwnerDto : HouseOwner
    {
        public string OwnerName { get; set; }
    }

    public interface IHomeOwnerAppService: IApplicationService
    {
        Task<object> GetAll();
        Task<object> Create(HouseOwner createInput);
        Task<object> Update(HouseOwner updateInput);
    }

    public class HomeOwnerAppService : MHPQAppServiceBase, IHomeOwnerAppService
    {
        private readonly IRepository<HouseOwner, long> _houseOwnerRepos;
        private readonly IRepository<User, long> _userRepos;
        public HomeOwnerAppService(
            IRepository<HouseOwner, long> houseOwnerRepos,
            IRepository<User, long> userRepos)
        {
            _houseOwnerRepos = houseOwnerRepos;
            _userRepos = userRepos;
        }
        public async Task<object> GetAll()
        {
            try
            {
                var owner = from houseOwner in _houseOwnerRepos.GetAll()
                            join user in _userRepos.GetAll()
                            on houseOwner.UserId equals user.Id
                            select new HouseOwnerDto() { 
                                Id = houseOwner.Id,
                                IsVote = houseOwner.IsVote,
                                UserId = houseOwner.UserId,
                                SmartHomeId = houseOwner.SmartHomeId,
                                OwnerName = user.Name
                            };
                return owner.ToList();
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message, e);
                return null;
            }
        }

        public async Task<object> Create(HouseOwner createInput)
        {
            try
            {
                return await _houseOwnerRepos.InsertAsync(createInput);
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message, e);
                return null;
            }
        }

        public async Task<object> Update(HouseOwner updateInput)
        {
            try
            {
                return await _houseOwnerRepos.UpdateAsync(updateInput);
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message, e);
                return null;
            }
        }
    }
}
