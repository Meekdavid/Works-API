using Works_API.Data;
using Works_API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Works_API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingwalkdifficulty = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            if (existingwalkdifficulty == null)
            {
                return null;
            }
            nZWalksDbContext.WalkDifficulty.Remove(existingwalkdifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return existingwalkdifficulty;

        }

        //public async Task<Region> DeleteAsync(Guid id)
        //{
        //    var region = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        //    if (region == null)
        //    {
        //        return null;
        //    }

        //    //Delete the region
        //    nZWalksDbContext.Regions.Remove(region);
        //    await nZWalksDbContext.SaveChangesAsync();
        //    return region;
        //}


        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if (existingWalkDifficulty == null)
            {
                return null;
            }

            existingWalkDifficulty.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficulty;
        }
    }
}
