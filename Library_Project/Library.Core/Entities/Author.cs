using Library.Core.Interfaces;

namespace Library.Core.Entities
{
    public class Author : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Surname { get; set; }
        private static int _id;
        public Author(string name, string? surname = null) 
        {
            Name = name;
            Surname = surname;
            Id = ++_id;
        }
        public override string ToString()
        {
            return $"Id: {Id}\tName: {Name}\tSurname: {Surname}";
        }
    }
}
