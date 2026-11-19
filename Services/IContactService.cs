using SlowBurn.Models;

namespace SlowBurn.Services;

/// <summary>
/// Service abstraction for customer contact messages.
/// </summary>
public interface IContactService
{
    Task SendAsync(ContactForm form);
}

