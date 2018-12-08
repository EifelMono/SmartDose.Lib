using System;
using System.Collections.Generic;
using System.Text;

#if MasterData9002
namespace MasterData9002
#else
namespace ServicesShared
#endif
{
    public partial class SearchFilter
    {
        public static List<SearchFilter> New()
            => new List<SearchFilter>();

        public static SearchFilter New(string name, string value, ComparisonType comparisonType, ConcatenationType concatenationType)
            => new SearchFilter()
            {
                Name = name,
                Value = value,
                ComparisonType = comparisonType,
                ConcatenationType = concatenationType
            };

        public static List<SearchFilter> New(params SearchFilter[] searchFilters)
            => new List<SearchFilter>().AddSearchFilters(searchFilters);
    }
    public static class SearchFilterExtensions
    {
        #region Globals
        public static List<SearchFilter> AddSearchFilters(this List<SearchFilter> thisValue, params SearchFilter[] searchFilters)
        {
            foreach (var searchFilter in searchFilters)
                thisValue.Add(searchFilter);
            return thisValue;
        }

        public static List<SearchFilter> AddSearchFilter(this List<SearchFilter> thisValue,
            string name, string value, ComparisonType comparisonType, ConcatenationType concatenationType)
            => thisValue.AddSearchFilters(SearchFilter.New(name, value, comparisonType, concatenationType));

        public static List<SearchFilter> AddEqualToOr(this List<SearchFilter> thisValue, string name, string value)
            => thisValue.AddSearchFilters(SearchFilter.New(name, value, ComparisonType.EqualTo, ConcatenationType.Or));

        public static List<SearchFilter> AddEqualToAnd(this List<SearchFilter> thisValue, string name, string value)
        => thisValue.AddSearchFilters(SearchFilter.New(name, value, ComparisonType.EqualTo, ConcatenationType.And));
        #endregion

        #region Add by name

        #region Identifier
        private const string IdentifierName = "Identifier";
        public static List<SearchFilter> AddIdentifier(this List<SearchFilter> thisValue, string value, ComparisonType comparisonType = ComparisonType.EqualTo, ConcatenationType concatenationType = ConcatenationType.Or)
            => thisValue.AddSearchFilter(IdentifierName, value, comparisonType, concatenationType);
        public static List<SearchFilter> AddIdentifierEqualToOr(this List<SearchFilter> thisValue, string value)
            => thisValue.AddIdentifier(value, ComparisonType.EqualTo, ConcatenationType.Or);

        public static List<SearchFilter> AddIdentifierEqualToAnd(this List<SearchFilter> thisValue, string value)
         => thisValue.AddIdentifier(value, ComparisonType.EqualTo, ConcatenationType.And);
        #endregion

        #region ParentOrderIdentifier
        private const string ParentOrderIdentifierName = "ParentOrderIdentifier";
        public static List<SearchFilter> AddParentOrderIdentifier(this List<SearchFilter> thisValue, string value, ComparisonType comparisonType = ComparisonType.EqualTo, ConcatenationType concatenationType = ConcatenationType.Or)
           => thisValue.AddSearchFilter(ParentOrderIdentifierName, value, comparisonType, concatenationType);
        #endregion

        #region IsDeleted
        private const string IsDeletedName = "IsDeleted";
        public static List<SearchFilter> AddIsDeleted(this List<SearchFilter> thisValue, string value, ComparisonType comparisonType = ComparisonType.EqualTo, ConcatenationType concatenationType = ConcatenationType.Or)
           => thisValue.AddSearchFilter(IsDeletedName, value, comparisonType, concatenationType);
        #endregion

        #region Status
        private const string StatusName = "Status";
        public static List<SearchFilter> AddStatus(this List<SearchFilter> thisValue, string value, ComparisonType comparisonType = ComparisonType.EqualTo, ConcatenationType concatenationType = ConcatenationType.Or)
           => thisValue.AddSearchFilter(StatusName, value, comparisonType, concatenationType);
        #endregion

        #region ById
        private const string ById = "ById";
        public static List<SearchFilter> AddById(this List<SearchFilter> thisValue, string value, ComparisonType comparisonType = ComparisonType.EqualTo, ConcatenationType concatenationType = ConcatenationType.Or)
           => thisValue.AddSearchFilter(ById, value, comparisonType, concatenationType);
        #endregion

        #region ByName
        private const string ByName = "ByName";
        public static List<SearchFilter> AddByName(this List<SearchFilter> thisValue, string value, ComparisonType comparisonType = ComparisonType.EqualTo, ConcatenationType concatenationType = ConcatenationType.Or)
           => thisValue.AddSearchFilter(ByName, value, comparisonType, concatenationType);
        #endregion

        #region ByIdentifier
        private const string ByIdentifier = "ByIdentifier";
        public static List<SearchFilter> AddByIdentifier(this List<SearchFilter> thisValue, string value, ComparisonType comparisonType = ComparisonType.EqualTo, ConcatenationType concatenationType = ConcatenationType.Or)
            => thisValue.AddSearchFilter(ByBarcode, value, comparisonType, concatenationType);
        #endregion

        #region ByIdentifier
        private const string ByBarcode = "ByBarcode";
        public static List<SearchFilter> AddByBarcode(this List<SearchFilter> thisValue, string value, ComparisonType comparisonType = ComparisonType.EqualTo, ConcatenationType concatenationType = ConcatenationType.Or)
           => thisValue.AddSearchFilter(ByBarcode, value, comparisonType, concatenationType);
        #endregion

        #region ByIdentifier
        private const string BySynonym = "BySynonym";
        public static List<SearchFilter> AddBySynonym(this List<SearchFilter> thisValue, string value, ComparisonType comparisonType = ComparisonType.EqualTo, ConcatenationType concatenationType = ConcatenationType.Or)
           => thisValue.AddSearchFilter(BySynonym, value, comparisonType, concatenationType);
        #endregion

        #endregion
    }
}
