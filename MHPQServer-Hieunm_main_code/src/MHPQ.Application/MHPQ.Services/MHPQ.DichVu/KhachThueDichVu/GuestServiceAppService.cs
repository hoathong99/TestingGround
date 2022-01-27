using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public class GuestServiceAppService : MHPQAppServiceBase, IGuestServiceAppService
    {
        private readonly IRepository<GuestService, long> _guestRepository;

        public GuestServiceAppService(IRepository<GuestService, long> guestRepository)
        {
            _guestRepository = guestRepository;
        }

        public async Task<object> GetAll()
        {
            try
            {
                var result = await _guestRepository.GetAllListAsync();

                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return null;
            }

        }
        public async Task<object> Create(GuestServiceDto guestDto)
        {
            try
            {
                var guest = new GuestService
                {
                    FullName = guestDto.FullName,
                    Email = guestDto.Email,
                    IdentityNumber = guestDto.IdentityNumber,
                    PhoneNumber = guestDto.PhoneNumber,
                };


                await _guestRepository.InsertAsync(guest);

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.InsertSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return null;
            }
        }
        public async Task<object> Update(GuestServiceDto guestDto)
        {
            try
            {
                var guest = new GuestService
                {
                    Id = guestDto.GuestServiceId,
                    FullName = guestDto.FullName,
                    Email = guestDto.Email,
                    IdentityNumber = guestDto.IdentityNumber,
                    PhoneNumber = guestDto.PhoneNumber,
                };

                await _guestRepository.UpdateAsync(guest);

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.UpdateSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return null;
            }
        }
        public async Task<object> Delete(GuestServiceDto guestDto)
        {
            try
            {
                await _guestRepository.DeleteAsync(guestDto.GuestServiceId);

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.DeleteSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return null;
            }
        }
    }
}
