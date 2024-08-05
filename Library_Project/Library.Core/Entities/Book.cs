using Library.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class Book : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int totalCount { get; set; }
        public int availableCount { get; set; }
        
        public DateTime publicationDate { get; set; }
        public List<Genre> Genres { get; set;}
        public List<Author> Authors { get; set; }
        public Book(string title, int count, DateTime publicationdate)
        {
            Title = title;
            totalCount = count;
            availableCount = count;
            publicationDate = publicationdate;
            Genres = new List<Genre>();
            Authors = new List<Author>();
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return $"Id: {Id}\tTitle: {Title}\tCount:{totalCount}\tPublicationDate:{publicationDate.ToString("dd.MM.yyyy")}";
        }

    }
}
