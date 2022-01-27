using Abp.AutoMapper;
using MHPQ.EntityDb;


namespace MHPQ.Services.Dto
{


    [AutoMap(typeof(ProblemSystem))]
    public class ProblemSystemDto : ProblemSystem
    {
        public string NamePerformer { get; set; }
        public string NameGiver { get; set; }
        public string NameCustomer { get; set; }
    }
}
