using System.ComponentModel.DataAnnotations;

namespace BookInfo.Models
{
    public class Book
    {
        public int Id { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        [Display(Name = "Дата создания")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Количество страниц
        /// </summary>
        [Display(Name = "Количество страниц")]
        public uint Pages { get; set; }
        /// <summary>
        /// Обложка
        /// </summary>
        [Display(Name = "Обложка")]
        public string? Cover { get; set; }
        /// <summary>
        /// Красткое описание
        /// </summary>
        [Display(Name = "Красткое описание")]
        public string Description { get; set; }
        /// <summary>
        /// ID Автор
        /// </summary>
        [Required]
        [Display(Name = "Автор")]
        public int IdAuthor { get; set; }
        /// <summary>
        /// Автор
        /// </summary>
        public Author Author { get; set; }  
    }
}
