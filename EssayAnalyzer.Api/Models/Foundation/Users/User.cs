using System.Text.Json.Serialization;
using EssayAnalyzer.Api.Models.Foundation.Essays;

namespace EssayAnalyzer.Api.Models.Foundation.Users;

public class User
{
    public Guid Id {get; set; }
    public string FirstName {get; set; }
    public string LastName {get; set; }
    public string EmailAddress {get; set; }
    
    [JsonIgnore]
    public virtual IEnumerable<Essay>? Essay { get; set; }
}