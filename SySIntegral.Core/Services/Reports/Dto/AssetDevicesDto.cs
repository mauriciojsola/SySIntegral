using System.Collections.Generic;

namespace SySIntegral.Core.Services.Reports.Dto
{
    public class AssetDevicesDto
    {
        public AssetDevicesDto()
        {
            Devices = new List<DeviceDto>();
        }
        public string AssetName { get; set; }
        public IList<DeviceDto> Devices { get; set; }
    }
}