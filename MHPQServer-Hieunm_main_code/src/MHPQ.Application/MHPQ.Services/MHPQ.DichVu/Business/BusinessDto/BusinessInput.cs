using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Services.Dto
{
    public class ItemViewSettingInputDto
    {
        public long? Id { get; set; }
        public int? FormId { get; set; }
        public int? FormCase { get; set; } //Điều kiện lọc 1
        public int? FormCase2 { get; set; } //Điều kiện lọc 2
        public int? Type { get; set; }
        public string Keyword { get; set; }
        public DateTime? FromDay { get; set; }
        public DateTime? ToDay { get; set; }

    }

    public class ItemTypeInputDto
    {
        public long? Id { get; set; }
        public int? FormId { get; set; }
        public int? FormCase { get; set; } //Điều kiện lọc 1
        public int? FormCase2 { get; set; } //Điều kiện lọc 2
        public int? Type { get; set; }
        public string Keyword { get; set; }
        public DateTime? FromDay { get; set; }
        public DateTime? ToDay { get; set; }

    }


    public class ItemsInputDto
    {
        public long? Id { get; set; }
        public int? FormId { get; set; }
        public int? FormCase { get; set; } //Điều kiện lọc 1
        public int? FormCase2 { get; set; } //Điều kiện lọc 2
        public int? Type { get; set; }
        public string Keyword { get; set; }
        public DateTime? FromDay { get; set; }
        public DateTime? ToDay { get; set; }

    }

    public class ObjectInputDto
    {
        public long? Id { get; set; }
        public int? FormId { get; set; }
        public int? FormCase { get; set; } //Điều kiện lọc 1
        public int? FormCase2 { get; set; } //Điều kiện lọc 2
        public int? Type { get; set; }
        public string Keyword { get; set; }
        public DateTime? FromDay { get; set; }
        public DateTime? ToDay { get; set; }

    }


    public class GetListRateInput
    {
        public long? Id { get; set; }
        public int? FormId { get; set; }
        public int? FormCase { get; set; }
        public int? FormCase2 { get; set; }
        public int? Type { get; set; }
        public string Keyword { get; set; }
        public DateTime? FromDay { get; set; }
        public DateTime? ToDay { get; set; }
    }
}
