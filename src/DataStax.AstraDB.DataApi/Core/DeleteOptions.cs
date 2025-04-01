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

using DataStax.AstraDB.DataApi.Core.Query;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DataStax.AstraDB.DataApi.Core;

public class DeleteOptions<T> where T : class
{
  internal Filter<T> Filter { get; set; }

  [JsonInclude]
  [JsonPropertyName("filter")]
  internal Dictionary<string, object> FilterMap => Filter == null ? new Dictionary<string, object>() : Filter.Serialize();

  [JsonIgnore]
  public SortBuilder<T> Sort { get; set; }

  [JsonInclude]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  [JsonPropertyName("sort")]
  internal Dictionary<string, object> SortMap => Sort == null ? null : Sort.Sorts.ToDictionary(x => x.Name, x => x.Value);
}

public class FindOneAndDeleteOptions<T> : DeleteOptions<T> where T : class
{
  [JsonIgnore]
  public IProjectionBuilder Projection { get; set; }

  [JsonInclude]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  [JsonPropertyName("projection")]
  internal Dictionary<string, object> ProjectionMap => Projection == null ? null : Projection.Projections.ToDictionary(x => x.FieldName, x => x.Value);
}

public class DeleteManyOptions<T> where T : class
{
  internal Filter<T> Filter { get; set; }

  [JsonInclude]
  [JsonPropertyName("filter")]
  internal Dictionary<string, object> FilterMap => Filter == null ? new Dictionary<string, object>() : Filter.Serialize();
}
