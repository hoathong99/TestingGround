using Abp.Application.Services.Dto;
using MHPQ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services
{
    public class GetItemInputDto: PagedInputDto
    {

        public long? Id { get; set; }
        public long? ObjectPartnerId { get; set; }
        public int? FormId { get; set; }
        public int? FormCase { get; set; } // Kiểu get dữ liệu
        public int? Type { get; set; }
        public int? TypeGoods { get; set; }
        public string Keyword { get; set; }
        public DateTime? FromDay { get; set; }
        public DateTime? ToDay { get; set; }

        public bool IsLoadmore { get; set; }

        public string PageSession { get; set; }
        public string Timelife { get; set; }
    }
}
