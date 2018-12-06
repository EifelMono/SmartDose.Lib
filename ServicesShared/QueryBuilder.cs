using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serialize.Linq.Extensions;
using RowaMore.Extensions;

#if MasterData10000
namespace MasterData10000
#elif Settings10000
namespace Settings10000
#elif MasterData9002
namespace MasterData9002
#else
namespace ConnectedService
#endif
{
    public class QueryBuilder
    {
        public enum QueryRequestOrderByAs
        {
            None = 0,
            Int = 1,
            String = 2,
            Long = 3
        }

        protected static Dictionary<Type, QueryRequestOrderByAs> QueryRequestOrderByAsDictory = new Dictionary<Type, QueryRequestOrderByAs>
        {
            [typeof(int)] = QueryRequestOrderByAs.Int,
            [typeof(string)] = QueryRequestOrderByAs.String,
            [typeof(long)] = QueryRequestOrderByAs.Long,
        };

        public enum QueryRequestResultAs
        {
            None = 0,
            Item = 1,
            List = 2
        }

        public QueryBuilder(ServiceClientBase client)
        {
            Client = client;
        }

        protected ServiceClientBase Client { get; set; }

        protected string WhereAsJson { get; set; } = string.Empty;
        protected string OrderByAsJson { get; set; } = string.Empty;

        protected bool OrderByAsc { get; set; } = true;

        protected QueryRequestOrderByAs OrderByAs { get; set; } = QueryRequestOrderByAs.None;

        protected QueryRequestResultAs ResultAs { get; set; } = QueryRequestResultAs.None;

        protected Type ModelType { get; set; }

        protected int Page { get; set; } = -1;
        protected int PageSize { get; set; } = -1;

        // Use deconstructor while protected properties 
        public (string WhereAsJson,
            string OrderByAsJson,
            bool OrderByAsc,
            QueryRequestOrderByAs OrderByAs,
            QueryRequestResultAs ResultAs,
            Type ModelType,
            int Page,
            int PageSize) GetValues()
            => (WhereAsJson, OrderByAsJson, OrderByAsc, OrderByAs, ResultAs, ModelType, Page, PageSize);
    }

    public class QueryBuilder<TModel> : QueryBuilder where TModel : class
    {
        public QueryBuilder(ServiceClientBase client) : base(client)
        {
            ModelType = typeof(TModel);
        }

        public QueryBuilder<TModel> Where(Expression<Func<TModel, bool>> whereExpression)
        {
            if (whereExpression != null)
                WhereAsJson = whereExpression.ToJson();
            return this;
        }

        protected QueryBuilder<TModel> InternalOrderBy<T>(Expression<Func<TModel, T>> orderByExpression, bool asc)
        {
            OrderByAsJson = orderByExpression.ToJson();
            OrderByAsc = asc;
            OrderByAs = QueryRequestOrderByAs.None;
            if (QueryRequestOrderByAsDictory.ContainsKey(typeof(T)))
                OrderByAs = QueryRequestOrderByAsDictory[typeof(T)];
            else
                throw new NotImplementedException($"type {typeof(T).Name} not implemented");
            return this;
        }
        public QueryBuilder<TModel> OrderBy<T>(Expression<Func<TModel, T>> orderByExpression)
            => InternalOrderBy(orderByExpression, asc: true);

        public QueryBuilder<TModel> OrderByDescending<T>(Expression<Func<TModel, T>> orderByExpression)
            => InternalOrderBy(orderByExpression, asc: false);

        public QueryBuilder<TModel> Paging(int page = -1, int pageSize = -1)
        {
            Page = page;
            PageSize = pageSize;
            return this;
        }

        protected async Task<ServiceResult<TResult>> ExecuteAsync<TResult>() where TResult : class
        {
            var executeServiceResult = await Client.ExecuteQueryBuilderAsync(this).ConfigureAwait(false);
            var returnResult = executeServiceResult.CastByClone<ServiceResult<TResult>>();
            if (executeServiceResult.IsOk)
                returnResult.Data = (executeServiceResult.Data as string).UnZipString().ToObjectFromJson<TResult>();
            return returnResult;
        }

        public ServiceResult<List<TModel>> ToList()
            => ToListAsync().Result;

        public async Task<ServiceResult<List<TModel>>> ToListAsync()
        {
            ResultAs = QueryRequestResultAs.List;
            return await ExecuteAsync<List<TModel>>().ConfigureAwait(false);
        }

        public ServiceResult<TModel> FirstOrDefault(Expression<Func<TModel, bool>> whereExpression = null)
            => FirstOrDefaultAsync(whereExpression).Result;

        public async Task<ServiceResult<TModel>> FirstOrDefaultAsync(Expression<Func<TModel, bool>> whereExpression = null)
        {
            Where(whereExpression);
            ResultAs = QueryRequestResultAs.Item;
            return await ExecuteAsync<TModel>().ConfigureAwait(false);
        }
    }
}
