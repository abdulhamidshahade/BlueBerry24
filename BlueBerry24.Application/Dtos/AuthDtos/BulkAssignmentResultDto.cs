using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class BulkAssignmentResultDto
    {
        public int TotalUsers { get; set; }
        public int SuccessfulAssignments { get; set; }
        public int FailedAssignments { get; set; }
        public List<string> FailedUserIds { get; set; } = new List<string>();
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public bool IsSuccess => FailedAssignments == 0;
    }
}