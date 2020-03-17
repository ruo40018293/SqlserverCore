using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core
{
    public class ResultModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResultModel() { Success = true; }
        public ResultModel(string errMsg)
        {
            if (string.IsNullOrWhiteSpace(errMsg))
            {
                Success = true;
            }
            else
            {
                Success = false;
                Message = errMsg;
            }
        }
    }
}
