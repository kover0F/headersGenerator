using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using DocumentFormat.OpenXml.Bibliography;
using Bold = DocumentFormat.OpenXml.Wordprocessing.Bold;


//using System.Windows.Forms;


namespace headersGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RichTextBox richText;
        TextBox name;
        ComboBox ext;
        string folderPath;

        public MainWindow()
        {
            InitializeComponent();
            richText = (RichTextBox)this.FindName("headers");
            name = (TextBox)this.FindName("filename");
            ext = (ComboBox)this.FindName("extension");

            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderPath = fbd.SelectedPath;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();

            string[] headers = new TextRange(richText.Document.ContentStart, richText.Document.ContentEnd).Text.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
            
            string filePath = "";
            try
            {
                string fileName = name.Text;
                string fileExtension = ext.Text;

                filePath = System.IO.Path.Combine(folderPath, $"{fileName}{fileExtension}");
                Console.WriteLine(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} \n\r Enter all filds correctly!");
                return;
            }
            /**
            try
            {
                // Создаем новый файл по указанному пути
                FileStream fs = File.Create(filePath);
                fs.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }*/

            if(ext.Text == ".docx")
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    // Добавьте необходимые элементы в документ
                    // Например, можно создать параграф и добавить в него текст заголовка
                    MainDocumentPart mainPart = doc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());



                    foreach (string header in headers)
                    {
                        // Создаем элементы для шрифта и размера шрифта
                        RunProperties runProperties = new RunProperties();
                        RunFonts runFonts = new RunFonts() { Ascii = "Arial" }; // Указываем шрифт Arial
                        FontSize fontSize = new FontSize() { Val = "36" }; // Указываем размер шрифта 36

                        // Добавляем шрифт и размер шрифта в элемент RunProperties
                        runProperties.Append(runFonts);
                        runProperties.Append(fontSize);
                        Bold bold = new Bold();
                        runProperties.Append(bold);

                        Paragraph paragraph = body.AppendChild(new Paragraph());
                        ParagraphProperties paragraphProperties = paragraph.AppendChild(new ParagraphProperties());
                        Justification justification = new Justification() { Val = JustificationValues.Center };
                        paragraphProperties.Append(justification);

                        Run run = paragraph.AppendChild(new Run());
                        run.AppendChild(runProperties);
                        run.AppendChild(new Text(header));
                    }

                }

            }
            else if(ext.Text == ".pdf") {
                PdfHeaderEditor creator = new PdfHeaderEditor();
                creator.CreatePdfFileWithHeaders(filePath, headers);

            }



            /*try
            {
                // Создаем новый файл по указанному пути
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    // Записываем каждый заголовок в отдельную строку файла
                    foreach (string header in headers)
                    {
                        writer.WriteLine(header);
                    }
                    writer.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/

        }

    }
}
