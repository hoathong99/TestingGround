using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using MHPQ.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public interface IThemeAppService : IApplicationService
    {
        Task<object> GetAllTheme(GetThemeInput input);
        Task<object> GetByIdTheme(long id);
        Task<object> DeleteTheme(long id);
    }

        public class ThemeAppService : MHPQAppServiceBase, IThemeAppService
        {

        private readonly IRepository<Device, long> _deviceRepos;
        private readonly IRepository<SmartHome, long> _smartHomeRepos;
        private readonly IRepository<HomeServer, long> _homeServerRepos;
        private readonly IRepository<Theme, long> _themeRepos;
        private readonly IRepository<FloorSmartHome, long> _floorSmarHomeRepos;
        private readonly IRepository<RoomSmartHome, long> _roomSmarHomeRepos;

        public ThemeAppService(

            IRepository<Device, long> deviceRepos,
            IRepository<SmartHome, long> smartHomeRepos,
            IRepository<HomeServer, long> homeServerRepos,
            IRepository<Theme, long> themeRepos,
            IRepository<FloorSmartHome, long> floorSmarHomeRepos,
            IRepository<RoomSmartHome, long> roomSmarHomeRepos
        )
        {
            _deviceRepos = deviceRepos;
            _floorSmarHomeRepos = floorSmarHomeRepos;
            _homeServerRepos = homeServerRepos;
            _smartHomeRepos = smartHomeRepos;
            _themeRepos = themeRepos;
            _roomSmarHomeRepos = roomSmarHomeRepos;
        }


        [Obsolete]
        public async Task<long> CreateOrUpdateTheme(ThemeInput input)
        {

            try
            {
                if (input.Id > 0)
                {
                    //update
                    var updateData = await _themeRepos.GetAsync(input.Id);
                    if (updateData != null)
                    {
                        input.MapTo(updateData);
                        await _themeRepos.UpdateAsync(updateData);


                    }
                }
                else
                {
                    //Insert
                    var insertInput = input.MapTo<Theme>();
                    long id = await _themeRepos.InsertAndGetIdAsync(insertInput);
                }

                return 1;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return -1;
            }
        }

        public async Task<object> GetAllTheme(GetThemeInput input)
        {
            try
            {
                //var query = (from room in _roomSmarHomeRepos.GetAll()
                //             select new RoomOutputDto()
                //             {
                //                 Id = room.Id,
                //                 CreationTime = room.CreationTime,
                //                 CreatorUserId = room.CreatorUserId,
                //                 FloorSmartHomeId = room.FloorSmartHomeId,
                //                 ImageUrl = room.ImageUrl,
                //                 Name = room.Name,
                //                 Number = room.Number,
                //                 NumberDevices = room.NumberDevices,
                //                 SmartHomeId = room.SmartHomeId
                //             });

                var result = await _themeRepos.GetAllListAsync(x => x.RoomSmartHomeId == input.RoomId);
                var data = DataResult.ResultSucces(result, "Get success!");
                return data;




            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }
        }

        public async Task<object> GetByIdTheme(long id)
        {
            try
            {
                var result = await _themeRepos.GetAsync(id);

                var data = DataResult.ResultSucces(result, "Get success!");
                return data;
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;

            }
        }

        public async Task<object> DeleteTheme(long id)
        {
            try
            {
                var theme = await _themeRepos.GetAsync(id);
                if (theme != null)
                {
                    await _themeRepos.DeleteAsync(theme);
                    var data = DataResult.ResultSucces("Xóa thành công !");
                    return data;
                }
                else
                {
                    var data = DataResult.ResultFail("Chủ đề không tồn tại !");
                    return data;
                }
            }
            catch (Exception e)
            {
                var data = DataResult.ResultError(e.ToString(), "Có lỗi");
                return data;
            }
        }

    }
}
