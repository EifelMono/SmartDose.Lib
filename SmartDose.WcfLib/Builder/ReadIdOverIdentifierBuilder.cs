﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serialize.Linq.Extensions;
using RowaMore.Extensions;

namespace SmartDose.WcfLib
{
    public class ReadIdOverIdentifierBuilder : ModelBuilder
    {
        public ReadIdOverIdentifierBuilder(IServiceClientModel client) : base(client)
        {
        }

        protected string Identifier { get; set; }

        // Use deconstructor while protected properties 
        public (Type ModelType,
            bool DebugInfoFlag,
            string Identifier
            ) GetValues()
                => (ModelType, DebugInfoFlag, Identifier);
    }

    public class ReadIdOverIdentifierBuilder<TModel> : ReadIdOverIdentifierBuilder where TModel : class
    {
        public ReadIdOverIdentifierBuilder(IServiceClientModel client) : base(client)
        {
            ModelType = typeof(TModel);
        }

        #region Model
        public ReadIdOverIdentifierBuilder<TModel> SetDebugInfoFlagAll(bool debugInfoFlagAll)
        {
            SetDebugInfoFlagAll(debugInfoFlagAll);
            return this;
        }
        public ReadIdOverIdentifierBuilder<TModel> SetDebugInfoFlag(bool debugInfoFlag)
        {
            DebugInfoFlag = debugInfoFlag;
            return this;
        }

        public ReadIdOverIdentifierBuilder<TModel> SetTableOnlyFlag(bool tableOnlyFlag)
        {
            TableOnlyFlag = tableOnlyFlag;
            return this;
        }
        #endregion

        #region Where

        public ReadIdOverIdentifierBuilder<TModel> Where(string identifier)
        {
            if (identifier != null)
                Identifier = identifier;
            return this;
        }
        #endregion

        #region Execute

        public IServiceResult<long> ExecuteFirstOrDefault(string identifier = null)
            => ExecuteFirstOrDefaultAsync(identifier).Result;

        public async Task<IServiceResult<long>> ExecuteFirstOrDefaultAsync(string identifier = null)
        {
            Where(identifier);
            return await Client.ExecuteModelReadIdOverIdentifierAsync(this).ConfigureAwait(false);
        }

        #endregion
    }
}
