namespace RinhaDeBackend2026.Models.Vectors;

public readonly record struct FeatureVector(
    float Amount,
    float Installments,
    float AmountVsAvg,
    float HourOfDay,
    float DayOfWeek,
    float MinutesSinceLastTx,
    float KmFromLastTx,
    float KmFromHome,
    float TxCount24h,
    float IsOnline,
    float CardPresent,
    float UnknownMerchant,
    float MccRisk,
    float MerchantAvgAmount)
{
    public float[] ToArray()
    {
        return
        [
            Amount,
            Installments,
            AmountVsAvg,
            HourOfDay,
            DayOfWeek,
            MinutesSinceLastTx,
            KmFromLastTx,
            KmFromHome,
            TxCount24h,
            IsOnline,
            CardPresent,
            UnknownMerchant,
            MccRisk,
            MerchantAvgAmount
        ];
    }
}