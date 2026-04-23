namespace E_Commerce.BLL
{
    public record TokenDto
    (
        string AccessToken,
        int DurationInMinutes,
        string TokenType = "Bearer"
    );
}
