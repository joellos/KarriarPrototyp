using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.DTOs.TemplateDtos
{
    public class CreateTemplateDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Titel är obligatoriskt")]
        [MaxLength(100, ErrorMessage = "Titel får inte överstiga 100 tecken")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Beskrivning är obligatoriskt")]
        [MaxLength(1000, ErrorMessage = "Beskrivning får inte överstiga 1000 tecken")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Kategori är obligatoriskt")]
        [MaxLength(50, ErrorMessage = "Kategori får inte överstiga 50 tecken")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Nivå är obligatoriskt")]
        [MaxLength(50, ErrorMessage = "Nivå får inte överstiga 50 tecken")]
        public string Level { get; set; }

        [Required(ErrorMessage = "Pris är obligatoriskt")]
        [Range(0, 9999.99, ErrorMessage = "Pris måste vara mellan 0 och 9999.99")]
        public decimal Price { get; set; }

        public bool Active { get; set; } = true;

        [Required(ErrorMessage = "PDF-URL är obligatoriskt")]
        [Url(ErrorMessage = "Ogiltig URL-format")]
        public string PdfUrl { get; set; }
    }
}