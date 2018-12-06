using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RowaMore.Extensions
{
    public static class JsonExtensions
    {
        public static JsonSerializerSettings JsonSerializerSettingsExpandableObject = new JsonSerializerSettings()
        {
            ContractResolver = new SerializableExpandableObjetContractResolver(),
            Formatting = Formatting.Indented
        };

        public static string ToJson(this object thisValue)
            => thisValue.ToJsonFromObject();

        public static string ToJsonFromObject(this object thisValue)
            => JsonConvert.SerializeObject(thisValue, Formatting.Indented);

        public static T ToObjectFromJson<T>(this string thisValue)
            => JsonConvert.DeserializeObject<T>(thisValue);

        public static string ToJsonFromExpandableObject(this object thisValue)
            => JsonConvert.SerializeObject(thisValue, settings: JsonSerializerSettingsExpandableObject);

        public static T ToExpandableObjectFromJson<T>(this string thisValue)
            => JsonConvert.DeserializeObject<T>(thisValue, settings: JsonSerializerSettingsExpandableObject);

        public static object ToExpandableObjectFromJson(this string value, Type type)
            => JsonConvert.DeserializeObject(value, type, settings: JsonSerializerSettingsExpandableObject);
    }

    public class SerializableExpandableObjetContractResolver : DefaultContractResolver
    {
        public SerializableExpandableObjetContractResolver()
        {
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            if (TypeDescriptor.GetAttributes(objectType).Contains(new TypeConverterAttribute(typeof(ExpandableObjectConverter))))
            {
                return CreateObjectContract(objectType);
            }
            return base.CreateContract(objectType);
        }
    }
}
