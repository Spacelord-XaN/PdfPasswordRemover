using iText.Kernel.Pdf;
using System.Text;

namespace PdfPasswordRemover;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args != null && args.Length == 1)
        {
            RemovePasswordFromPdf(args[0]);
        }
        else
        {
            Console.WriteLine("Usage pdfPassClean.exe <pathToFile>");
        }
    }

    private static void RemovePasswordFromPdf(string fileName)
    {
        var outputFile = Path.Combine(Path.GetDirectoryName(fileName), $"{Path.GetFileNameWithoutExtension(fileName)}_noPassword.pdf");

        using Stream input = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        var readerProperties = new ReaderProperties();
        EnterPassword(readerProperties);

        using var reader = new PdfReader(input, readerProperties);

        using Stream output = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None);
        var writerProperties = new WriterProperties();
        using var writer = new PdfWriter(output, writerProperties);
        using var sourceDoc = new PdfDocument(reader, writer);
    }

    private static void EnterPassword(ReaderProperties readerProperties)
    {
        Console.WriteLine("Enter password:");

        var password = new StringBuilder();

        ConsoleKeyInfo key = Console.ReadKey(true);
        while (key.Key != ConsoleKey.Enter)
        {
            password.Append(key.KeyChar);
            key = Console.ReadKey(true);
        }
        readerProperties.SetPassword(Encoding.UTF8.GetBytes(password.ToString()));
    }
}
