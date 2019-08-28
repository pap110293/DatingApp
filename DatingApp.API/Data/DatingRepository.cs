using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class DatingRepository : BaseRepository<User>, IDatingRepository
    {
        public DatingRepository(DataContext context) : base(context){}
    }
}