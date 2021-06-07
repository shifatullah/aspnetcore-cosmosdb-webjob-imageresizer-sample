namespace Images.Services
{
    using Images.Entities;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImageDbService : IImageDbService
    {
        private Container _container;
        private IConfigurationSection _configurationSection;
        private const string containerName = "Image";

        public ImageDbService(
            CosmosClient client,
            IConfigurationSection configurationSection)
        {
            _configurationSection = configurationSection;

            string databaseName = _configurationSection.GetSection("DatabaseName").Value;            

            this._container = client.GetContainer(databaseName, containerName);

            DatabaseResponse database = client.CreateDatabaseIfNotExistsAsync(databaseName).Result;
            database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
        }

        public async Task AddItemAsync(Image image)
        {
            await this._container.CreateItemAsync<Image>(image, new PartitionKey(image.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<Image>(id, new PartitionKey(id));
        }

        public async Task<Image> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Image> response = await this._container.ReadItemAsync<Image>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Image>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Image>(new QueryDefinition(queryString));
            List<Image> results = new List<Image>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Image image)
        {
            await this._container.UpsertItemAsync<Image>(image, new PartitionKey(id));
        }
    }
}