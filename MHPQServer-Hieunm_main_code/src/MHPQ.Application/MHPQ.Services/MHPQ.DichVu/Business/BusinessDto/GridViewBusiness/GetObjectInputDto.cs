using Abp.Application.Services.Dto;
using MHPQ.Common;
using System;


namespace MHPQ.Services.Dto
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location(double lati, double lon)
        {
            Latitude = lati;
            Longitude = lon;
        }
    }


    public class GetObjectInputDto: PagedInputDto
    {

        public long? Id { get; set; }
        public int? FormId { get; set; }
        public int? FormCase { get; set; } // Kiểu get dữ liệu
        public int? Type { get; set; }
        public string Keyword { get; set; }
        public DateTime? FromDay { get; set; }
        public DateTime? ToDay { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }

}
