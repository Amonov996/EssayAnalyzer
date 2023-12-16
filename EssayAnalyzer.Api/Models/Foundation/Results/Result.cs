using System.Text.Json.Serialization;
using EssayAnalyzer.Api.Models.Foundation.Essays;

namespace EssayAnalyzer.Api.Models.Foundation.Results;

public class Result
{
    public Guid Id { get; set; }
    public Guid EssayId { get; set; }
    public int Mark { get; set; }
    public string Feedback { get; set; }

    [JsonIgnore]
    public virtual Essay? Essay { get; set; }
}