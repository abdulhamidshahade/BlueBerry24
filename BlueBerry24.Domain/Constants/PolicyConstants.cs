using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Constants
{
    public static class PolicyConstants
    {
        public const string SuperAdminOnly = "SuperAdminOnly";
        public const string AdminAndAbove = "AdminAndAbove";
        public const string UserAndAbove = "UserAndAbove";
        public const string AllRoles = "AllRoles";

        public const string CanManageUsers = "CanManageUsers";
        public const string CanManageProducts = "CanManageProducts";
        public const string CanManageOrders = "CanManageOrders";
        public const string CanManageCoupons = "CanManageCoupons";
        public const string CanViewReports = "CanViewReports";
        public const string CanManageShops = "CanManageShops";
        public const string CanManageInventory = "CanManageInventory";
    }
}
