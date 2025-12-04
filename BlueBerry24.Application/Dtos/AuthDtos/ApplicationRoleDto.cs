namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class ApplicationRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string concurrencyStamp { get; set; }
        public string normalizedName { get; set; }
    }
}
