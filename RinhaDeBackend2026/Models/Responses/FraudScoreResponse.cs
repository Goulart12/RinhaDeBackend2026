namespace RinhaDeBackend2026.Models.Responses;

public sealed class FraudScoreResponse
{
    public bool Approved { get; init; }

    public float FraudScore { get; init; }
}