namespace Core.Models.Smtp;

public class EmailMessage
{
    /// <summary>
    /// Email subject
    /// </summary>
    public string Subject { get; set; } = string.Empty;
    /// <summary>
    /// All content of the email
    /// </summary>
    public string Body { get; set; } = string.Empty;
    /// <summary>
    /// Who the email is sent to
    /// </summary>
    public string To { get; set; } = string.Empty;
}
