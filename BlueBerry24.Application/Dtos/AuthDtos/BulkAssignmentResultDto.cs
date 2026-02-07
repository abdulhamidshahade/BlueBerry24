namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class BulkAssignmentResultDto
    {
        public int TotalUsers { get; set; }
        public int SuccessfulAssignments { get; set; }
        public int FailedAssignments { get; set; }
        public List<string> FailedUserIds { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool IsSuccess => FailedAssignments == 0;
    }
}