using Abp.Application.Services;
using MHPQ.Services.DichVu;
using MHPQ.Services.QuanLyKhachSan.ThuePhong;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.ThanhToan
{
    public interface IThanhToanAppService : IApplicationService
    {
        Task<object> PaypalCheckout();
        Task<object> VnpayCheckout();
        Task<object> PhongKhachSan(BookingRoomHotelDto bookingDto);
        Task<object> DichVu(HireServiceDto hireDto);
    }
}
