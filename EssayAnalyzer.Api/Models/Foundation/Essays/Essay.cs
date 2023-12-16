using System.Text.Json.Serialization;
using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Foundation.Users;

namespace EssayAnalyzer.Api.Models.Foundation.Essays;

public class Essay
{
    public Guid Id { get; set; }
    public Guid UserId { get; set;}
    public string Title { get; set; }
    public string Content { get; set; }
    
    [JsonIgnore]
    public virtual User? User { get; set; }
    
    [JsonIgnore]
    public virtual Result? Result { get; set; }
}