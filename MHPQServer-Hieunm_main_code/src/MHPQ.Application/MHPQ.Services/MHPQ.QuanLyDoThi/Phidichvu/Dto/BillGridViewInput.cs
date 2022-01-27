using MHPQ.Common;
using System;


namespace MHPQ.Services
{
    public class BillGridViewInput: PagedInputDto
    {
        public long? Id { get; set; }
        public int? FormId { get; set; }
        public int? FormCase { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? Type { get; set; }
        public string Keyword { get; set; }
        public DateTime? FromDay { get; set; }
        public DateTime? ToDay { get; set; }
    }

    public class GetBillViewSettingInput
    {
        public long? Id { get; set; }
        public int? Type { get; set; }
        public string  Keyword { get; set; }
    }
}
