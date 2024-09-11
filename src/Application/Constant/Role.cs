using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Constant
{
    public class RoleConst
    {
        public const string ADMIN = "ADMIN";
        public const int ADMIN_ID = 1;
        public const string MANAGER = "MANAGER";
        public const int MANAGER_ID = 2;
        public const string STAFF = "STAFF";
        public const int STAFF_ID = 3;
        public const string CUSTOMER = "CUSTOMER";
        public const int CUSTOMER_ID = 4;
        public const string GUEST = "GUEST";
        public const int GUEST_ID = 5;
        public Dictionary<string, int> RoleId = new()
  {
    { ADMIN, ADMIN_ID },
    { MANAGER, MANAGER_ID },
    { STAFF, STAFF_ID },
    { CUSTOMER, CUSTOMER_ID },
    { GUEST, GUEST_ID }
  };

        public static int GetRoleId(string roleName)
        {
            return new RoleConst().RoleId[roleName];
        }
    }
}