using RinhaDeBackend2026.Models.Requests;
using RinhaDeBackend2026.Models.Vectors;

namespace RinhaDeBackend2026.Services.Vectorization;

public interface IVectorizationService
{
    FeatureVector CreateVector(FraudScoreRequest request);
}