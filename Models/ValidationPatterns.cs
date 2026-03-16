namespace DropThisSite.Models
{
    public static class ValidationPatterns
    {
        public const int ShortTextMaxLength = 50;
        public const int MediumTextMaxLength = 100;
        public const int LongTextMaxLength = 250;
        public const string SafeTextPattern = @"^[a-zA-Zа-яА-ЯёЁ0-9\s\-\.]+$";
        public const string LoginPattern = @"^[a-zA-Z0-9._-]+$";
        public const string PasswordPattern = @"^(?=.{6,64}$)[^\s]+$";
        public const string PhonePattern = @"^\+?[0-9\-\s\(\)]{7,20}$";
    }
}
