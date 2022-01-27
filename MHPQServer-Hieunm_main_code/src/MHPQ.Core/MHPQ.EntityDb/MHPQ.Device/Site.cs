using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("Sites")]
    public class Site: Entity<long>
    {

        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Lable { get; set; }
    }
}
