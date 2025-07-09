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
using DataStax.AstraDB.DataApi.Core.Results;
using DataStax.AstraDB.DataApi.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStax.AstraDB.DataApi.Admin
{
    public class DatabaseAdminOther : IDatabaseAdmin
    {
        private readonly Guid _id;
        private readonly Database _database;
        private readonly CommandOptions _adminOptions;
        private readonly DataApiClient _client;
        private CommandOptions[] _optionsTree => new CommandOptions[] { _client.ClientOptions, _adminOptions };

        internal DatabaseAdminOther(Database database, DataApiClient client, CommandOptions adminOptions)
        {
            Guard.NotNull(client, nameof(client));
            _client = client;
            _adminOptions = adminOptions;
            _database = database;
            _id = _database.DatabaseId;
        }

        public DatabaseAdminOther(Guid id)
        {
            _id = id;
        }

        public IEnumerable<string> ListKeyspaces()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> ListKeyspacesAsync()
        {
            throw new NotImplementedException();
        }

        public FindEmbeddingProvidersResult FindEmbeddingProviders()
        {
            throw new NotImplementedException();
        }

        public Database GetDatabase(string keyspace)
        {
            throw new NotImplementedException();
        }

        public Database GetDatabase(string keyspace, string userToken)
        {
            throw new NotImplementedException();
        }

        public Database GetDatabase()
        {
            throw new NotImplementedException();
        }

        public void DropKeyspace(string keyspace)
        {
            throw new NotImplementedException();
        }

        public void DropKeyspace(string keyspace, CommandOptions options)
        {
            throw new NotImplementedException();
        }

        public Task DropKeyspaceAsync(string keyspace)
        {
            throw new NotImplementedException();
        }

        public void CreateKeyspace(string keyspace, bool updateDBKeyspace)
        {
            throw new NotImplementedException();
        }

        public void CreateKeyspace(string keyspace)
        {
            throw new NotImplementedException();
        }

        public Task CreateKeyspaceAsync(string keyspace)
        {
            throw new NotImplementedException();
        }

        public bool DoesKeyspaceExist(string keyspace)
        {
            throw new NotImplementedException();
        }

        public void CreateKeyspace(string keyspace, CommandOptions options)
        {
            throw new NotImplementedException();
        }

        public void CreateKeyspace(string keyspace, bool updateDBKeyspace, CommandOptions options)
        {
            throw new NotImplementedException();
        }

        public void CreateKeyspace(string keyspace, bool updateDBKeyspace, bool waitForCompletion)
        {
            throw new NotImplementedException();
        }

        public void CreateKeyspace(string keyspace, bool updateDBKeyspace, bool waitForCompletion, CommandOptions options)
        {
            throw new NotImplementedException();
        }

        public Task CreateKeyspaceAsync(string keyspace, CommandOptions options)
        {
            throw new NotImplementedException();
        }

        public Task CreateKeyspaceAsync(string keyspace, bool updateDBKeyspace)
        {
            throw new NotImplementedException();
        }

        public Task CreateKeyspaceAsync(string keyspace, bool updateDBKeyspace, CommandOptions options)
        {
            throw new NotImplementedException();
        }

        public Task CreateKeyspaceAsync(string keyspace, bool updateDBKeyspace, bool waitForCompletion)
        {
            throw new NotImplementedException();
        }

        public Task CreateKeyspaceAsync(string keyspace, bool updateDBKeyspace, bool waitForCompletion, CommandOptions options)
        {
            throw new NotImplementedException();
        }

        public void DropKeyspace(string keyspace, bool waitForCompletion)
        {
            throw new NotImplementedException();
        }

        public void DropKeyspace(string keyspace, bool waitForCompletion, CommandOptions options)
        {
            throw new NotImplementedException();
        }

        public Task DropKeyspaceAsync(string keyspace, CommandOptions options)
        {
            throw new NotImplementedException();
        }

        public Task DropKeyspaceAsync(string keyspace, bool waitForCompletion)
        {
            throw new NotImplementedException();
        }

        public Task DropKeyspaceAsync(string keyspace, bool waitForCompletion, CommandOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesKeyspaceExistAsync(string keyspace)
        {
            throw new NotImplementedException();
        }
    }
}
