using CC_Karriarpartner.Data;
using CC_Karriarpartner.DTOs.TemplateDtos;
using CC_Karriarpartner.Models;
using CC_Karriarpartner.Services.ITemplateServices;
using CC_Karriarpartner.Services.ValidationServices;
using Microsoft.EntityFrameworkCore;

namespace CC_Karriarpartner.Services.TemplateServices
{
    public class TemplateService : ITemplateService
    {
        private readonly KarriarPartnerDBContext context;
        private readonly IValidationService validationService;

        public TemplateService(KarriarPartnerDBContext context, IValidationService validationService)
        {
            this.context = context;
            this.validationService = validationService;
        }

        public async Task<List<TemplateDto>> GetAllTemplates()
        {
            var templates = await context.Templates.ToListAsync();

            var templateDtos = templates.Select(template => new TemplateDto
            {
                
                Title = template.Title,
                Description = template.Description,
                Category = template.Category,
                Level = template.Level,
                Price = template.Price,
                Active = template.Active,
                PdfUrl = template.PdfUrl
            }).ToList();

            return templateDtos;
        }

        public async Task<CreateTemplateDto> GetTemplateById(int id)
        {
            var template = await context.Templates.FirstOrDefaultAsync(t => t.TemplateId == id);

            if (template == null)
            {
                throw new KeyNotFoundException("Mallen existerar ej.");
            }

            var templateDto = new CreateTemplateDto
            {
                Id = template.TemplateId,
                Title = template.Title,
                Description = template.Description,
                Category = template.Category,
                Level = template.Level,
                Price = template.Price,
                Active = template.Active,
                PdfUrl = template.PdfUrl
            };

            return templateDto;
        }

        public async Task<(bool Success, CreateTemplateDto? TemplateDto, List<string> ErrorMessages)> CreateTemplate(CreateTemplateDto templateDto)
        {
            // Validate the templateDto
            var (isValid, errorMessages) = validationService.ValidateModel(templateDto);
            if (!isValid)
            {
                return (false, null, errorMessages);
            }

            var template = new Template
            {
                Title = templateDto.Title,
                Description = templateDto.Description,
                Category = templateDto.Category,
                Level = templateDto.Level,
                Price = templateDto.Price,
                Active = templateDto.Active,
                PdfUrl = templateDto.PdfUrl
            };

            context.Templates.Add(template);
            await context.SaveChangesAsync();

            templateDto.Id = template.TemplateId;

            return (true, templateDto, new List<string>());
        }

        public async Task<(bool Success, List<string> ErrorMessages)> UpdateTemplate(int id, TemplateDto templateDto)
        {
            // Validate the templateDto
            var (isValid, errorMessages) = validationService.ValidateModel(templateDto);
            if (!isValid)
            {
                return (false, errorMessages);
            }

            var template = await context.Templates.FirstOrDefaultAsync(t => t.TemplateId == id);

            if (template == null)
            {
                return (false, new List<string> { "Mallen existerar ej." });
            }

            template.Title = templateDto.Title;
            template.Description = templateDto.Description;
            template.Category = templateDto.Category;
            template.Level = templateDto.Level;
            template.Price = templateDto.Price;
            template.Active = templateDto.Active;
            template.PdfUrl = templateDto.PdfUrl;

            try
            {
                await context.SaveChangesAsync();
                return (true, new List<string>());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TemplateExists(id))
                {
                    return (false, new List<string> { "Mallen existerar ej." });
                }
                throw;
            }
        }

        public async Task<(bool Success, List<string> ErrorMessages)> DeleteTemplate(int id)
        {
            var template = await context.Templates.FindAsync(id);

            if (template == null)
            {
                return (false, new List<string> { "Mallen existerar ej." });
            }

            context.Templates.Remove(template);
            await context.SaveChangesAsync();

            return (true, new List<string>());
        }

        private bool TemplateExists(int id)
        {
            return context.Templates.Any(t => t.TemplateId == id);
        }
    }
}