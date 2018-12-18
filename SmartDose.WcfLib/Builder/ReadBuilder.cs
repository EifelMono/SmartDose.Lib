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
        public ReadBuilder(IServiceClientModel client) : base(client)
        {
        }

        protected string WhereAsJson { get; set; } = string.Empty;

        protected string OrderByAsJson { get; set; } = string.Empty;
        protected Type OrderByType { get; set; } = null;
        protected bool OrderByAsc { get; set; } = true;

        protected string SelectAsJson { get; set; } = string.Empty;
        protected Type SelectType { get; set; } = null;

        protected int Page { get; set; } = -1;
        protected int PageSize { get; set; } = -1;

        protected Type ResultType { get; set; } = null;

        // Use deconstructor while protected properties 
        public (Type ModelType,
            bool DebugInfoFlag,
            bool TableOnlyFlag,
            string WhereAsJson,
            string OrderByAsJson,
            Type OrderByType,
            bool OrderByAsc,
            string SelectAsJson,
            Type SelectType,
            int Page,
            int PageSize,
            Type ResutType
            ) GetValues()
            => (ModelType, DebugInfoFlag, TableOnlyFlag,
                WhereAsJson,
                OrderByAsJson, OrderByType, OrderByAsc,
                SelectAsJson,
                SelectType,
                Page, PageSize,
                ResultType);
    }

    public class ReadBuilder<TModel> : ReadBuilder
    {
        public ReadBuilder(IServiceClientModel client) : base(client)
        {
            ModelType = typeof(TModel);
        }

        #region Model
        public ReadBuilder<TModel> DebugInfoAll(bool debugInfoFlagAll)
        {
            ModelBuilder.DebugInfoAll(debugInfoFlagAll);
            return this;
        }
        public ReadBuilder<TModel> DebugInfo(bool debugInfoFlag)
        {
            DebugInfoFlag = debugInfoFlag;
            return this;
        }

        public ReadBuilder<TModel> TableOnly(bool tableOnlyFlag=true)
        {
            TableOnlyFlag = tableOnlyFlag;
            return this;
        }
        #endregion

        #region Where

        public ReadBuilder<TModel> Where(Expression<Func<TModel, bool>> whereExpression)
        {
            if (whereExpression != null)
                WhereAsJson = whereExpression.ToJson();
            return this;
        }

        #endregion

        #region OrderBy

        protected ReadBuilder<TModel> InternalOrderBy<T>(Expression<Func<TModel, T>> orderByExpression, bool asc)
        {
            OrderByAsJson = orderByExpression.ToJson();
            OrderByType = typeof(T);
            OrderByAsc = asc;
            return this;
        }
        public ReadBuilder<TModel> OrderBy<T>(Expression<Func<TModel, T>> orderByExpression)
            => InternalOrderBy(orderByExpression, asc: true);

        public ReadBuilder<TModel> OrderByDescending<T>(Expression<Func<TModel, T>> orderByExpression)
            => InternalOrderBy(orderByExpression, asc: false);

        #endregion

        #region Select
        public ReadBuilder<T> Select<T>(Expression<Func<TModel, T>> selectExpression)
        {
            var readSelectBuilder = new ReadBuilder<T>(Client);
            readSelectBuilder.DebugInfoFlag = DebugInfoFlag;
            readSelectBuilder.TableOnlyFlag = TableOnlyFlag;
            readSelectBuilder.ModelType = ModelType;
            readSelectBuilder.WhereAsJson = WhereAsJson;
            readSelectBuilder.OrderByAsJson = OrderByAsJson;
            readSelectBuilder.OrderByType = OrderByType;
            readSelectBuilder.OrderByAsc = OrderByAsc;
            readSelectBuilder.SelectAsJson = selectExpression.ToJson();
            readSelectBuilder.SelectType = typeof(T);
            return readSelectBuilder;
        }
        #endregion

        #region Paging
        public ReadBuilder<TModel> Paging(int page = -1, int pageSize = -1)
        {
            Page = page;
            PageSize = pageSize;
            return this;
        }
        #endregion

        #region Execute
        protected async Task<IServiceResult<TResult>> ExecuteAsync<TResult>()
        {
            var executeServiceResult = await Client.ExecuteModelReadAsync(this).ConfigureAwait(false);
            var returnResult = executeServiceResult.CastByClone<ServiceResult<TResult>>(withData: false);
            if (executeServiceResult.Status == 0)
                returnResult.Data = (executeServiceResult.Data as string).UnZipString().ToObjectFromJson<TResult>();
            return returnResult;
        }

        public IServiceResult<List<TModel>> ExecuteToList()
            => ExecuteToListAsync().Result;

        public async Task<IServiceResult<List<TModel>>> ExecuteToListAsync()
        {
            ResultType = typeof(List<TModel>);
            return await ExecuteAsync<List<TModel>>().ConfigureAwait(false);
        }

        public IServiceResult<TModel> ExecuteFirstOrDefault(Expression<Func<TModel, bool>> whereExpression = null)
            => ExecuteFirstOrDefaultAsync(whereExpression).Result;

        public async Task<IServiceResult<TModel>> ExecuteFirstOrDefaultAsync(Expression<Func<TModel, bool>> whereExpression = null)
        {
            Where(whereExpression);
            ResultType = typeof(TModel);
            return await ExecuteAsync<TModel>().ConfigureAwait(false);
        }

        #endregion
    }
}
