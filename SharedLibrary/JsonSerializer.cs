using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

namespace SharedLibrary
{
    public class JsonSerializer<T> : ISerializer<T>, IDeserializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            if (data == null)
            {
                return null!;
            }
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return default!;
            }
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data.ToArray()))!;
        }
    }
}
