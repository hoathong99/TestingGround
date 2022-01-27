using Abp.Domain.Repositories;
using MHPQ.Common.DataResult;
using MHPQ.EntityDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.QuanLyKhachSan.ThuePhong
{
    public class BookingRoomHotelAppService : MHPQAppServiceBase, IBookingRoomHotelAppService
    {
        private readonly IRepository<BookingRoomHotel, long> _bookingRoomHotelRepo;
        private readonly IRepository<RoomHotel, long> _roomHotelRepo;
        private readonly IRepository<GuestHotel, long> _guestHotelRepo;

        public BookingRoomHotelAppService(
            IRepository<BookingRoomHotel, long> bookingRoomHotelRepo, 
            IRepository<RoomHotel, long> roomHotelRepo,
            IRepository<GuestHotel, long> guestHotelRepo)
        {
            _bookingRoomHotelRepo = bookingRoomHotelRepo;
            _roomHotelRepo = roomHotelRepo;
            _guestHotelRepo = guestHotelRepo;
        }

        public async Task<object> GetAll()
        {

            try
            {
                var result = await _bookingRoomHotelRepo.GetAllListAsync();

                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }

        public async Task<object> GetBookingByHotelId(long hotelId)
        {

            try
            {
                var result = from bookings in _bookingRoomHotelRepo.GetAll()
                             join rooms in _roomHotelRepo.GetAll()
                             on bookings.RoomHotelId equals rooms.Id
                             join guests in _guestHotelRepo.GetAll()
                             on bookings.GuestHotelId equals guests.Id
                             where rooms.HotelId == hotelId
                             select new BookingGuestHotelDto { 
                                Id = bookings.Id,
                                GuestHotelName = guests.FullName,
                                GuestHotelIdentity = guests.IdentityNumber,
                                StartDate = bookings.StartDate,
                                EndDate = bookings.EndDate,
                                IsPaid = bookings.IsPaid,
                                Money = bookings.Money,
                                NumberPeople = bookings.NumberPeople,
                                RoomHotelName = rooms.RoomHotelName,
                             };

                var data = DataResult.ResultSucces(result.ToList(), Common.Resource.QuanLyChung.GetAllSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }

        public async Task<object> GetEmptyRoom()
        {
            try
            {
                var allRoom = await _roomHotelRepo.GetAllListAsync();

                var emptyRoom = from rooms in _roomHotelRepo.GetAll()
                                where rooms.IsRent == false
                                select rooms;
                var result = emptyRoom.ToList();

                var data = DataResult.ResultSucces(result, Common.Resource.QuanLyChung.GetEmptyRoom);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        public async Task<object> CreateBooking(BookingRoomHotelDto bookingDto)
        {
            try
            {
                //check phòng trống
                var roomEmpty = from room in _roomHotelRepo.GetAll()
                                where room.Id == bookingDto.RoomHotelId
                                && room.IsRent == false
                                select room;

                if (roomEmpty != null && roomEmpty.Count() > 0)
                {
                    //thêm booking vào db
                    var bookingEntity = ObjectMapper.Map<BookingRoomHotel>(bookingDto); 
                    await _bookingRoomHotelRepo.InsertAsync(bookingEntity);

                    //cập nhật trạng thái phòng đó
                    var roomListInUse = _roomHotelRepo.GetAll().Where(room => room.Id == bookingDto.RoomHotelId).ToList();
                    roomListInUse.ForEach(room => room.IsRent = true);
                }

                var data = DataResult.ResultSucces(Common.Resource.QuanLyChung.InsertSuccess);
                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        /// <summary>
        /// Hàm checkBooking được gọi thường xuyên từ BackgroundJob
        /// </summary>
        public void CheckBooking()
        {
            try
            {
                //Lấy các booking hết hạn
                var bookingRoomId = from bookings in _bookingRoomHotelRepo.GetAll()
                                    where bookings.EndDate < DateTime.Now
                                    select bookings.RoomHotelId;


                //cập nhật lại trạng thái phòng
                var roomListExpire = _roomHotelRepo.GetAll().Where(room => bookingRoomId.Contains(room.Id)).ToList();

                roomListExpire.ForEach(room => room.IsRent = false);
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
            }

        }
    }
}
