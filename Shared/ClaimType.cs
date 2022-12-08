namespace Shared;

public enum ClaimType
{
    Medical,
    Hospital,
    Dental,
    Prescription
}

public static class ClaimTypeExtensions
{
    public static ClaimType ToFileType(this char c)
    {
        return c switch
        {
            'M' => ClaimType.Medical,
            'H' => ClaimType.Hospital,
            'D' => ClaimType.Dental,
            'P' => ClaimType.Prescription
        };
    }
}