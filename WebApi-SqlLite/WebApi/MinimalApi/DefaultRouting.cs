namespace Presentation.MinimalApi
{
    public static class DefaultRouting
    {
        public static void RegisterMinimalApiDefault(this WebApplication app)
        {
            app.MapGet("/hello", () => "Hello World!");
            app.MapGet("/sum", (int? n1, int? n2) => n1 + n2);
        }
    }
}
