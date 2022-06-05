using System.ComponentModel.DataAnnotations;
namespace BookInfo.ViewModels
{
    /// <summary>
    /// Модель для регистрации пользователя
    /// </summary>
    public class RegisterModel
    {
        [Required(ErrorMessage = "Поле Email должно быть заполнено!")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^((([0-9A-Za-z]{1}[-0-9A-z\.]{0,30}[0-9A-Za-z]?)|([0-9А-Яа-я]{1}[-0-9А-я\.]{0,30}[0-9А-Яа-я]?))@([-A-Za-z]{1,}\.){1,}[-A-Za-z]{2,})$",
         ErrorMessage = "Формат email указан не верно!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Пароль должен быть заполнен!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{6,}$",
        ErrorMessage = "Пароль не соответствует требованиям: минимум 6 символов, 1 специальный символ, 1 заглавная латинская буква, 1 строчная латинская буква")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле Email должно быть заполнено!")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        public string ConfirmPassword { get; set; }

    }
}
