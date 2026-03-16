namespace DropThisSite.Models
{
    public static class ValidationMessages
    {
        public const string Required = "Поле обязательно для заполнения";
        public const string InvalidText = "Допустимы только буквы, цифры и стандартные символы";
        public const string InvalidLogin = "Логин может содержать только латинские буквы, цифры и символы ._-";
        public const string InvalidPassword = "Пароль должен содержать не менее 6 символов и не включать пробелы";
        public const string InvalidPhone = "Введите корректный номер телефона в формате +7XXXXXXXXXX или 8XXXXXXXXXX";
        public const string InvalidEmail = "Введите корректный email";
        public const string InvalidAddress = "Выберите корректный адрес на карте";
        public const string InvalidSelection = "Выберите значение из списка";
        public const string InvalidPrice = "Цена должна быть больше нуля";
        public const string InvalidQuantity = "Количество должно быть больше нуля";
        public const string InvalidLength = "Превышена допустимая длина поля";
        public const string PasswordsDoNotMatch = "Пароли не совпадают";
    }
}
