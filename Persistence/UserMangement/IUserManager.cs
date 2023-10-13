namespace Persistance.UserMangement
{
    public interface IUserManager
    {
        string GetUserId ();
        string GetUserId(string token);
        string GetEmail();
        string GetRole();
        string GetRole(string token);
        bool CheckAuthorize ();
    }
}
