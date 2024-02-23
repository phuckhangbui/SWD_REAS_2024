using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class RuleRepository : BaseRepository<Rule>, IRuleRepository
    {
        public RuleRepository(DataContext context) : base(context)
        {
        }
    }
}
