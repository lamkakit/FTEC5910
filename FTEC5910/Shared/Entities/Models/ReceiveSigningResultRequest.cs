using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTEC5910.Shared.Entities.Models
{
    public class ReceiveSigningResultRequest
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public SigningContent Content { get; set; }
    }

    public class SigningContent 
    {
        public string BusinessID { get; set; }
        public string State { get; set; }
        public string HashCode { get; set; }
        public long Timestamp { get; set; }
        public string Signature { get; set; }
        public string Cert { get; set; }
    }
}
