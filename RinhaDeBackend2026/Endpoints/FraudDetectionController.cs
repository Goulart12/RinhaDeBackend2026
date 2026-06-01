using RinhaDeBackend2026.Models;
using RinhaDeBackend2026.Models.Requests;
using RinhaDeBackend2026.Services;
using RinhaDeBackend2026.Services.Fraud;
using RinhaDeBackend2026.Services.Vectorization;

namespace RinhaDeBackend2026.Endpoints;

public static class FraudDetectionController
{
    public static IEndpointRouteBuilder MapFraudEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet("/ready", Ready);

        app.MapPost(
            "/fraud-score",
            (
                FraudScoreRequest request,
                IVectorizationService vectorization,
                IFraudScoringService scoring
            ) =>
            {
                var vector =
                    vectorization.CreateVector(request);

                var result =
                    scoring.Calculate(vector);

                return Results.Ok(result);
            });

        return app;
    }

    private static IResult Ready()
    {
        return Results.Ok();
    }

}