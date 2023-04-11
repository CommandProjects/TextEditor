using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.IO;
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
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace TextEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            editor.Document = new FlowDocument();
        }
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|PDF (*.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
            {
                string filename = dialog.FileName;
                string extension = Path.GetExtension(filename).ToLower();
                if (extension == ".txt")
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        editor.Document = new FlowDocument(new Paragraph(new Run(reader.ReadToEnd())));
                    }
                }
                else
                {
                    TextRange range = new TextRange(editor.Document.ContentStart, editor.Document.ContentEnd);
                    using (FileStream stream = new FileStream(filename, FileMode.Open))
                    {
                        if (extension == ".rtf")
                        {
                            range.Load(stream, DataFormats.Rtf);
                        }
                        else if (extension == ".pdf")
                        {
                            range.Load(stream, DataFormats.XamlPackage);
                        }
                    }
                }
            }
        }
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|PDF (*.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
            {
                string filename = dialog.FileName;
                string extension = Path.GetExtension(filename).ToLower();
                TextRange range = new TextRange(editor.Document.ContentStart, editor.Document.ContentEnd);
                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    if (extension == ".txt")
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(range.Text);
                        }
                    }
                    else
                    {
                        range.Save(stream, extension == ".rtf" ? DataFormats.Rtf : DataFormats.XamlPackage);
                    }
                }
            }
        }
    }
}
