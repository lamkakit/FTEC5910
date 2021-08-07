using FTEC5910.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTEC5910.Shared.Entities.Models
{
    public class AddressFormModel
    {
        public string FormType { get; set; }
        public string FormID { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public EffectiveDate? EffectiveDate { get; set; } = null;
        public DateTime EffectiveDateFrom { get; set; } = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"),"yyyy-MM-dd",null);
        public AddressType? AddressType { get; set; } = null;
        public string Room { get; set; }
        public string Flat { get; set; }
        public string Floor { get; set; }
        public string Block { get; set; }
        public string Building { get; set; }
        public string Estate { get; set; }        
        public string Street { get; set; }
        public string District { get; set; }
        public DistrictLarge? DistrictLarge { get; set; } = null;
        public string CountryAndPostalCode { get; set; }
        public string ChineseAddress { get; set; }
        public ChineseAddressLarge? ChineseAddressLarge { get; set; } = null;
        public bool OptOutChineseAddress { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string HashCode { get; set; }
        public long Timestamp { get; set; }
        public string Signature { get; set; }
        public string Cert { get; set; }

    }
}
