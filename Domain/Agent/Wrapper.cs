using System.Text.Json.Serialization;

namespace ImaginedWorlds.Domain.Agent;

public record ResponseWrapper(
    [property: JsonPropertyName("candidates")] List<Candidate> Candidates
);

public record Candidate(
    [property: JsonPropertyName("content")] Content Content,
    [property: JsonPropertyName("finishReason")] string FinishReason
);

public record Content(
    [property: JsonPropertyName("parts")] List<Part> Parts
);

public record Part(
    [property: JsonPropertyName("text")] string Text
);