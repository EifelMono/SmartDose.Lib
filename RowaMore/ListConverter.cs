using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RowaMore
{
    public class ListConverter : CollectionConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            if (!(value is IList list) || list.Count == 0)
                return base.GetProperties(context, value, attributes);

            var items = new PropertyDescriptorCollection(null);
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                items.Add(new ExpandableCollectionPropertyDescriptor(list, i));
            }
            return items;
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return base.IsValid(context, value);
        }

    }

    public class ExpandableCollectionPropertyDescriptor : PropertyDescriptor
    {
        private readonly IList _collection;
        private readonly int _index;

        public ExpandableCollectionPropertyDescriptor(IList coll, int idx)
            : base(GetDisplayName(coll, idx), null)
        {
            _collection = coll;
            _index = idx;
        }

        private static string GetDisplayName(IList list, int index)
        {
            return "[" + index + "]  " + CSharpName(list[index].GetType());
        }

        private static string CSharpName(Type type)
        {
            var sb = new StringBuilder();
            var name = type.Name;
            if (!type.IsGenericType)
                return name;
            sb.Append(name.Substring(0, name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments()
                                            .Select(CSharpName)));
            sb.Append(">");
            return sb.ToString();
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get { return _collection.GetType(); }
        }

        public override object GetValue(object component)
        {
            if (_index < _collection.Count)
                return _collection[_index];
            return null;
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override string Name
        {
            get { return _index.ToString(CultureInfo.InvariantCulture); }
        }

        public override Type PropertyType
        {
            get { return _collection[_index].GetType(); }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override void SetValue(object component, object value)
        {
            _collection[_index] = value;
        }

        public override void ResetValue(object component)
        {
        }
    }
}
