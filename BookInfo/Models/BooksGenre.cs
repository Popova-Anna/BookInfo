﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookInfo.Models
{
    public class BooksGenre
    {
        [Key]
        [Column(Order = 1)]
        public int IdBook { get; set; }
        [Key]
        [Column(Order = 2)]
        public int IdGenre { get; set; }
        public List<Book> Books { get; set; }
        public List<Genre> Genres { get; set; }
        public BooksGenre()
        {
            Books = new List<Book>();
            Genres = new List<Genre>();
        }
    }
}
