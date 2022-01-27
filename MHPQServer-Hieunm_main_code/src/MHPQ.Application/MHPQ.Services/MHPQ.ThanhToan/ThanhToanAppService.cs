using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PayPal.Core;
using PayPal.v1.Payments;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MHPQ.EntityDb;
using MHPQ.Common.DataResult;
using System.Text.Json;
using System.Text;
using MHPQ.Sessions;
using MHPQ.Services.ThanhToan.Dto;
using MHPQ.Services.QuanLyKhachSan.ThuePhong;
using MHPQ.Services.DichVu;
using MHPQ.Services.ThanhToan.Model;

namespace MHPQ.Services.ThanhToan
{
    public class ThanhToanAppService : MHPQAppServiceBase, IThanhToanAppService
    {
        //Paypal
        private readonly string _clientId;
        private readonly string _secretKey;
        //Vnpay
        private readonly string _vnpHashsetSecret;
        private readonly string _vnpTmnCode;
        private readonly string _vnpUrl;
        private readonly string _querydr;

        private readonly IHttpContextAccessor _httpContextAccessor;
        //Lượt thuê dịch vụ
        private readonly IRepository<Hires, long> _hireRepository;
        private readonly IRepository<LimitedSpaceServices, long> _limitedSpaceRepository;
        private readonly IRepository<UnlimitedSpaceServices, long> _unlimitedRepository;

        //lượt thuê phòng khách sạn
        private readonly IRepository<BookingRoomHotel, long> _bookingRoomHotelRepo;
        private readonly IRepository<RoomHotel, long> _roomHotelRepository;

        public double TyGiaUSD = 23300;//store in Database
        public ThanhToanAppService(IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Hires, long> hireRepository,
            IRepository<LimitedSpaceServices, long> limitedSpaceRepository,
            IRepository<UnlimitedSpaceServices, long> unlimitedRepository,
            IRepository<BookingRoomHotel, long> bookingRoomHotelRepo,
            IRepository<RoomHotel, long> roomHotelRepository)
        {
            //paypal
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];

            //vnpay
            _vnpHashsetSecret = config["VnpaySettings:VnpHashsetSecret"];
            _vnpTmnCode = config["VnpaySettings:VnpTmnCode"];
            _vnpUrl = config["VnpaySettings:VnpUrl"];
            _querydr = config["VnpaySettings:VnpQueryDr"];


            _httpContextAccessor = httpContextAccessor;
            _hireRepository = hireRepository;
            _limitedSpaceRepository = limitedSpaceRepository;
            _unlimitedRepository = unlimitedRepository;
            _bookingRoomHotelRepo = bookingRoomHotelRepo;
            _roomHotelRepository = roomHotelRepository;
        }

