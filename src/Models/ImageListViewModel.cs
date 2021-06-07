using System;
using System.Collections.Generic;
using Images.Entities;

namespace Images.Models
{
    public class ImageListViewModel
    {
        public ImageListViewModel()
        {
        }

        public List<Image> Images { get; set; }
    }
}
