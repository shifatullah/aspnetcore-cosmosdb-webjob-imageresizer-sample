using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Images.Models
{
    public class ImageViewModel
    {
        public ImageViewModel()
        {
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
