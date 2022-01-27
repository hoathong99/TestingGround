using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("BillMappingTypes")]
    public class BillMappingType : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? Type { get; set; }
        [StringLength(2000)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Property { get; set; }
        [StringLength(2000)]
        public string Mapping { get; set; }
        public int? TenantId { get; set; }
    }
}
