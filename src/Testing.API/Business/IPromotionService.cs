using Testing.API.DataAccess.Entities;

namespace Testing.API.Business
{
    public interface IPromotionService
    {
        Task<bool> PromoteInternalEmployeeAsync(InternalEmployee employee);
    }
}