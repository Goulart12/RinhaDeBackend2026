using RinhaDeBackend2026.Models.Vectors;

namespace RinhaDeBackend2026.Models.Dataset;

public readonly struct ReferenceVector
{
    public readonly float[] Values;

    public readonly byte Label;

    public ReferenceVector(
        float[] values,
        byte label)
    {
        Values = values;
        Label = label;
    }

    public bool IsFraud => Label == 1;
}