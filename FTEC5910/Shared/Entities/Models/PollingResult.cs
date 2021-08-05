using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTEC5910.Shared.Entities.Models
{
    public class PollingResult
    {
        public Guid RequestID { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }  
        public string Message { get; set; }
    }

    public class EMeMessage 
    {
        public string HomeAddress { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string PostalAddress { get; set; }
    }


    public class LoginMessage 
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public string IAMSmartToken { get; set; }
    }
}
