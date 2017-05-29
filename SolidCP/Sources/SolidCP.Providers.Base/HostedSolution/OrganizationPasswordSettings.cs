namespace SolidCP.Providers.HostedSolution
{
    public class OrganizationPasswordSettings
    {
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }
        public int EnforcePasswordHistory { get; set; }
        public int MaxPasswordAge { get; set; }

        public bool LockoutSettingsEnabled { get; set; }
        public int AccountLockoutDuration { get; set; }
        public int AccountLockoutThreshold { get; set; }
        public int ResetAccountLockoutCounterAfter { get; set; }

        public bool PasswordComplexityEnabled { get; set; }
        public int UppercaseLettersCount { get; set; }
        public int NumbersCount { get; set; }
        public int SymbolsCount { get; set; }
    }
}