using JsonToCsv.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToCsv.Converter
{
    public class JsonToCsvConverter
    {
        public static async Task Convert<T>(Stream bytes, TextWriter output)
        {
            var jsonObject = JsonParser.Parse<T>(bytes);

            var properties = typeof(T).GetProperties();
            var header = new StringBuilder();
            foreach (var property in properties)
            {
                header.Append($"{property.Name},");
            }
            await output.WriteLineAsync(header.ToString());

            foreach (var record in jsonObject)
            {
                var row = new StringBuilder();
                foreach (var property in properties)
                {
                    var value = property.GetValue(record);
                    var writeValue = string.Empty;

                    switch (value)
                    {
                        case Array a:
                            writeValue = ((IEnumerable<object>)a).Aggregate<object, string, string>(string.Empty, (accum, next) => $"{accum}, {next}", (r) => $"{r.Substring(2, r.Length - 2)}");
                            break;
                        case string s:
                            // Remove line breaks and escape special characters
                            writeValue = ((string)value).Replace("\r", " ").Replace("\n", " ").Replace("\"", "\"\"");
                            break;
                        default:
                            writeValue = value.ToString();
                            break;
                    }

                    row.Append($"\"{writeValue}\",");
                }
                await output.WriteLineAsync(row.ToString());
            }

            output.Flush();
        }
    }
}
