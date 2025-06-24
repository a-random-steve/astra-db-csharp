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

using DataStax.AstraDB.DataApi.Core.Commands;
using System.Collections.Generic;

namespace DataStax.AstraDB.DataApi.Core.Query;


internal class Projection
{
    internal string FieldName { get; set; }
    internal bool Include { get; set; }
    internal int? SliceStart { get; set; }
    internal int? SliceLength { get; set; }

    internal object Value
    {
        get
        {
            if (SliceStart.HasValue)
            {
                object sliceVal;
                if (SliceLength.HasValue)
                {
                    sliceVal = new int[] { SliceStart.Value, SliceLength.Value };
                }
                else
                {
                    sliceVal = SliceStart.Value;
                }
                return new Dictionary<string, object>
                {
                    { DataApiKeywords.Slice, sliceVal }
                };
            }
            return Include;
        }
    }

    internal Projection Clone()
    {
        return new Projection
        {
            FieldName = FieldName,
            Include = Include,
            SliceStart = SliceStart,
            SliceLength = SliceLength
        };
    }
}
