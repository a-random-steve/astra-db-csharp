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

using DataStax.AstraDB.DataApi.Core.Results;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataStax.AstraDB.DataApi.Core.Query;

public class DocumentResultSet<T, TResult> : ResultSet<T, TResult, DocumentSortBuilder<T>>
    where T : class
    where TResult : class
{
    internal DocumentResultSet(IQueryRunner<T, DocumentSortBuilder<T>> queryRunner, Filter<T> filter, IFindManyOptions<T, DocumentSortBuilder<T>> findOptions, CommandOptions commandOptions)
        : base(queryRunner, filter, findOptions, commandOptions)
    {
    }
}

/// <summary>
/// A Fluent API for finding documents in a collection.
/// </summary>
/// <typeparam name="T">The type of the documents in the collection.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public class ResultSet<T, TResult, TSort> : IAsyncEnumerable<TResult>, IEnumerable<TResult>
    where T : class
    where TResult : class
    where TSort : SortBuilder<T>
{
    private readonly Filter<T> _filter;
    private readonly IQueryRunner<T, TSort> _queryRunner;
    private IFindManyOptions<T, TSort> _findOptions;
    private CommandOptions _commandOptions;

    internal Filter<T> Filter => _filter;
    internal IQueryRunner<T, TSort> QueryRunner => _queryRunner;

    private IFindManyOptions<T, TSort> FindOptions
    {
        get { return _findOptions; }
    }

    internal ResultSet(IQueryRunner<T, TSort> queryRunner, Filter<T> filter, IFindManyOptions<T, TSort> findOptions, CommandOptions commandOptions)
    {
        _filter = filter;
        _queryRunner = queryRunner;
        _findOptions = findOptions;
        _commandOptions = commandOptions;
    }

    /// <summary>
    /// Specify a Projection to apply to the results of the operation.
    /// </summary>
    /// <param name="projection">The projection to apply.</param>
    /// <returns>The ResultSet instance to continue specifying the find options.</returns>
    /// <example>
    /// <code>
    /// // Inclusive Projection, return only the nested Properties.PropertyOne field
    /// var projectionBuilder = Builders&lt;SimpleObject&gt;.Projection;
    /// var projection = projectionBuilder.Include(p =&gt; p.Properties.PropertyOne);
    /// </code>
    /// </example>
    public ResultSet<T, TResult, TSort> Project(IProjectionBuilder projection)
    {
        FindOptions.Projection = projection;
        return this;
    }

    /// <summary>
    /// Specify the maximum number of documents to return.
    /// </summary>
    /// <param name="limit">The maximum number of documents to return.</param>
    /// <returns>The ResultSet instance to continue specifying the find options.</returns>
    public ResultSet<T, TResult, TSort> Limit(int limit)
    {
        FindOptions.Limit = limit;
        return this;
    }

    /// <summary>
    /// The number of documents to skip before starting to return documents.
    /// Use in conjuction with <see cref="Sort"/> to determine the order to apply before skipping. 
    /// </summary>
    /// <param name="skip">The number of documents to skip.</param>
    /// <returns>The ResultSet instance to continue specifying the find options.</returns>
    public ResultSet<T, TResult, TSort> Skip(int skip)
    {
        FindOptions.Skip = skip;
        return this;
    }

    /// <summary>
    /// Specify a Sort to use when running the find.
    /// </summary>
    /// <param name="sortBuilder">The sort to apply.</param>
    /// <returns>The ResultSet instance to continue adding options.</returns>
    /// <example>
    /// <code>
    /// // Sort by the nested Properties.PropertyOne field
    /// var sortBuilder = Builders&lt;SimpleObject&gt;.Sort;
    /// var sort = sortBuilder.Ascending(p =&gt; p.Properties.PropertyOne);
    /// </code>
    /// </example>
    public ResultSet<T, TResult, TSort> Sort(TSort sortBuilder)
    {
        FindOptions.Sort = sortBuilder;
        return this;
    }


    /// <summary>
    /// Whether to include the similarity score in the result or not.
    /// </summary>
    /// <param name="includeSimilarity">Whether to include the similarity score in the result or not.</param>
    /// <returns>The ResultSet instance to continue specifying the find options.</returns>
    /// <example>
    /// You can use the attribute <see cref="SerDes.DocumentMappingAttribute"/> to map the similarity score to the result class.
    /// <code>
    /// public class SimpleObjectWithVectorizeResult : SimpleObjectWithVectorize
    /// {
    ///     [DocumentMapping(DocumentMappingField.Similarity)]
    ///     public double? Similarity { get; set; }
    /// }
    /// 
    /// var ResultSet = collection.Find&lt;SimpleObjectWithVectorizeResult&gt;()
    ///     .Sort(Builders&lt;SimpleObjectWithVectorize&gt;.Sort.Vectorize(dogQueryVectorString))
    ///     .IncludeSimilarity(true);
    /// var cursor = ResultSet.ToCursor();
    /// var list = cursor.ToList();
    /// var result = list.First();
    /// var similarity = result.Similarity;
    /// </code>
    /// </example>
    public ResultSet<T, TResult, TSort> IncludeSimilarity(bool includeSimilarity)
    {
        FindOptions.IncludeSimilarity = includeSimilarity;
        return this;
    }

    /// <summary>
    /// Whether to include the sort vector in the result or not.
    /// </summary>
    /// <param name="includeSortVector">Whether to include the sort vector in the result or not.</param>
    /// <returns>The ResultSet instance to continue specifying the find options.</returns>
    /// <example>
    /// To access the sort vectors, you need to use <see cref="Cursor{T}.SortVectors"/> after calling <see cref="ToCursor()"/> on your ResultSet instance.
    /// <code>
    /// var ResultSet = collection.Find&lt;SimpleObjectWithVectorizeResult&gt;()
    ///     .Sort(Builders&lt;SimpleObjectWithVectorize&gt;.Sort.Vectorize(dogQueryVectorString))
    ///     .IncludeSortVector(true);
    /// var cursor = ResultSet.ToCursor();
    /// var sortVector = cursor.SortVectors;
    /// </code>
    /// </example>
    public ResultSet<T, TResult, TSort> IncludeSortVector(bool includeSortVector)
    {
        FindOptions.IncludeSortVector = includeSortVector;
        return this;
    }

    internal Task<ApiResponseWithData<DocumentsResult<TResult>, FindStatusResult>> RunAsync(string pageState = null, bool runSynchronously = false)
    {
        FindOptions.PageState = pageState;
        return _queryRunner.RunFindManyAsync<TResult>(_filter, FindOptions, _commandOptions, runSynchronously);
    }

    /// <summary>
    /// Returns a cursor to iterate over the results of the find operation page by page.
    /// 
    /// NOTE: It is recommended to use the find results as an IEnumerable or IAsyncEnumerable instead of using the Cursor directly.
    /// The only situation where you would need to use the Cursor is when you need to access the sort vectors (see <see cref="IncludeSortVector(bool)"/>)
    /// </summary>
    /// <returns>A cursor to iterate over the results of the find operation page by page.</returns>
    public Cursor<TResult> ToCursor()
    {
        var cursor = new Cursor<TResult>((string pageState, bool runSynchronously) => RunAsync(pageState, runSynchronously));
        return cursor;
    }

    /// <summary>
    /// Returns an async enumerator to iterate over the results of the find operation.
    /// </summary>
    /// <param name="cancellationToken">An optional cancellation token to use for the operation.</param>
    /// <returns>An async enumerator</returns>
    public async IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        var cursor = ToCursor();
        await foreach (var item in cursor.ToAsyncEnumerable(cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return item;
        }
    }

    /// <summary>
    /// Returns an enumerator to iterate over the results of the find operation.    
    /// </summary>
    /// <returns>An enumerator</returns>
    public IEnumerator<TResult> GetEnumerator()
    {
        var cursor = ToCursor();
        bool hasNext;
        do
        {
            hasNext = cursor.MoveNext();
            if (!hasNext || cursor.Current == null)
            {
                yield break;
            }
            foreach (var item in cursor.Current)
            {
                yield return item;
            }
        } while (hasNext);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

}