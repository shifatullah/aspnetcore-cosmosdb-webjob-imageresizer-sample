using Images.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Images.Services
{

    public interface IImageDbService
    {
        Task<IEnumerable<Image>> GetItemsAsync(string query);
        Task<Image> GetItemAsync(string id);
        Task AddItemAsync(Image item);
        Task UpdateItemAsync(string id, Image item);
        Task DeleteItemAsync(string id);
    }
}
