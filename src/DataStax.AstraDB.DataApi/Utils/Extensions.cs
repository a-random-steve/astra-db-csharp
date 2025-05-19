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
using DataStax.AstraDB.DataApi.SerDes;
using DataStax.AstraDB.DataApi.Tables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace DataStax.AstraDB.DataApi.Utils;

internal static class Extensions
{
    internal static string OrIfEmpty(this string str, string alternate)
    {
        return string.IsNullOrEmpty(str) ? alternate : str;
    }

    internal static T Merge<T>(this IEnumerable<T> list)
    {
        T result = default(T);
        foreach (T item in list)
        {
            result = EqualityComparer<T>.Default.Equals(item, default) ? result : item;
        }
        return result;
    }

    internal static string GetMemberNameTree<T1, T2>(this Expression<Func<T1, T2>> expression)
    {
        var body = expression.Body;
        if (body is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Convert)
        {
            body = unaryExpression.Operand;
        }

        if (body is MemberExpression memberExpression)
        {
            StringBuilder sb = new StringBuilder();
            BuildPropertyName(memberExpression, sb);
            return sb.ToString();
        }

        throw new ArgumentException("Invalid property expression.");
    }

    internal static string GetMemberNameTree(this Expression expression)
    {
        if (expression is MemberExpression memberExpression)
        {
            StringBuilder sb = new StringBuilder();
            BuildPropertyName(memberExpression, sb);
            return sb.ToString();
        }

        throw new ArgumentException("Invalid property expression.");
    }

    private static void BuildPropertyName(MemberExpression memberExpression, StringBuilder sb)
    {
        if (memberExpression.Expression is MemberExpression parentExpression)
        {
            BuildPropertyName(parentExpression, sb);
            sb.Append('.');
        }

        var name = memberExpression.Member.Name;
        if (memberExpression.Member is PropertyInfo propertyInfo)
        {
            var jsonPropertyNameAttribute = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (jsonPropertyNameAttribute != null)
            {
                name = jsonPropertyNameAttribute.Name;
            }
            var attribute = propertyInfo.GetCustomAttribute<DocumentIdAttribute>();
            if (attribute != null)
            {
                name = DataApiKeywords.Id;
            }
            var columnNameAttribute = propertyInfo.GetCustomAttribute<ColumnNameAttribute>();
            if (columnNameAttribute != null)
            {
                name = columnNameAttribute.ColumnName;
            }
        }
        sb.Append(name);
    }

}