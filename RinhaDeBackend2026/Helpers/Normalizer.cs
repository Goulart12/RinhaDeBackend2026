namespace RinhaDeBackend2026.Helpers;

public static class Normalizer
{
    public static float Clamp(float value)
        => Math.Clamp(value, 0f, 1f);

    public static float Normalize(float value, float max)
        => Clamp(value / max);
}