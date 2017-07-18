using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace M101N.Models.Home
{
    public class NewCommentModel
    {
        [HiddenInput(DisplayValue = false)]
        public string PostId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Content { get; set; }
    }
}