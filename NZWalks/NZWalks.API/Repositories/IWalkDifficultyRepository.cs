using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkDifficultyRepository
    {
        Task<IEnumerable<WalkDifficulty>> GetAllAsync();

        Task<WalkDifficulty> GetAsync(Guid Id);

        Task<WalkDifficulty> AddAsync(WalkDifficulty WalkDifficulty);

        Task<WalkDifficulty> DeleteAsync(Guid id);

        Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty);
    }
}
