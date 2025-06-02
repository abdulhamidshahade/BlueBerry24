using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class BulkAssignRoleRequest
    {
        public List<int> UserIds { get; set; } = new List<int>();
        public string RoleName { get; set; }
    }
}