        public List<CartItem> Carts
        {
            get
            {

                var jsonGetString = _httpContextAccessor.HttpContext.Session.Get("GioHang");
                var jsonObject = new List<CartItem>();
                if (jsonGetString != null)
                {
                    jsonObject = JsonSerializer.Deserialize<List<CartItem>>(jsonGetString);

                }

                return jsonObject;
            }
        }
        /// <summary>
        /// Thanh toán dịch vụ
        /// </summary>
        /// <param name="hireDto"></param>
        /// <returns></returns>
        public async Task<object> DichVu(HireServiceDto hireDto)
        {
            try
            {

                var hire = await _hireRepository.GetAsync(hireDto.HireId);
                if (hire.IsPaid == false)
                {
                    //Cập nhật cartItem để gọi api paypal

                    //lấy thông tin dịch vụ
                    dynamic service;
                    //nếu là dịch vụ tính chỗ
                    if (hire.UnlimitedSpaceServiceId == 0)
                    {
                        service = await _limitedSpaceRepository.GetAsync(hire.LimitedSpaceServiceId);

                    }
                    //nếu là dịch vụ ko tính chỗ
                    else if (hire.LimitedSpaceServiceId == 0)
                    {
                        service = await _unlimitedRepository.GetAsync(hire.UnlimitedSpaceServiceId);

                    }
                    else
                    {
                        service = null;
                    }

                    //Cho vào session
                    List<CartItem> carts = new List<CartItem>{
                        new CartItem
                            {
                                DonGia = Convert.ToDouble(service.PriceHours),
                                SoLuong = hire.HireHours,
                            },
                        };


                    var jsonSetString = JsonSerializer.Serialize(carts);
                    _httpContextAccessor.HttpContext.Session.SetString("GioHang", jsonSetString);

                    //Cập nhật db
                    hire.IsPaid = true;
                    await _hireRepository.UpdateAsync(hire);

                    //gọi paypal
                    await PaypalCheckout();

                    return DataResult.ResultSucces(carts, "Cập nhật db thành công");
                }

                return DataResult.ResultSucces(null, "Lỗi sản phẩm thanh toán rồi");
            }
            catch (Exception e)
            {

                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }
        /// <summary>
        /// Thanh toán phòng khách sạn
        /// </summary>
        /// <param name="bookingDto"></param>
        /// <returns></returns>
        public async Task<object> PhongKhachSan(BookingRoomHotelDto bookingDto)
        {
            try
            {
                var bookingsPay = (from bookings in _bookingRoomHotelRepo.GetAll()
                                   where bookings.RoomHotelId == bookingDto.RoomHotelId && bookings.GuestHotelId == bookingDto.GuestHotelId
                                   select bookings).ToList();

                List<CartItem> carts = new List<CartItem>();
                //duyệt các phòng khách thuê
                foreach (var bookPay in bookingsPay)
                {
                    if (bookPay.IsPaid == false)
                    {
                        //lấy thông tin phòng
                        var room = await _roomHotelRepository.GetAsync(bookPay.RoomHotelId??0);
                        var cartItem = new CartItem
                        {
                            DonGia = room.Price,
                            SoLuong = 1,
                        };
                        carts.Add(cartItem);

                        //cập nhật db
                        bookPay.IsPaid = true;
                        await _bookingRoomHotelRepo.UpdateAsync(bookPay);
                    }
                }

                //cho vào session
                var jsonSetString = JsonSerializer.Serialize(carts);
                _httpContextAccessor.HttpContext.Session.SetString("GioHang", jsonSetString);


                //gọi paypal
                await PaypalCheckout();

                return DataResult.ResultSucces(carts, "Cập nhật db thành công");

            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }

        public async Task<object> PaypalCheckout()
        {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            #region Create Paypal Order
            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };
            var total = Math.Round(Carts.Sum(p => p.ThanhTien) / TyGiaUSD, 0);
            foreach (var item in Carts)
            {
                itemList.Items.Add(new Item()
                {
                    Name = item.TenHh,
                    Currency = "USD",
                    Price = Math.Round(item.DonGia / TyGiaUSD, 0).ToString(),
                    Quantity = item.SoLuong.ToString(),
                    Sku = "sku",
                    Tax = "0"
                });
            }
            #endregion

            var paypalOrderId = DateTime.Now.Ticks;
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = total.ToString(),
                            Currency = "USD",
                            Details = new AmountDetails()
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = total.ToString()
                            }

                        },
                        ItemList = itemList,
                        Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"http://localhost:21021/swagger/index.html",
                    ReturnUrl = $"http://localhost:21021/swagger/index.html"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }
                //xóa bỏ trong session
                _httpContextAccessor.HttpContext.Session.Remove("GioHang");

                var data_result = DataResult.ResultSucces(paypalRedirectUrl, "Thanh toán thành công");
                return data_result;

            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }

        public async Task<object> VnpayCheckout()
        {
            try
            {
                //Get Config Info
                string vnp_Url = _vnpUrl; //URL thanh toan cua VNPAY 
                string vnp_TmnCode = _vnpTmnCode; //Ma website
                string vnp_HashSecret = _vnpHashsetSecret; //Chuoi bi mat
                if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
                {
                    return DataResult.ResultSucces("TmnCode hoặc HashSecret rỗng");
                }
                //Get payment input
                OrderInfo order = new OrderInfo();
                //Save order to db
                order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
                order.Amount = 100000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
                order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
                order.OrderDesc = "";
                order.CreatedDate = DateTime.Now;
                string locale = "";
                //Build URL for VNPAY
                VnPayLibrary vnpay = new VnPayLibrary();

                vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
                vnpay.AddRequestData("vnp_BankCode", "NCB");
                vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");

                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                vnpay.AddRequestData("vnp_IpAddr", ipAddress);
                if (!string.IsNullOrEmpty(locale))
                {
                    vnpay.AddRequestData("vnp_Locale", locale);
                }
                else
                {
                    vnpay.AddRequestData("vnp_Locale", "vn");
                }
                vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
                vnpay.AddRequestData("vnp_OrderType", default); //default value: other
                vnpay.AddRequestData("vnp_ReturnUrl", "");
                vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
                                                                              //Add Params of 2.1.0 Version
                vnpay.AddRequestData("vnp_ExpireDate", "");
                //Billing
                vnpay.AddRequestData("vnp_Bill_Mobile", "");
                vnpay.AddRequestData("vnp_Bill_Email", "");
                var fullName = "";
                if (!String.IsNullOrEmpty(fullName))
                {
                    var indexof = fullName.IndexOf(' ');
                    vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
                    vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1, fullName.Length - indexof - 1));
                }
                vnpay.AddRequestData("vnp_Bill_Address", "");
                vnpay.AddRequestData("vnp_Bill_City", "");
                vnpay.AddRequestData("vnp_Bill_Country", "");
                vnpay.AddRequestData("vnp_Bill_State", "");
                // Invoice
                vnpay.AddRequestData("vnp_Inv_Phone", "");
                vnpay.AddRequestData("vnp_Inv_Email", "");
                vnpay.AddRequestData("vnp_Inv_Customer", "");
                vnpay.AddRequestData("vnp_Inv_Address", "");
                vnpay.AddRequestData("vnp_Inv_Company", "");
                vnpay.AddRequestData("vnp_Inv_Taxcode", "");
                vnpay.AddRequestData("vnp_Inv_Type", "");

                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

                return DataResult.ResultSucces(paymentUrl, "Thanh toán Vnpay thành công");

            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                return DataResult.ResultFail(e.Message);
            }
        }

    }

}
