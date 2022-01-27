﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;


namespace MHPQ.EntityDb
{
    public  class Items: FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Properties { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        public long? ObjectPartnerId { get; set; }
        public long? Quantity { get; set; }
        public int? Price { get; set; }
        public int? Like { get; set; }
        public int? NumberOrder { get; set; }
        public int? Type { get; set; }
        public int? TypeGoods { get; set; }
        [StringLength(1000)]
        public string Category { get; set; }
        [StringLength(2000)]
        public string QueryKey { get; set; }
        public int? State { get; set; }
        public string StateProperties { get; set; }
        public string PropertyHistories { get; set; }

    }
}
