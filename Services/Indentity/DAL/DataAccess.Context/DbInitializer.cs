namespace DataAccess.Context;

public class DbInitializer
{
    public static void Initialize(AuthContext context)
    {
        context.Database.EnsureCreated();
    }
}