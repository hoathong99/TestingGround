using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.EntityDb
{
    [Table("GuestService")]
    public class GuestService : Entity<long>
    {
        [StringLength(256)]
        public string FullName { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(256)]
        public string PhoneNumber { get; set; }
        [StringLength(256)]
        public string IdentityNumber { get; set; }
    }
}
