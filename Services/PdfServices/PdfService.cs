using DinkToPdf;
using DinkToPdf.Contracts;

namespace CC_Karriarpartner.Services.PdfServices
{
    public class PdfService
    {
        private readonly IConverter _converter; 

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GenerateDiploma(string html)
        {
            var document = new HtmlToPdfDocument
            {
                // Skapar ny dokument med globalinställningar
                GlobalSettings =
                {
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects =
                {
                    new ObjectSettings
                    {
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            return _converter.Convert(document);
        }
    }
}
