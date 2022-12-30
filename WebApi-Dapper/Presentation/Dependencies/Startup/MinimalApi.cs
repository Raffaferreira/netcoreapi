using Presentation.MinimalApi;

namespace Presentation.Dependencies.Startup
{
    /// <summary>
    /// Class containing methods registering routing endpoint to minimal apis following list of http procotol.
    /// </summary>
    public static class MinimalApi
    {
        /// <summary>
        /// Registration of various routing of contexts
        /// </summary>
        /// <param name="app"></param>
        public static void RegisterMinimalApis(this WebApplication app)
        {
            app.RegisterMinimalApiDefault();
            app.RegisterMinimalApiCredit();
            app.RegisterMinimalApiTransaction();
            app.RegisterMinimalApiDebit();
        }
    }
}
