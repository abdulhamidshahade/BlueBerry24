namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class BulkAssignRoleRequest
    {
        public List<int> UserIds { get; set; } = new List<int>();
        public string RoleName { get; set; }
    }
}
