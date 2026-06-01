namespace RinhaDeBackend2026.Models.Dataset;

public class NormalizationSettings
{
    public float MaxAmount { get; init; }

    public float MaxInstallments { get; init; }

    public float AmountVsAvgRatio { get; init; }

    public float MaxMinutes { get; init; }

    public float MaxKm { get; init; }

    public float MaxTxCount24h { get; init; }

    public float MaxMerchantAvgAmount { get; init; }
}