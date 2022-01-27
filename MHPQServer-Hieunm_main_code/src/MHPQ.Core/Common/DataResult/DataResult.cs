using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Common.DataResult
{
   
    public interface IDataResult
    {
        string Message { get; set; }
        string Error { get; set; }
        bool Success { get; set; }
        object Data { get; set; }
    }


    public class DataResult : IDataResult
    {
        public string Message { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; } = true;
        public object Data { get; set; }
        public int? TotalRecords { get; set; }
        public int? Result_Code { get; set; }

        public static DataResult ResultSucces(object data, string message) => new DataResult()
        {
            Data = data,
            Message = message,
            Success = true
            
        };
        public static DataResult ResultSucces(object data, string message, int totalReCords) => new DataResult()
        {
            Data = data,
            Message = message,
            Success = true,
            TotalRecords = totalReCords

        };

        public static DataResult ResultError(string err, string message) => new DataResult()
        {
            Error = err,
            Message = message,
            Success = false
        };

        public static DataResult ResultSucces(string message) => new DataResult()
        {
            Message = message,
            Success = true
        };

        public static DataResult ResultFail(string message) => new DataResult()
        {
            Message = message,
            Success = false
        };

        public static DataResult ResultCode(object data, string message, int result_code) => new DataResult()
        {
            Data = data,
            Message = message,
            Success = (result_code == 200),
            Result_Code = result_code

        };

    }
}
