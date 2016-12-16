namespace BL.Utilities.AccountPolicy
{
    /// <summary>
    /// Defines various roles used within authentication
    /// </summary>
    public static class Claims
    {
        public const string Standard = "Standard";
        public const string Premium = "Premium";
        public const string Admin = "Administrator";
        public const string Vips = "Premium, Administrator";
        public const string AuthenticatedUsers = "Standard, Premium, Administrator";
    }
}
