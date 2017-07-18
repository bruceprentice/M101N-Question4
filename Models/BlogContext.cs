using System;
using System.Collections.Generic;
using System.Linq;
using M101N.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace M101N.Models
{
    public class BlogContext
    {

       // public const string CONNECTION_STRING_NAME = "Blog";
       // public const string DATABASE_NAME = "Blog";
        public const string POSTS_COLLECTION_NAME = "posts";
        public const string USERS_COLLECTION_NAME = "users";
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly Settings _options;
        public BlogContext(IOptions<Settings> optionsAccessor)
        {
            _options = optionsAccessor.Value;
            _client = new MongoClient(_options.ConnectionString);
            _database = _client.GetDatabase(_options.Database);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }
        public IMongoCollection<Post> Posts
        {
            get { return _database.GetCollection<Post>(POSTS_COLLECTION_NAME); }
        }

        
    }
}