using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTEC5910.Shared.Entities.Dto
{
    //public class ReceiveAuthCodeResponseDto
    //{
    //    public string AccessToken { get; set; }
    //    public string OpenID { get; set; }
    //    public string Message { get; set; }
    //}

    public class GetTokenResponseDto
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public GetTokenResponseContent Content { get; set; }
    }

    public class GetTokenResponseContent
    {
        public string AccessToken { get; set; }
        public string OpenID { get; set; }
    }

    public class CallBackAuthResponseDto : AuthResponseDto
    {
        public string IAMSmartToken { get; set; }
    }
}
