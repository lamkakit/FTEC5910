using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTEC5910.Shared.Enums
{
    public enum AddressType
    {
        Residential,
        [Description("Residential And Correspondence")]
        ResidentialAndCorrespondence,
        [Description("Permanent And Correspondence")]
        PermanentAndCorrespondence,
        Permanent
    }
}
