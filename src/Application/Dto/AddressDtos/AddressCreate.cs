using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.AddressDtos
{
    public class AddressCreate
    {
        public string PhysicsAddress { get; set; } = null!;
        public string WardCode { get; set; } = null!;
        public int DistrictId { get; set; }
    }
}