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
            var readSelectBuilder = new ReadBuilder<T>(Client)
            {
                DebugInfoFlag = DebugInfoFlag,
                TableOnlyFlag = TableOnlyFlag,
                ModelType = ModelType,
                WhereAsJson = WhereAsJson,
                OrderByAsJson = OrderByAsJson,
                OrderByType = OrderByType,
                OrderByAsc = OrderByAsc,
                SelectAsJson = selectExpression.ToJson(),
                SelectType = typeof(T)
            };
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

        #region Exe
        protected async Task<IServiceResult<TResult>> ExecAsync<TResult>()
        {
            var execServiceResult = await Client.ExecModelReadAsync(this).ConfigureAwait(false);
            var returnResult = execServiceResult.CastByClone<ServiceResult<TResult>>(withData: false);
            if (execServiceResult.Status == 0)
                returnResult.Data = (execServiceResult.Data as string).UnZipString().ToObjectFromJson<TResult>();
            return returnResult;
        }

        public IServiceResult<List<TModel>> ExecToList()
            => ExecToListAsync().Result;

        public async Task<IServiceResult<List<TModel>>> ExecToListAsync()
        {
            ResultType = typeof(List<TModel>);
            return await ExecAsync<List<TModel>>().ConfigureAwait(false);
        }

        public IServiceResult<TModel> ExecFirstOrDefault(Expression<Func<TModel, bool>> whereExpression = null)
            => ExecFirstOrDefaultAsync(whereExpression).Result;

        public async Task<IServiceResult<TModel>> ExecFirstOrDefaultAsync(Expression<Func<TModel, bool>> whereExpression = null)
        {
            Where(whereExpression);
            ResultType = typeof(TModel);
            return await ExecAsync<TModel>().ConfigureAwait(false);
        }

        public IServiceResult<int> ExecCount(Expression<Func<TModel, bool>> whereExpression = null)
            => ExecCountAsync(whereExpression).Result;

        public async Task<IServiceResult<int>> ExecCountAsync(Expression<Func<TModel, bool>> whereExpression = null)
        {
            Where(whereExpression);
            ResultType = typeof(int);
            var readSelectBuilder = new ReadBuilder<int>(Client)
            {
                DebugInfoFlag = DebugInfoFlag,
                TableOnlyFlag = TableOnlyFlag,
                ModelType = ModelType,
                WhereAsJson = WhereAsJson,
                OrderByAsJson = OrderByAsJson,
                OrderByType = OrderByType,
                OrderByAsc = OrderByAsc,
                SelectAsJson = SelectAsJson,
                SelectType = SelectType,
                ResultType= ResultType
            };
            return await readSelectBuilder.ExecAsync<int>().ConfigureAwait(false);
        }

        #endregion
    }
}
