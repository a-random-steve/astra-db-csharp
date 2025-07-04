/*
 * Copyright DataStax, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using DataStax.AstraDB.DataApi.Core;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataStax.AstraDB.DataApi.SerDes;


public class AnalyzerOptionsConverter : JsonConverter<AnalyzerOptions>
{
    public override AnalyzerOptions Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var analyzerName = reader.GetString();
            return new AnalyzerOptions
            {
                Tokenizer = new TokenizerOptions { Name = analyzerName }
            };
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            return JsonSerializer.Deserialize<AnalyzerOptions>(ref reader, options);
        }
        throw new JsonException("Unexpected token type for AnalyzerOptions");
    }

    public override void Write(Utf8JsonWriter writer, AnalyzerOptions value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}