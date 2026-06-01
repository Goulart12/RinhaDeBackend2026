using RinhaDeBackend2026.Models.Responses;
using RinhaDeBackend2026.Models.Vectors;
using RinhaDeBackend2026.Services.Search;

namespace RinhaDeBackend2026.Services.Fraud;

public sealed class FraudScoringService
    : IFraudScoringService
{
    private readonly IKnnSearchService _knn;

    public FraudScoringService(
        IKnnSearchService knn)
    {
        _knn = knn;
    }
    
    public FraudScoreResponse Calculate(
        FeatureVector vector)
    {
        var neighbors =
            _knn.Search(vector);

        var score =
            CalculateFraudScore(neighbors);

        return new FraudScoreResponse
        {
            FraudScore = score,
            Approved = score < 0.5f
        };
    }
    
    private static float CalculateFraudScore(
        SearchResult[] neighbors)
    {
        var fraudCount = 0;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.Vector.Label == 1)
            {
                fraudCount++;
            }
        }

        return fraudCount / (float)neighbors.Length;
    }
}