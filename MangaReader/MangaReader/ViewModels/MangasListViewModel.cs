using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Xamarin.Forms;
using MangaReader.Models;
using MangaReader.Views;

//TODO доделать интерфейс, сделать синхронизацию скачанных глав с json-файлом (возможно сделать БД), добавить нормальный парсер
namespace MangaReader.ViewModels
{
    public class MangasListViewModel : BaseViewModel
    {
        public BindingList<Manga> Mangas { get; set; }

        private Manga _selectedManga;
        private string _folderPath;
        private string _filename;
        private string _fullPath;

        public INavigation Navigation { get; set; }

        public Manga SelectedManga
        {
            get { return _selectedManga; }
            set
            {
                if (_selectedManga != value)
                {
                    _selectedManga = value;

                    Navigation.PushAsync(new MangaPage()
                        {BindingContext = new MangaViewModel() {MangaInfo = _selectedManga, Navigation = this.Navigation}});

                    _selectedManga = null;
                    OnPropertyChanged("SelectedManga");
                }
            }
        }

        public MangasListViewModel()
        {
            _folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _filename = "MangasData.json";
            _fullPath = Path.Combine(_folderPath, _filename);

            // var chapters = Parser();
            //
            // Mangas = new BindingList<Manga>() { new Manga() { Header = "Dr.Stone", Chapters = chapters } };
            //
            // File.WriteAllText(_fullPath, JsonConvert.SerializeObject(Mangas));

            Mangas = File.Exists(_fullPath)
                ? JsonConvert.DeserializeObject<BindingList<Manga>>(File.ReadAllText(_fullPath))
                : new BindingList<Manga>();
            Mangas.ListChanged += (s, e) => { File.WriteAllText(_fullPath, JsonConvert.SerializeObject(Mangas)); };
        }

        public void MangasOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            File.WriteAllText(_fullPath, JsonConvert.SerializeObject(Mangas));
        }

        private List<Chapter> Parser()
        {
            var list = new List<Chapter>();

            var html = @"https://mangabook.org/manga/dr-stone";
            var web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var names =
                htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'chapters-elem-download')]/following-sibling::h5/child::a");
            var hrefs = htmlDoc.DocumentNode.SelectNodes("//a[contains(@class, 'btn-filt2')]");


            for (int i = 0; i < names.Count; i++)
            {
                list.Add(new Chapter() {Header = names[i].InnerText, Url = hrefs[i].GetAttributeValue("href", null), IsDownload = false});
            }

            return list;
        }
    }
}