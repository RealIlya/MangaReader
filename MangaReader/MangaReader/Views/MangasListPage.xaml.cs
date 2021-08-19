using MangaReader.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MangaReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MangasListPage : ContentPage
    {
        public MangasListPage()
        {
            InitializeComponent();
            BindingContext = new MangasListViewModel() {Navigation = this.Navigation};
        }
    }
}