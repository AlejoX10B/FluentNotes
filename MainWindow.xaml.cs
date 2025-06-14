using FluentNotes.Services;
using Microsoft.UI.Xaml;

namespace FluentNotes
{

    public sealed partial class MainWindow : Window
    {

        private readonly IDataService _dataService;

        public MainWindow(IDataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;

            LoadTestData();
        }

        private void LoadTestData()
        {
            var notebooks = _dataService.GetNotebooks();
            var notes = _dataService.GetAllNotes();

            System.Diagnostics.Debug.WriteLine($"Notebooks: {notebooks.Count}");
            System.Diagnostics.Debug.WriteLine($"Notas: {notebooks.Count}");
        }
    }
}
