using AutoMapper;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Emenu.Repo.Base
{
    public class EmenuRepository
    {
        #region Properties and constructors

        protected DataContext Context { get; set; }
        public IHttpContextAccessor _httpContextAccessor;
        public readonly IMapper mapper;

        public EmenuRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper Pmapper)
        {
            Context = context;
            _httpContextAccessor = httpContextAccessor;
            mapper= Pmapper;
        }


        public EmenuRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            Context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckEntityExsist<T>(Expression<Func<T, bool>> whereExpr) where T : class
        {
            var x = await Context.Set<T>().AnyAsync(whereExpr);
            return x;
        }
    }
    #endregion
}

