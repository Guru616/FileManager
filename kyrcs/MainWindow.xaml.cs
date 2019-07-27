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
using System.Windows.Navigation;
using System.Drawing;
using System.IO;
using System.Diagnostics;
namespace kyrcs
{
    /// Логика взаимодействия для MainWindow.xaml
    public partial class MainWindow : Window
    {

        public string CreateDirName = "";//переменная названия директории
        public DriveInfo[] drives = DriveInfo.GetDrives();//
        public MainWindow()
        {
            InitializeComponent();
            foreach (DriveInfo drive in drives)Combx.Items.Add(drive.Name);// загрузка дисков в лист
            Combx.SelectedIndex = 0;
        }
        
        private void Disk_SelectionChanged(object sender, RoutedEventArgs e)//Выбор диска и вывод списка файлов
        {
            try
            {
                foreach (DriveInfo drive in drives)
                {
                    if (Combx.SelectedItem.ToString() == drive.Name)//Проверка на совпадения строки пути и диска
                    {
                        if (drive.IsReady)
                        {
                            InfoDisk.Text = (
                                "Размер: " + drive.TotalSize / 1024 / 1024 / 1024 + "Гб " +
                                "Свободно: " + drive.TotalFreeSpace / 1024 / 1024 / 1024 + "Гб ");
                        }

                        linkStr.Text = drive.Name;

                        //Обновление листа 
                        TextFile.Items.Clear();
                        DirectoryInfo dir = new DirectoryInfo(drive.Name);
                        DirectoryInfo[] dirs = dir.GetDirectories();
                        FileInfo[] files = dir.GetFiles();

                        foreach (DirectoryInfo item in dirs)
                            TextFile.Items.Add(item);

                        foreach (FileInfo item in files)
                            TextFile.Items.Add(item);

                    }
                    else if (Combx.SelectedItem.ToString() != linkStr.Text)
                    { TextFile.Items.Clear(); }
                }
            }
            catch (Exception e1) { MessageBox.Show(e1.Message + " Либо явлеяется съемным"); }
        }

        private void TextFile_MouseDoubleClick(object sender, MouseButtonEventArgs e)//Переход по мышке
        {
            string OldLink = linkStr.Text;//Запись строки пути на случай если будет отказано в доступе
            try
            {
                
                if (Path.GetExtension(Path.Combine(linkStr.Text, TextFile.SelectedItem.ToString())) == "")//Проверка является ли ли путь директорией
                {
                    linkStr.Text = Path.Combine(linkStr.Text, TextFile.SelectedItem.ToString());//Перезапись пути
                    TextFile.Items.Clear();

                    //Обновление листа 
                    DirectoryInfo dir = new DirectoryInfo(linkStr.Text);
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    FileInfo[] files = dir.GetFiles();

                    foreach (DirectoryInfo item in dirs)
                        TextFile.Items.Add(item);

                    foreach (FileInfo item in files)
                        TextFile.Items.Add(item);
                }
                else { Process.Start(Path.Combine(linkStr.Text, TextFile.SelectedItem.ToString())); }//Открытие файла
            }
            catch (Exception e1)//Блок обработки отказа доступа
            {
                MessageBox.Show(e1.Message);

                try
                {
                    //Удаление части пути по которому отказано в доступе
                    if (linkStr.Text[linkStr.Text.Length - 1] == '\\')//Проверка стоит ли впереди строки слеш 
                    {
                        linkStr.Text = linkStr.Text.Remove(linkStr.Text.Length - 1, 1);
                        while (linkStr.Text[linkStr.Text.Length - 1] != '\\')
                        {
                            linkStr.Text = linkStr.Text.Remove(linkStr.Text.Length - 1, 1);
                        }
                    }
                    else if (linkStr.Text[linkStr.Text.Length - 1] != '\\')//Если слеша нет то удали символы до следующего слеша
                    {
                        while (linkStr.Text[linkStr.Text.Length - 1] != '\\')
                        {
                            linkStr.Text = linkStr.Text.Remove(linkStr.Text.Length - 1, 1);
                        }
                    }
                    //Обновление листа по старому пути
                    TextFile.Items.Clear();
                    DirectoryInfo dir = new DirectoryInfo(OldLink);
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    FileInfo[] files = dir.GetFiles();

                    foreach (DirectoryInfo item in dirs)
                        TextFile.Items.Add(item);

                    foreach (FileInfo item in files)
                        TextFile.Items.Add(item);
                }
                catch (Exception k)
                {
                    MessageBox.Show(k.Message);
                }
            }
        }
        private void Button_Click_Forward(object sender, RoutedEventArgs e)//Кнопка вперед по пути
        {
            try
            { 
                if (linkStr.Text != "")//проверка не пустая ли строка
                {
                    //Обновление листа
                    TextFile.Items.Clear();
                    DirectoryInfo dir = new DirectoryInfo(linkStr.Text);
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    FileInfo[] files = dir.GetFiles();

                    foreach (DirectoryInfo item in dirs)
                        TextFile.Items.Add(item);

                    foreach (FileInfo item in files)
                        TextFile.Items.Add(item);
                }
            else { MessageBox.Show("Такого пути не существует"); }
            }
            catch (Exception e1) { MessageBox.Show(e1.Message); }

}
        private void Button_Click_Back(object sender, RoutedEventArgs e)//Кнопка возврата директорий
        {
            try
            {   //Удаление части пути 
                if (linkStr.Text[linkStr.Text.Length - 1] == '\\')//Проверка стоит ли впереди строки слеш 
                {
                    linkStr.Text = linkStr.Text.Remove(linkStr.Text.Length - 1, 1);
                    while (linkStr.Text[linkStr.Text.Length - 1] != '\\')
                    {
                        linkStr.Text = linkStr.Text.Remove(linkStr.Text.Length - 1, 1);
                    }
                }
                else if (linkStr.Text[linkStr.Text.Length - 1] != '\\')//Если слеша нет то удали символы до следующего слеша
                {
                    while (linkStr.Text[linkStr.Text.Length - 1] != '\\')
                    {
                        linkStr.Text = linkStr.Text.Remove(linkStr.Text.Length - 1, 1);
                    }
                }
                //Обновление листа
                TextFile.Items.Clear();
                DirectoryInfo dir = new DirectoryInfo(linkStr.Text);
                DirectoryInfo[] dirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                foreach (DirectoryInfo item in dirs)
                    TextFile.Items.Add(item);

                foreach (FileInfo item in files)
                    TextFile.Items.Add(item);
            }
            catch (Exception e1) { MessageBox.Show(e1.Message); }
        }


