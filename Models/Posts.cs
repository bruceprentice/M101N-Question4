using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101N.Models
{
    public class Post
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        
        public List<string> Tags { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAtUtc { get; set; }

        public List<Comment> Comments { get; set; }

    }
}


