namespace CRUD.API.Services;

/// <summary>Defines the contract for a service that sends mail messages.</summary>
public interface IMailService
{
    /// <summary>Sends a mail message with the specified subject and message body.</summary>
    /// <param name="subject">The subject of the mail.</param>
    /// <param name="message">The content of the mail message.</param>
    void Send(string subject, string message);
}