        private void Button_Create_Dir(object sender, RoutedEventArgs e)//Создание директории
        {
            try
            {
                WindowCreateDir window = new WindowCreateDir {Owner = this};//Назначения этого окна родителем
                window.Show();
            }
            catch(Exception e1){MessageBox.Show(e1.Message);}
        }
        private void Button_Create_File(object sender, RoutedEventArgs e)//Создание файла
        {
            try
            {
                WindowCreateFile window = new WindowCreateFile { Owner = this };//Назначения этого окна родителем
                window.Show();
            }
            catch (Exception e1) { MessageBox.Show(e1.Message); }
        }

        private void Button_Delete(object sender, RoutedEventArgs e)//Удаление
        {
            try
            {
                if (Path.GetExtension(Path.Combine(linkStr.Text, TextFile.SelectedItem.ToString())) == "")//Проверка является ли ли путь директорией
                {
                    if (Directory.Exists(Path.Combine(linkStr.Text, TextFile.SelectedItem.ToString())))//Удаление 
                    {
                        Directory.Delete(Path.Combine(linkStr.Text, TextFile.SelectedItem.ToString()), false);
                        Console.WriteLine();
                    }
                    //Обновление листа
                    TextFile.Items.Clear();
                    DirectoryInfo dir = new DirectoryInfo(linkStr.Text);
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    FileInfo[] files = dir.GetFiles();

                    foreach (DirectoryInfo item in dirs)
                        TextFile.Items.Add(item);

                    foreach (FileInfo item in files)
                        TextFile.Items.Add(item);
                }
                else
                {
                    File.Delete(Path.Combine(linkStr.Text, TextFile.SelectedItem.ToString()));//Удаление

                    //Обновление листа
                    TextFile.Items.Clear();
                    DirectoryInfo dir = new DirectoryInfo(linkStr.Text);
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    FileInfo[] files = dir.GetFiles();

                    foreach (DirectoryInfo item in dirs)
                        TextFile.Items.Add(item);

                    foreach (FileInfo item in files)
                        TextFile.Items.Add(item);
                }
            }
            catch(Exception e1) { MessageBox.Show(e1.Message); }
        }
        private void Copy_File(object sender, RoutedEventArgs e)//Копирование
        {
            try
            {
                File.Copy(Path.Combine(linkStr.Text, TextFile.SelectedItem.ToString()), Path.Combine(linkStr.Text, TextFile.SelectedItem.ToString()) + " - Копия.txt");
                //Обновление листа
                TextFile.Items.Clear();
                DirectoryInfo dir = new DirectoryInfo(linkStr.Text);
                DirectoryInfo[] dirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                foreach (DirectoryInfo item in dirs)
                    TextFile.Items.Add(item);

                foreach (FileInfo item in files)
                    TextFile.Items.Add(item);
                
            }
            catch (Exception e1) { MessageBox.Show(e1.Message); }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case Key.Back: Button_Click_Back(sender,e); break;
                    case Key.Enter: Button_Click_Forward(sender,e); break;
                }
            }
            catch (Exception e1) { MessageBox.Show(e1.Message); }
        }
    }
}
