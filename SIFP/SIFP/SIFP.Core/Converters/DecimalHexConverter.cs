using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SIFP.Core.Converters
{
    public class DecimalHexConverter : JsonConverter<UInt32>
    {
        public override uint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            UInt32 result = 0;
            try
            {
                result = Convert.ToUInt32(reader.GetString(), 16);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"error Addr：{reader.GetString()} in .json file");
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
        {
            writer.WriteStringValue("0x" + value.ToString("x4"));
        }
    }
}
