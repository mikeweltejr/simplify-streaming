using DynamoDB.DAL.App.Models;

namespace SimplifyStreaming.API.App.Common.Services
{
    public interface ITitleService
    {
        Task<Title?> GetTitle(string titleId);
        Task<Title> Save(Title title);
        Task<bool> Delete(string titleId);
    }
}
