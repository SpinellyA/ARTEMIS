public interface IPasswordHasher
{
    string HashPassword(string plainText);
    bool VerifyPassword(string plainText, string hash);
}
