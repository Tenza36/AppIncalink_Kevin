using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace AppIncalink.Datos
{
    public class HelloWorldDocument : IDocument
    {
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Header()
                    .Text("Hello World")
                    .FontSize(20)
                    .Bold()
                    .AlignCenter();
                page.Content()
                    .Text("This is a sample PDF generated using QuestPDF.")
                    .FontSize(12)
                    .AlignLeft();
                page.Footer()
                    .AlignCenter() // AlignCenter called on Footer to center the footer content
                    .Text(text =>
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                    });
            });
        }
    }
}
