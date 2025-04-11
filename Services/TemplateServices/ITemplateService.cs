using CC_Karriarpartner.DTOs.TemplateDtos;

namespace CC_Karriarpartner.Services.ITemplateServices
{
    public interface ITemplateService
    {
        Task<List<TemplateDto>> GetAllTemplates();
        Task<CreateTemplateDto> GetTemplateById(int id);
        Task<(bool Success, CreateTemplateDto? TemplateDto, List<string> ErrorMessages)> CreateTemplate(CreateTemplateDto templateDto);
        Task<(bool Success, List<string> ErrorMessages)> UpdateTemplate(int id, TemplateDto templateDto);
        Task<(bool Success, List<string> ErrorMessages)> DeleteTemplate(int id);
    }
}