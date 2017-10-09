using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace JsonToCsv.Parser
{
    public class JsonParser
    {
        /// <summary>
        /// Parses a json array of type T to a 
        /// .NET List<T>
        /// </summary>
        /// <typeparam name="T">Typeof record</typeparam>
        /// <param name="bytes">Bytestream of data</param>
        /// <returns></returns>
        public static List<T> Parse<T>(Stream bytes)
        {
            var serialiser = new JsonSerializer();

            using (var sr = new StreamReader(bytes))
            using (var jsonReader = new JsonTextReader(sr))
            {
                return serialiser.Deserialize<List<T>>(jsonReader);
            }
        }
    }
}
