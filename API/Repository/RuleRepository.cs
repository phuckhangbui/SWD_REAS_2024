using API.Data;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class RuleRepository : BaseRepository<Rule>, IRuleRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public RuleRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateNewRule(RuleCreateParam ruleCreate)
        {
            var rule = new Rule
            {
                Title = ruleCreate.Title,
                Content = ruleCreate.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
            };
            try
            {
                bool check = await CreateAsync(rule);
                if (check) { return true; }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<PageList<Rule>> GetAllRule()
        {
            PaginationParams paginationParams = new PaginationParams();
            var rule = _context.Rule;
            return await PageList<Rule>.CreateAsync(
                rule.AsNoTracking().ProjectTo<Rule>(_mapper.ConfigurationProvider),
                paginationParams.PageNumber,
                paginationParams.PageSize);
        }

        public async Task<Rule> GetRuleWhenUserSignInAuction()
        {
            return await _context.Rule.Where(x => x.Title == "Auction Contract").Select(x => new Rule
            {
                Title = x.Title,
                Content = x.Content
            }).SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateRuleByContentChange(RuleChangeContentParam ruleChangeContent)
        {
            var query = await _context.Rule.Where(x => x.RuleId == ruleChangeContent.idRule).SingleOrDefaultAsync();
            if (query != null)
            {
                    query.Content = ruleChangeContent.content;
                    query.DateUpdated = ruleChangeContent.DateUpdate;
                    try
                    {
                        bool check = await UpdateAsync(query);
                    if (check) return true;
                    else return false;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
            }
            else
            {
                return false;
            }

        }
    }
}
