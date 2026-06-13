
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BCrypt;
using Microsoft.AspNetCore.Identity.Data;
public class UserService
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _tokenGenerator;
    public UserService(IUnitOfWork uow, IPasswordHasher hasher, IJwtTokenGenerator tokenGenerator)
    {
        _uow = uow;
        _hasher = hasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<LoginDto> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _uow.Users.GetByUsernameAsync(request.Username);

        if (existingUser != null)
            throw new ApplicationException("This username is already taken!");

        var hashedPassword = _hasher.HashPassword(request.Password);
        var user = User.Create(request.Username, hashedPassword);

        _uow.Users.Add(user);

        var whitelistRequest = WhitelistRequest.Create(user.Id, request.SchoolId, request.FullName,
            request.FacebookUrl, request.MessageToAdmin);

        _uow.WhitelistRequests.Add(whitelistRequest);

        await _uow.SaveChangesAsync();

        var token = _tokenGenerator.GenerateToken(user);



        return new LoginDto
        {
            Token = token
        };
    }

    public async Task<LoginDto> LoginAsync(LoginRequest request)
    {
        var user = await _uow.Users.GetByUsernameAsync(request.Username);
        if (user == null) throw new ApplicationException("User doesn't exist!");

        bool isPasswordSuccess = _hasher.VerifyPassword(request.Password, user.PasswordHash);

        if (!isPasswordSuccess) throw new ApplicationException("Bad password!");

        var token = _tokenGenerator.GenerateToken(user);

        return new LoginDto
        {
            Token = token
        };
    }
}
