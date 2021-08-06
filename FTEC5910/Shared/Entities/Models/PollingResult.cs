using FTEC5910.Shared.Enums;
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
        public string Room { get; set; }
        public string Flat { get; set; }
        public string Floor { get; set; }
        public string Block { get; set; }
        public string Building { get; set; }
        public string Estate { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public DistrictLarge? DistrictLarge { get; set; } = null;
    }


    public class LoginMessage 
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public string IAMSmartToken { get; set; }
    }
}
