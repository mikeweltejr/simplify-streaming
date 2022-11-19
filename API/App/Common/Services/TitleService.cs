using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories;
using DynamoDB.DAL.App.Repositories.Interfaces;

namespace SimplifyStreaming.API.App.Common.Services
{
    public class TitleService : ITitleService
    {
        private readonly IScopedService<Title> _titleScopedService;
        private readonly ITitleRepository _titleRepository;

        public TitleService(
            IScopedService<Title> titleScopedService,
            ITitleRepository titleRepository)
        {
            _titleScopedService = titleScopedService;
            _titleRepository = titleRepository;
        }

        public async Task<bool> Delete(string titleId)
        {
            var title = await GetTitle(titleId);

            if(title == null) return false;

            await _titleRepository.Delete(title);

            return true;
        }

        public async Task<Title?> GetTitle(string titleId)
        {
            var title = _titleScopedService.Get();

            if(title == null)
                title = await _titleRepository.Get(titleId);

            return title;
        }

        public async Task<Title> Save(Title title)
        {
            return await _titleRepository.Save(title);
        }
    }
}
