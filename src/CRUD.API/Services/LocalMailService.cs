namespace CRUD.API.Services;

/// <inheritdoc />
public class LocalMailService : IMailService
{
    private readonly string _mailTo = string.Empty;
    private readonly string _mailFrom = string.Empty;

    /// <summary>Initializes a new instance of the <see cref="LocalMailService"/> class.</summary>
    /// <param name="configuration">The configuration to retrieve mail settings from.</param>
    public LocalMailService(IConfiguration configuration)
    {
        _mailTo = configuration["mailSettings:mailToAddress"];
        _mailFrom = configuration["mailSettings:mailFromAddress"];
    }

    /// <inheritdoc />
    public void Send(string subject, string message)
    {
        // send mail - output to a console window
        Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " +
                          $"with {nameof(LocalMailService)}.");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
    }
}