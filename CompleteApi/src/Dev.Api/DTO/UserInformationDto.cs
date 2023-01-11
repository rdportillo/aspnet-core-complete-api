namespace Dev.Api.DTO
{
    public class UserInformationDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public IEnumerable<ClaimDto> Claims { get; set; }
    }
}
