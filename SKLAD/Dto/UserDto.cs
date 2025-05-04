namespace SKLAD.Dto
{
    public record UserRegisterDto(string Email, string Password, string FirstName, string LastName);
    public record UserLoginDto(string Email, string Password);
}
