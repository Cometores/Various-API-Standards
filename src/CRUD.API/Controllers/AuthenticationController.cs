using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CRUD.API.Controllers;

/// <summary>The AuthenticationController responsible for handling user authentication processes.</summary>
[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Represents the data structure for user authentication requests, containing
    /// the username and password required for validating user credentials.
    /// </summary>
    public class AuthenticationRequestBody
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    /// <summary>
    /// Represents a user with associated information for the CityInfo application,
    /// including details such as user ID, username, first and last name, and city.
    /// </summary>
    private class CityInfoUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }

        public CityInfoUser(int userId, string userName, string firstName, string lastName, string city)
        {
            UserId = userId;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            City = city;
        }
    }

    /// <summary>
    /// The AuthenticationController class is responsible for handling authentication-related operations,
    /// such as login, registration, and token management for users. It serves as the entry point for
    /// client applications to interact with authentication services.
    /// </summary>
    public AuthenticationController(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Authenticates a user by validating the provided credentials and issuing a JSON Web Token (JWT)
    /// if the credentials are valid.
    /// </summary>
    /// <param name="authenticationRequestBody">An object containing the username and password for authentication.</param>
    /// <returns>
    /// Returns an HTTP 200 OK response with a signed JSON Web Token (JWT) if authentication is successful.
    /// If authentication fails, an HTTP 401 Unauthorized response is returned.
    /// </returns>
    [HttpPost("authenticate")]
    public ActionResult<string> Authenticate([FromBody] AuthenticationRequestBody authenticationRequestBody)
    {
        // Step 1: validate the username/password
        var user = ValidateUserCredentials(
            authenticationRequestBody.UserName,
            authenticationRequestBody.Password);

        if (user == null)
        {
            return Unauthorized();
        }

        // Step 2: defining Key
        var securityKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForkey"]));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Step 3: adding claims
        var claimsForToken = new List<Claim>
        {
            new("sub", user.UserId.ToString()),
            new("given_name", user.FirstName),
            new("family_name", user.LastName),
            new("city", user.City)
        };

        // Step 4: creating token
        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        var tokenToReturn = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);

        return Ok(tokenToReturn);
    }

    private CityInfoUser ValidateUserCredentials(string? userName, string? password)
    {
        // Dummy user creation instead of using a DB
        return new CityInfoUser(
            1,
            userName ?? "",
            "Max",
            "Mustermann",
            "Antwerp");
    }
}