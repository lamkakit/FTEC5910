using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTEC5910.Shared.Entities.Dto
{
    public class PollResponseDto
    {
        public Guid RequestID{ get; set; }
        public string Status { get; set; }
    }
}
