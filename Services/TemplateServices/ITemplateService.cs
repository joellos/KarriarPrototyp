using CC_Karriarpartner.DTOs;

namespace CC_Karriarpartner.Services.ITemplateServices
{
    public interface ITemplateService
    {
        Task<List<TemplateDto>> GetAllTemplates();
        Task<TemplateDto> GetTemplateById(int id);
        Task<(bool Success, TemplateDto? TemplateDto, List<string> ErrorMessages)> CreateTemplate(TemplateDto templateDto);
        Task<(bool Success, List<string> ErrorMessages)> UpdateTemplate(int id, TemplateDto templateDto);
        Task<(bool Success, List<string> ErrorMessages)> DeleteTemplate(int id);
    }
}