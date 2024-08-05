using Library.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class Genre : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        private static int _id;
        public Genre(string name)
        {
            Name = name;
            Id = ++_id;
        }
        public override string ToString()
        {
            return $"Id: {Id}\tName: {Name}";
        }
    }
}
