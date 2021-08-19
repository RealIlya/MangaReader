using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows.Input;
using Xamarin.Forms;
using MangaReader.Models;
using MangaReader.Views;

namespace MangaReader.ViewModels
{
    public class MangaViewModel : BaseViewModel
    {
        public ICommand DownloadCommand { get; protected set; }

        public INavigation Navigation { get; set; }
        public Manga MangaInfo { get; set; }

        private Chapter _selectedChapter;

        public Chapter SelectedChapter
        {
            get { return _selectedChapter; }
            set
            {
                if (_selectedChapter != value)
                {
                    _selectedChapter = value;

                    var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var filename = _selectedChapter.Header;
                    var fullpath = Path.Combine(folderPath, filename);

                    if (Directory.Exists(fullpath))
                    {
                        List<ImageChapter> result = new List<ImageChapter>();
                        var images = Directory.GetFiles(fullpath);

                        foreach (var image in images)
                            result.Add(new ImageChapter() {Image = image});

                        var sortedResult = result.OrderBy(i => i.Image).ToList();
                        Navigation.PushAsync(new ChapterPage()
                            {BindingContext = new ChapterViewModel() {ChapterInfo = _selectedChapter, ImageChapters = sortedResult}});
                    }

                    _selectedChapter = null;
                    OnPropertyChanged("SelectedChapter");
                }
            }
        }

        public MangaViewModel()
        {
            DownloadCommand = new Command((chapter) =>
            {
                var c = chapter as Chapter;
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filename = c.Header;
                var fullpath = Path.Combine(folderPath, filename);
                var zipPath = fullpath + ".zip";

                if (!Directory.Exists(fullpath))
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(c.Url, zipPath);
                    var zipToOpen = new FileStream(zipPath, FileMode.Open);
                    var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update, true);
                    archive.ExtractToDirectory(fullpath);
                    zipToOpen.Close();
                    File.Delete(zipPath);
                    c.IsDownload = true;
                }
            }, (chapter) =>
            {
                var c = chapter as Chapter;

                if (c != null)
                {
                    if (c.IsDownload == false)
                        return true;
                }
                return false;
            });
        }
    }
}