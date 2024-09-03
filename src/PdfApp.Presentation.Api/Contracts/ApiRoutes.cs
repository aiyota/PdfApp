namespace PdfApp.Presentation.Api.Contracts;

public static class ApiRoutes
{
    public const string Root = "api";
    public const string Base = Root + "/";

    public static class User
    {
        public const string Name = Base + "user";
        public const string Login = "login";
        public const string Logout = "logout";
        public const string GetCurrentUser = "me";
    }

    public static class Pdf
    {
        public const string Name = Base + "pdf";
        public const string Get = "";
        public const string GetFile = "file/{fileName}";
        public const string GetById = "{id}";
        public const string Create = "";
        public const string Upload = "{id}";
        public const string Update = "{id}";
        public const string Delete = "{id}";
        public const string GetTags = "tags";
        public const string SaveProgress = "progress/{pdfId}";
        public const string GetProgresses = "progress/{pdfId}";
    }
}

