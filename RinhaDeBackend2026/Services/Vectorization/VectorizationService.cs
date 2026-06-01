using RinhaDeBackend2026.Helpers;
using RinhaDeBackend2026.Models.Dataset;
using RinhaDeBackend2026.Models.Requests;
using RinhaDeBackend2026.Models.Vectors;

namespace RinhaDeBackend2026.Services.Vectorization;

public sealed class VectorizationService : IVectorizationService
{
    private readonly NormalizationSettings _settings;
    private readonly IReadOnlyDictionary<string, float> _mccRisk;

    public VectorizationService(NormalizationSettings settings, IReadOnlyDictionary<string, float> mccRisk)
    {
        _settings = settings;
        _mccRisk = mccRisk;
    }

    public FeatureVector CreateVector(FraudScoreRequest request)
    {
        return new FeatureVector(
            Amount(request),
            Installments(request),
            AmountVsAverage(request),
            HourOfDay(request),
            DayOfWeek(request),
            MinutesSinceLastTransaction(request),
            KmFromLastTransaction(request),
            KmFromHome(request),
            TransactionCount24h(request),
            IsOnline(request),
            CardPresent(request),
            UnknownMerchant(request),
            MccRisk(request),
            MerchantAverageAmount(request)
        );
    }
    
    private float Amount(FraudScoreRequest request)
    {
        return Normalizer.Normalize(
            (float)request.Transaction.Amount,
            _settings.MaxAmount);
    }
    
    private float Installments(FraudScoreRequest request)
    {
        return Normalizer.Normalize(
            request.Transaction.Installments,
            _settings.MaxInstallments);
    }
    
    private float AmountVsAverage(FraudScoreRequest request)
    {
        if (request.Customer.AvgAmount <= 0)
            return 1f;

        var ratio =
            (float)(request.Transaction.Amount /
                    request.Customer.AvgAmount);

        return MathF.Min(
            ratio / _settings.AmountVsAvgRatio,
            1f);
    }
    
    private static float HourOfDay(FraudScoreRequest request)
    {
        return request.Transaction
            .RequestedAt
            .ToUniversalTime()
            .Hour / 23f;
    }
    
    private static float DayOfWeek(FraudScoreRequest request)
    {
        var day =
            (int)request.Transaction
                .RequestedAt
                .ToUniversalTime()
                .DayOfWeek;

        return ((day + 6) % 7) / 6f;
    }
    
    private float MinutesSinceLastTransaction(
        FraudScoreRequest request)
    {
        if (request.LastTransaction is null)
            return -1f;

        var minutes =
            (float)(
                request.Transaction.RequestedAt -
                request.LastTransaction.RequestedAt
            ).TotalMinutes;

        return Normalizer.Normalize(
            minutes,
            _settings.MaxMinutes);
    }
    
    private float KmFromLastTransaction(
        FraudScoreRequest request)
    {
        if (request.LastTransaction is null)
            return -1f;

        return Normalizer.Normalize(
            (float)request.LastTransaction.KmFromCurrent,
            _settings.MaxKm);
    }
    
    private float KmFromHome(
        FraudScoreRequest request)
    {
        return Normalizer.Normalize(
            (float)request.Terminal.KmFromHome,
            _settings.MaxKm);
    }
    
    private float TransactionCount24h(
        FraudScoreRequest request)
    {
        return Normalizer.Normalize(
            request.Customer.TxCount24h,
            _settings.MaxTxCount24h);
    }
    
    private static float IsOnline(
        FraudScoreRequest request)
    {
        return request.Terminal.IsOnline
            ? 1f
            : 0f;
    }
    
    private static float CardPresent(
        FraudScoreRequest request)
    {
        return request.Terminal.CardPresent
            ? 1f
            : 0f;
    }
    
    private static float UnknownMerchant(
        FraudScoreRequest request)
    {
        return request.Customer
            .KnownMerchants
            .Contains(request.Merchant.Id)
            ? 0f
            : 1f;
    }
    
    private float MccRisk(
        FraudScoreRequest request)
    {
        return _mccRisk.TryGetValue(
            request.Merchant.Mcc,
            out var risk)
            ? risk
            : 0.5f;
    }
    
    private float MerchantAverageAmount(
        FraudScoreRequest request)
    {
        return Normalizer.Normalize(
            (float)request.Merchant.AvgAmount,
            _settings.MaxMerchantAvgAmount);
    }
}