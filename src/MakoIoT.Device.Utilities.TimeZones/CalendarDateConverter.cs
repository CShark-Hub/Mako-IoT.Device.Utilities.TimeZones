using nanoFramework.Json;
using nanoFramework.Json.Converters;
using System;
using System.Collections;
using System.Diagnostics;

namespace MakoIoT.Device.Utilities.TimeZones
{
    public class CalendarDateConverter : IConverter
    {
        private const string MessageTypeKey = nameof(ICalendarDate.ObjectType);

        public string ToJson(object value)
        {
            throw new NotSupportedException();
        }

        public object ToType(object value)
        {
            var jsonObject = (JsonObject)value;
            var msgTypeJsonValue = (JsonValue)jsonObject.Get(MessageTypeKey).Value;
            var msgTypeString = msgTypeJsonValue.Value.ToString();

            Debug.WriteLine(msgTypeString);


            var msgType = Type.GetType(msgTypeString);

            Debug.WriteLine(msgType.FullName);


            if (msgType == null)
            {
                throw new Exception("Unable to find type.");
            }

            // Convert from Json objects to hashtable
            // Then call serialization to get json string
            // Then call deserialization with proper object
            var hashtable = ExtractKeyValuePairsFromJsonObject(jsonObject);
            var sectionAsJson = JsonConvert.SerializeObject(hashtable);

            Debug.WriteLine(sectionAsJson);
            
            var obj = JsonConvert.DeserializeObject(sectionAsJson, msgType);

            Debug.WriteLine(obj.ToString());

            return obj;
        }

        private Hashtable ExtractKeyValuePairsFromJsonObject(JsonObject jsonObject)
        {
            var hashtable = new Hashtable();
            foreach (var item in jsonObject.Members)
            {
                if (item is JsonProperty jsonProperty)
                {
                    var keyJsonProp = jsonProperty.Name;
                    var propValue = HandleValue(jsonProperty.Value);

                    hashtable.Add(keyJsonProp, propValue);
                    continue;
                }

                throw new NotSupportedException();
            }

            return hashtable;
        }

        private object HandleValue(JsonToken token)
        {
            if (token is JsonValue value)
            {
                return value.Value;
            }

            if (token is JsonObject obj)
            {
                return ExtractKeyValuePairsFromJsonObject(obj);
            }

            throw new NotSupportedException();
        }
    }
}
