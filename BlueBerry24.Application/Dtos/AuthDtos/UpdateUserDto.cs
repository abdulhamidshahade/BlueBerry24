namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}