using Abp.Application.Services;
using MHPQ.Services.DichVu.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.DichVu
{
    public interface IFootballServiceAppService : IApplicationService
    {
        Task<object> UpdateFootballPitch(FootballPitchDto dto);
        Task<object> GetAllPitches();
        Task<object> GetAllPitchBooking();
        Task<object> GetAllBookingOfPitchToday(long? pitchId);
        Task<object> CreateBooking(PitchBookingDto dto);
    }
}
