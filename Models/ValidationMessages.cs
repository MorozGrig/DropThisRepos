namespace DropThisSite.Models
{
    public static class ValidationMessages
    {
        public const string Required = "Поле обязательно для заполнения";
        public const string InvalidText = "Допустимы только буквы, цифры и стандартные символы";
        public const string InvalidLogin = "Логин может содержать только латинские буквы, цифры и символы ._-";
        public const string InvalidPassword = "Пароль должен содержать от 6 до 64 символов без пробелов";
        public const string InvalidPhone = "Введите корректный номер телефона";
        public const string InvalidEmail = "Введите корректный адрес электронной почты";
        public const string InvalidAddress = "Введите корректный адрес доставки";
        public const string InvalidRange = "Некорректный ввод данных";
        public const string InvalidPrice = "Цена должна быть больше нуля";
        public const string InvalidQuantity = "Количество должно быть больше нуля";
        public const string InvalidLength = "Превышена допустимая длина поля";
    }
}
