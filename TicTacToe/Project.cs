using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Common;

namespace TicTacToe;
public class Project : IProjectMeta
{
    public string Name { get; set; } = "TicTacToe";

    public BitmapImage Icon
    {
        get
        {

            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Uri uri = new Uri($"pack://application:,,,/{assemblyName};component/Resources/Main.png");

            return new BitmapImage(uri);
        }
    }

    public void Run()
    {
        MainWindow window = new MainWindow();
        window.ShowDialog();
    }
}