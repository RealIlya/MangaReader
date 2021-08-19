using System.Collections.Generic;
using MangaReader.Models;

namespace MangaReader.ViewModels
{
    public class ChapterViewModel : BaseViewModel
    {
        public List<ImageChapter> ImageChapters { get; set; }

        private int _currentIndexImage;


        public Chapter ChapterInfo { get; set; }

        public int CurrentIndexImage
        {
            get { return _currentIndexImage; }
            set
            {
                if (value < ImageChapters.Count - 1)
                {
                    _currentIndexImage = value;
                    OnPropertyChanged("CurrentIndexImage");
                }
            }
        }

    }
}