using System.Threading;
using System.Windows;

namespace FilesSorterRenamer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private App()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("it-IT");
        }
    }
}
