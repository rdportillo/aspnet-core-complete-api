namespace Dev.Api.DTO
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }

        public double ExpiresIn { get; set; }

        public UserInformationDto UserInformation { get; set; }
    }
}
