using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace M101N.Models.Home
{
    public class IndexModel
    {
        public List<Post> RecentPosts { get; set; }
    }
}