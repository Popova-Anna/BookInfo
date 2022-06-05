using System.ComponentModel.DataAnnotations;

namespace BookInfo.Models
{
    public class Author
    {
        public int Id { get; set; }        
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        [Display(Name ="Фамилия")]
        public string Surname { get; set; }       
        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        [Required]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        public List<Book> Books { get; set; }
        public Author()
        {
            Books = new List<Book>();   
        }
    }
}
