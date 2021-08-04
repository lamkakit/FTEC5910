using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FTEC5910.Shared.Entities.Models
{
    public class ReceiveEMEInfoRequest
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public Content Content { get; set; }
    }

    #region Content
    public class Content
    {
        public string BusinessID { get; set; }
        public MobileNumber MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public ResidentialAddress ResidentialAddress { get; set; }
        public PostalAddress PostalAddress { get; set; }

    }

    #region MobileNumber
    public class MobileNumber 
    {
        public string CountryCode { get; set; }
        public string SubscriberNumber { get; set; }
    }

    #endregion

    #region ResidentialAddress
    public class ResidentialAddress
    {
        public ChiPremisesAddress ChiPremisesAddress { get; set; }
    }
    #region ChiPremisesAddress
    public class ChiPremisesAddress 
    {
        public string Region { get; set; }
        public ChiDistrict ChiDistrict { get; set; }
        public string BuildingName { get; set; }
        public ChiEstate ChiEstate { get; set; }
        public ChiStreet ChiStreet { get; set; }
        public ChiBlock ChiBlock { get; set; }
        public Chi3dAddress Chi3dAddress { get; set; }

    }
    #region ChiDistrict
    public class ChiDistrict 
    {
        public string DcDistrict { get; set; }

        [JsonPropertyName("Sub-district")]
        public string SubDistrict { get; set; }
    }
    #endregion

    #region ChiEstate
    public class ChiEstate
    {
        public string EstateName { get; set; }
        public ChiPhase ChiPhase { get; set; }
    }
    public class ChiPhase
    {
        public string PhaseName { get; set; }
    }
    #endregion

    #region ChiStreet
    public class ChiStreet
    {
        public string StreetName { get; set; }
        public string BuildingNoFrom { get; set; }
    }
    #endregion 

    #region ChiBlock
    public class ChiBlock
    {
        public string BlockDescriptor { get; set; }
        public string BlockNo { get; set; }
    }
    #endregion

    #region Chi3dAddress
    public class Chi3dAddress
    {
        public ChiFloor ChiFloor { get; set; }
        public ChiUnit ChiUnit { get; set; }
    }

    public class ChiFloor 
    {
        public string FloorNum { get; set; }
    }
    public class ChiUnit
    {
        public string UnitDescriptor { get; set; }
        public string UnitNo { get; set; }
    }
    #endregion

    #endregion
    #endregion

    #region PostalAddress
    public class PostalAddress
    {
        public PostBoxAddress PostBoxAddress { get; set; }
    }
    #region PostBoxAddress
    public class PostBoxAddress {
        public EngPostBox EngPostBox { get; set; }
    }
    public class EngPostBox 
    {
        public int PoBoxNo { get; set; }
        public string PostOffice { get; set; }
        public string PostOfficeRegion { get; set; }
    }
    #endregion
    #endregion

    #endregion

}
