using OrcardCore.Client.Web.Entities;

namespace OrcardCore.Client.Web.Repositories
{
    public interface IPageRepository
    {
        Task<List<Page>> GetPages(); 
    }
}
