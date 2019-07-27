using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace kyrcs
{
    /// <summary>
    /// Логика взаимодействия для WindowCreateDir.xaml
    /// </summary>
    public partial class WindowCreateDir : Window
    {
        public WindowCreateDir()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = (MainWindow)Owner;
            if (!Directory.Exists(System.IO.Path.Combine(w.linkStr.Text, NewDir.Text.ToString())))
            {
                Directory.CreateDirectory(System.IO.Path.Combine(w.linkStr.Text, NewDir.Text.ToString()));//Создание директории по пути
                if (Directory.Exists(System.IO.Path.Combine(w.linkStr.Text, NewDir.Text.ToString())))//Проверка создалась ли директория и обновление листа
                {
                    w.TextFile.Items.Clear();
                    DirectoryInfo dir = new DirectoryInfo(w.linkStr.Text);
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    FileInfo[] files = dir.GetFiles();

                    foreach (DirectoryInfo item in dirs)
                        w.TextFile.Items.Add(item);

                    foreach (FileInfo item in files)
                        w.TextFile.Items.Add(item);
                }
            }
            else { MessageBox.Show("Такой папка уже существует"); }
            Hide();
        }
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Button_Click(sender,e); }
            if (e.Key == Key.Escape) { Close(); }
        }
    }
}
