using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101N.Models
{
    public class Comment
    {
        public string Author { get; set; }
        public string Content { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAtUtc { get; set; }
        public int Likes { get; set; }
    }
}