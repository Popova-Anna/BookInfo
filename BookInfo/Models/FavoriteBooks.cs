using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookInfo.Models
{
    public class FavoriteBooks
    {
        [Key]
        [Column(Order = 1)]
        public int IdBook { get; set; }
        [Key]
        [Column(Order = 2)]
        public int IdUser { get; set; }    
        public List<Book> Books { get; set; }  
        public List<User> Users { get; set; }
        public FavoriteBooks()
        {
            Books = new List<Book>();
            Users = new List<User>();
        }
    }
}
