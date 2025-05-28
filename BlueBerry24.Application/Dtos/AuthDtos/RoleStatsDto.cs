using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class RoleStatsDto
    {
        public int TotalRoles { get; set; }
        public int TotalUsers { get; set; }
        public int UsersWithRoles { get; set; }
        public int UsersWithoutRoles { get; set; }
    }
}
