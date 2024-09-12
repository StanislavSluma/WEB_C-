using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253505_Bekarev.Domain.Entities
{
    public class Anime
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category? Category { get; set; }
        public string? CategoryId { get; set; }
        public int SeriesAmount{ get; set; }
        public int SeriesTime { get; set; }
        public int TotalTime { get; set; }
        public string? Image { get; set; }
        public string? Mime { get; set; }
    }
}
