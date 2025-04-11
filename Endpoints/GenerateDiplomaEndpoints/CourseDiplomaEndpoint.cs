namespace CC_Karriarpartner.Endpoints.GenerateDiplomaEndpoints
{
    public class CourseDiplomaEndpoint
    {
        public static void RegisterCourseDiplomaEndpoint(WebApplication app)
        {
            app.MapGet("/api/diploma/{userId:int}/", () =>
            {

            });
        }
    }
}
