namespace Database;
internal static class ConnectionString
{
    internal static string Get()
    {
        return "Server=tcp:despino-server.database.windows.net,1433;Initial Catalog=flashcardsDB;Persist Security Info=False;User ID=despino-admin;Password=Iv3ry$Tr95n^;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}
