using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.Model.User
{
    public class ResponseModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class ResonseSuccessModel : ResponseModel
    {
        public Dictionary<string, string> Token { get; set; } = new Dictionary<string, string>();
    }
}
