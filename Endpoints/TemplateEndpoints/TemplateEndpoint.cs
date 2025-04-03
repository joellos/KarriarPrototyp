using CC_Karriarpartner.DTOs.TemplateDtos;
using CC_Karriarpartner.Services.ITemplateServices;
using CC_Karriarpartner.Services.TemplateServices;

namespace CC_Karriarpartner.Endpoints.TemplateEndpoints
{
    public static class TemplateEndpoint
    {
        public static void RegisterTemplateEndpoints(WebApplication app)
        {
            var templateGroup = app.MapGroup("/api/templates");

            // GET all templates
            templateGroup.MapGet("/", async (ITemplateService templateService) =>
            {
                try
                {
                    var templates = await templateService.GetAllTemplates();
                    return Results.Ok(templates);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("GetAllTemplates")
            .WithOpenApi();

            // GET template by id
            templateGroup.MapGet("/{id}", async (int id, ITemplateService templateService) =>
            {
                try
                {
                    var template = await templateService.GetTemplateById(id);
                    return Results.Ok(template);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { Message = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("GetTemplateById")
            .WithOpenApi();

            // POST create template
            // POST create template
            templateGroup.MapPost("/", async (CreateTemplateDto templateDto, ITemplateService templateService) =>
            {
                var (success, createdTemplate, errorMessages) = await templateService.CreateTemplate(new CreateTemplateDto
                {
                    Id = templateDto.Id,
                    Title = templateDto.Title,
                    Description = templateDto.Description,
                    Category = templateDto.Category,
                    Level = templateDto.Level,
                    Price = templateDto.Price,
                    Active = templateDto.Active,
                    PdfUrl = templateDto.PdfUrl
                });

                if (!success)
                {
                    return Results.BadRequest(new { Errors = errorMessages });
                }

                return Results.Created($"/api/templates/{createdTemplate.Id}", createdTemplate);
            })
            .WithName("CreateTemplate")
            .WithOpenApi();


            // PUT update template
            templateGroup.MapPut("/{id}", async (int id, TemplateDto templateDto, ITemplateService templateService) =>
            {
                var (success, errorMessages) = await templateService.UpdateTemplate(id, templateDto);

                if (!success)
                {
                    if (errorMessages.Contains("Mallen existerar ej."))
                    {
                        return Results.NotFound(new { Errors = errorMessages });
                    }

                    return Results.BadRequest(new { Errors = errorMessages });
                }

                return Results.NoContent();
            })
            .WithName("UpdateTemplate")
            .WithOpenApi();

            // DELETE template
            templateGroup.MapDelete("/{id}", async (int id, ITemplateService templateService) =>
            {
                var (success, errorMessages) = await templateService.DeleteTemplate(id);

                if (!success)
                {
                    return Results.NotFound(new { Errors = errorMessages });
                }

                return Results.NoContent();
            })
            .WithName("DeleteTemplate")
            .WithOpenApi();
        }
    }
}