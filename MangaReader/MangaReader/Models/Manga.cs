using System.Collections.Generic;
using MangaReader.ViewModels;

namespace MangaReader.Models
{
    public class Manga : BaseViewModel
    {
        public string Header { get; set; }

        // public string Image { get; set; }
        
        public List<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}