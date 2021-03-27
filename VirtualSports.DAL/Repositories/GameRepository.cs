using VirtualSports.DAL.Contexts;
using VirtualSports.DAL.Entities;

namespace VirtualSports.DAL.Repositories
{
    class GameRepository : Repository<Game>
    {
        public GameRepository(DatabaseManagerContext dbContext) : base(dbContext)
        {
        }
    }
}
