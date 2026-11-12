using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Технология_WPF
{
    public partial class MainWindow : Window
    {
        private int docCounter = 1;

        public MainWindow()
        {
            InitializeComponent();
            CreateNewTab("Новый документ " + docCounter++, "");
        }

        private void CreateNewTab(string title, string content, string filePath = null)
        {
            TabItem newTab = new TabItem();
            newTab.Header = title;
            newTab.Tag = filePath;

            TextBox memoBox = new TextBox
            {
                Text = content,
                AcceptsReturn = true,
                AcceptsTab = true,
                TextWrapping = TextWrapping.Wrap,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                FontSize = 14,
                Padding = new Thickness(5)
            };

            newTab.Content = memoBox;
            DocumentsTabControl.Items.Add(newTab);
            DocumentsTabControl.SelectedItem = newTab;
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            CreateNewTab("Новый документ " + docCounter++, "");
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);
                string fileName = Path.GetFileName(filePath);

                CreateNewTab(fileName, fileContent, filePath);
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentsTabControl.SelectedItem is TabItem currentTab && currentTab.Content is TextBox memoBox)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.FileName = currentTab.Header.ToString();

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, memoBox.Text);
                    currentTab.Header = Path.GetFileName(saveFileDialog.FileName);
                    currentTab.Tag = saveFileDialog.FileName;
                    MessageBox.Show("Файл успешно сохранен!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Нет открытого документа для сохранения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
