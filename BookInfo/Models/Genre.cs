using System.ComponentModel.DataAnnotations;

namespace BookInfo.Models
{
    public class Genre
    {
        public int Id { get; set; }
        /// <summary>
        /// Название жанра
        /// </summary>
        [Required]
        [Display(Name="Название жанра")]
        public string Name { get; set; }
    }
}
