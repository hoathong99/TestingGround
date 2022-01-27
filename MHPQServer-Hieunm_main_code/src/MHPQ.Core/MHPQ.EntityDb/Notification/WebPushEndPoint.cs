
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHPQ.EntityDb
{
    [Table("AppEndpoints")]
    public class WebPushEndPoint : AuditedEntity<long>
    {
        public string Endpoint { get; set; }
        public string P256DH { get; set; }
        public string Auth { get; set; }
    }
}
