using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{
    internal class PolymorphicJsonConverter<T> : JsonConverter<T>
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string? propertyName = reader.GetString();

            if (propertyName != "Type")
            {
                throw new JsonException();
            }

            reader.Read();

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            string? assemblyQualifiedName = reader.GetString();

            if (assemblyQualifiedName == null)
            {
                throw new JsonException(null, new NullReferenceException());
            }

            Type? type = Type.GetType(assemblyQualifiedName);

            if (type == null)
            {
                int indexOfComma = assemblyQualifiedName.IndexOf(',');
                string assemblyName = assemblyQualifiedName.Substring(assemblyQualifiedName.IndexOf(',') + 2);
                Assembly? assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName == assemblyName);
                if (assembly == null)
                {
                    throw new JsonException(null, new NullReferenceException());
                }
                else
                {
                    type = assembly.GetType(assemblyQualifiedName.Substring(0, indexOfComma));
                    if (type == null)
                    {
                        throw new JsonException(null, new NullReferenceException());
                    }
                }
            }

            if (!typeof(T).IsAssignableFrom(type))
            {
                throw new JsonException();
            }

            reader.Read();

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            propertyName = reader.GetString();

            if (propertyName != "Data")
            {
                throw new JsonException();
            }

            reader.Read();

            // TODO: Perhaps 'options' should be omitted here for security reasons. Still we might want to pass arguments such as WriteIndented?
            T? result = (T?)(JsonSerializer.Deserialize(ref reader, type, new JsonSerializerOptions()));

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            reader.Read();

            return result;
        }

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(value);

            writer.WriteStartObject();

            writer.WritePropertyName("Type");

            writer.WriteStringValue(value.GetType().AssemblyQualifiedName);

            writer.WritePropertyName("Data");

            lock (value)
                JsonSerializer.Serialize(writer, value, value.GetType(), new JsonSerializerOptions());

            writer.WriteEndObject();
        }
    }
}
