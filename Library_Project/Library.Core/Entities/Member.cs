using Library.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class Member : IEntity<int>
    {
        public int Id { get; set; }
        public string Pin { get; set; }
        public string Name { get; set; } = null!;
        public string? Surname { get; set; }
        public bool bookRented { get; set; }
        private static int _id;
        public Member(string name, string pin ,string? surname=null)
        {
            Name = name;
            Pin = pin;
            Surname = surname;
            Id = ++_id;
        }
        public override string ToString()
        {
            return $"Id: {Id}\tPIN: {Pin}\tName: {Name}\tSurname: {Surname}";
        }
    }
}
