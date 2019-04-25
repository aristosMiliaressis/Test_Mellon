using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClient.DTO
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int Director { get; set; }
        public List<int> Actors { get; set; }
    }
}
