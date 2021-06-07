using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Images.Models
{
    public class DeleteImageViewModel
    {
        public DeleteImageViewModel()
        {
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("File Name")]
        public string ImageFileName { get; set; }

        public string ImageFileUrl { get; set; }
    }
}
