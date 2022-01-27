using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.EntityDb
{
    [Table("RoomHotel")]
    public class RoomHotel : AuditedEntity<long>
    {
        public int Type { get; set; }
        public float Area { get; set; }
        public int Floor { get; set; }
        public bool IsRent { get; set; }
        public long? HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public int Price { get; set; }
        public string RoomHotelName { get; set; }
    }
}
