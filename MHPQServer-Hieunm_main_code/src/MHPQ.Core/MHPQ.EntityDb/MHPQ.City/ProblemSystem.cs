using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("ProblemSystems")]
    public class ProblemSystem : FullAuditedEntity<long>
    {
        [StringLength(2000)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Address { get; set; }
        public string Description { get; set; }
        public long? GiverId { get; set; }
        public long? PerformerId { get; set; }
        public int? TypeProblem { get; set; }
        public int? State { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeFinish { get; set; }
    }

}
