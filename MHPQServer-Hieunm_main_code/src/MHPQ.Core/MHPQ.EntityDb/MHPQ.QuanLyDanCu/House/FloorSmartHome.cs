
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MHPQ.EntityDb
{
    [Table("FloorSmartHomes")]
    public class FloorSmartHome: FullAuditedEntity<long>
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Number { get; set; }

        public long? SmartHomeId { get; set; }
        [StringLength(2000)]
        public string ImageUrl { get; set; }
       

        public int? NumberRooms { get; set; }

        public int? NumberDevices { get; set; }
    }

}
