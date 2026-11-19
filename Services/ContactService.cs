using Supabase.Postgrest;
using SlowBurn.Models;
using SlowBurn.Models.Records;

namespace SlowBurn.Services;

/// <summary>
/// Persists contact form inquiries to Supabase.
/// </summary>
public class ContactService : IContactService
{
    private readonly Supabase.Client _client;

    public ContactService(Supabase.Client client)
    {
        _client = client;
    }

    public async Task SendAsync(ContactForm form)
    {
        var record = new ContactMessageRecord
        {
            Name = form.Name.Trim(),
            Email = form.Email.Trim(),
            Message = form.Message.Trim()
        };

        var options = new QueryOptions { Returning = QueryOptions.ReturnType.Minimal };
        await _client.From<ContactMessageRecord>().Insert(record, options);
    }
}

