using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

public class PdfHeaderEditor
{
    public void CreatePdfFileWithHeaders(string filePath, string[] headers)
    {
        Document document = new Document();
        try
        {
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();
            document.Add(new Paragraph(" "));


            foreach (string header in headers)
            {
                Paragraph paragraph = new Paragraph(header, FontFactory.GetFont("Arial", 32, Font.BOLD));
                paragraph.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph);
            }

            Console.WriteLine("PDF-файл успешно создан.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка при создании PDF-файла: " + ex.Message);
        }
        finally
        {
            document.Close();
        }
    }
}