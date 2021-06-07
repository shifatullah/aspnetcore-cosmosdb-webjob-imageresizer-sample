using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Images.Entities
{
    public class Image
    {
        public Image()
        {
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string FileName { get; set; }
    }
}
