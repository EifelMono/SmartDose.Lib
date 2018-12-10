using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serialize.Linq.Extensions;
using RowaMore.Extensions;

namespace SmartDose.WcfLib
{
    public class ReadBuilder : ModelBuilder
    {
        public enum ReadRequestOrderByAs
        {
            None = 0,
            Int = 1,
            String = 2,
            Long = 3
        }

        protected static Dictionary<Type, ReadRequestOrderByAs> ReadRequestOrderByAsDictory = new Dictionary<Type, ReadRequestOrderByAs>
        {
            [typeof(int)] = ReadRequestOrderByAs.Int,
            [typeof(string)] = ReadRequestOrderByAs.String,
            [typeof(long)] = ReadRequestOrderByAs.Long,
        };

        public enum ReadRequestResultAs
        {
            None = 0,
            Item = 1,
            List = 2
        }

        public ReadBuilder(IServiceClientModel client) : base(client)
        {
        }

        protected bool TableOnlyFlag { get; set; } = false;

        protected string WhereAsJson { get; set; } = string.Empty;
        protected string OrderByAsJson { get; set; } = string.Empty;

        protected bool OrderByAsc { get; set; } = true;
        protected ReadRequestOrderByAs OrderByAs { get; set; } = ReadRequestOrderByAs.None;

        protected ReadRequestResultAs ResultAs { get; set; } = ReadRequestResultAs.None;

        protected int Page { get; set; } = -1;
        protected int PageSize { get; set; } = -1;

        // Use deconstructor while protected properties 
        public (Type ModelType,
            bool DebugInfoFlag,
            bool TableOnlyFlag,
            string WhereAsJson,
            string OrderByAsJson,
            bool OrderByAsc,
            ReadRequestOrderByAs OrderByAs,
            ReadRequestResultAs ResultAs,
            int Page,
            int PageSize
            ) GetValues()
            => (ModelType, DebugInfoFlag, TableOnlyFlag, WhereAsJson, OrderByAsJson, OrderByAsc, OrderByAs, ResultAs, Page, PageSize);
    }

    public class ReadBuilder<TModel> : ReadBuilder where TModel : class
    {
        public ReadBuilder(IServiceClientModel client) : base(client)
        {
            ModelType = typeof(TModel);
        }

        public ReadBuilder<TModel> Where(Expression<Func<TModel, bool>> whereExpression)
        {
            if (whereExpression != null)
                WhereAsJson = whereExpression.ToJson();
            return this;
        }

        protected ReadBuilder<TModel> InternalOrderBy<T>(Expression<Func<TModel, T>> orderByExpression, bool asc)
        {
            OrderByAsJson = orderByExpression.ToJson();
            OrderByAsc = asc;
            OrderByAs = ReadRequestOrderByAs.None;
            if (ReadRequestOrderByAsDictory.ContainsKey(typeof(T)))
                OrderByAs = ReadRequestOrderByAsDictory[typeof(T)];
            else
                throw new NotImplementedException($"type {typeof(T).Name} not implemented");
            return this;
        }
        public ReadBuilder<TModel> OrderBy<T>(Expression<Func<TModel, T>> orderByExpression)
            => InternalOrderBy(orderByExpression, asc: true);

        public ReadBuilder<TModel> OrderByDescending<T>(Expression<Func<TModel, T>> orderByExpression)
            => InternalOrderBy(orderByExpression, asc: false);

        public ReadBuilder<TModel> Paging(int page = -1, int pageSize = -1)
        {
            Page = page;
            PageSize = pageSize;
            return this;
        }

        public ReadBuilder<TModel> UseTableOnly(bool tableOnlyFlag = true)
        {
            TableOnlyFlag = tableOnlyFlag;
            return this;
        }

        public ReadBuilder<TModel> UseDebugInfo(bool debugInfoFlag = true)
        {
            DebugInfoFlag = debugInfoFlag;
            return this;
        }

        public ReadBuilder<TModel> UseDebugInfoAll(bool debugInfoAllFlag)
        {
            SwitchDebugInfoAll(debugInfoAllFlag);
            return this;
        }

        protected async Task<IServiceResult<TResult>> ExecuteAsync<TResult>() where TResult : class
        {
            var executeServiceResult = await Client.ExecuteModelReadAsync(this).ConfigureAwait(false);
            var returnResult = executeServiceResult;
            if (executeServiceResult.StatusAsInt == 0)
                returnResult.Data = (executeServiceResult.Data as string).UnZipString().ToObjectFromJson<TResult>();
            return returnResult.CastByIClone<ServiceResult<TResult>>() as IServiceResult<TResult>;
        }

        public IServiceResult<List<TModel>> ToList()
            => ToListAsync().Result;

        public async Task<IServiceResult<List<TModel>>> ToListAsync()
        {
            ResultAs = ReadRequestResultAs.List;
            return await ExecuteAsync<List<TModel>>().ConfigureAwait(false);
        }

        public IServiceResult<TModel> FirstOrDefault(Expression<Func<TModel, bool>> whereExpression = null)
            => FirstOrDefaultAsync(whereExpression).Result;

        public async Task<IServiceResult<TModel>> FirstOrDefaultAsync(Expression<Func<TModel, bool>> whereExpression = null)
        {
            Where(whereExpression);
            ResultAs = ReadRequestResultAs.Item;
            return await ExecuteAsync<TModel>().ConfigureAwait(false);
        }
    }
}
