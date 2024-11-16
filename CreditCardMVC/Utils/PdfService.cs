using DinkToPdf;
using DinkToPdf.Contracts;

namespace CreditCardMVC.Utils
{
    public class PdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdf(string htmlContent)
        {
            var pdfDoc = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                }
            };

            // Agregar los ObjectSettings a la lista existente
            pdfDoc.Objects.Add(new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
            });

            return _converter.Convert(pdfDoc);
        }
    }
}
