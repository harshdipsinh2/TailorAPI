namespace TailorAPI.Services.Interface
{
    public interface IAccessScopeService
    {
        (string Role, int ShopId, int BranchId) GetUserScope();
        bool IsSuperAdmin();
        bool IsAdmin();
        bool IsManager();
        bool IsTailor();
    }

}
