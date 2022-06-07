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
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
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
        public string? Description { get; set; }
        /// <summary>
        /// ID Автор
        /// </summary>
        [Display(Name = "Автор")]
        public int? AuthorId { get; set; }
        /// <summary>
        /// Автор
        /// </summary>
        public Author? Author { get; set; }  
        public Book()
        {
            //Author = new Author();
            Title = " ";
        }
    }
}
