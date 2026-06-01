using RinhaDeBackend2026.Models.Responses;
using RinhaDeBackend2026.Models.Vectors;

namespace RinhaDeBackend2026.Services.Fraud;

public interface IFraudScoringService
{
    FraudScoreResponse Calculate(
        FeatureVector vector);
}