using RinhaDeBackend2026.Models;
using RinhaDeBackend2026.Models.Dataset;
using RinhaDeBackend2026.Models.Vectors;

namespace RinhaDeBackend2026.Services.Search;

public interface IKnnSearchService
{
    SearchResult[] Search(
        FeatureVector query,
        int k = 5);
}