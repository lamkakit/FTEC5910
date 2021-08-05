using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTEC5910.Shared.Entities.Dto
{
    public class GetUserResponseDto
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string UserID { get; set; }
    }
}
