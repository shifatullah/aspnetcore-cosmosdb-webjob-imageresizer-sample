using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Images.Models
{
    public class UploadImageViewModel
    {
        public UploadImageViewModel()
        {
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ImageFileName { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
