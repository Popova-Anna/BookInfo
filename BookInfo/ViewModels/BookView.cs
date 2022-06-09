using BookInfo.Models;
namespace BookInfo.ViewModels
{
    public class BookView
    {
        /// <summary>
        /// Идентификатор книги
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Обложка
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// ФИО автора
        /// </summary>
        public string Author { get; set; }  
        /// <summary>
        /// Список жанров
        /// </summary>
        public List<Genre> Genres { get; set; }
    }
}
