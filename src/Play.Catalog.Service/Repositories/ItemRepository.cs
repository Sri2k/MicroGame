using MongoDB.Driver;

namespace Play.Catalog.Service.Repositories
{
    public class ItemRepository
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<Item>? dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        public ItemRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<Item>(collectionName);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllSync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid Id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, Id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            await dbCollection.InsertOneAsync(entity);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, entity.Id);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            await dbCollection.ReplaceOneAsync(filter, entity);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        }

        public async Task RemoveAsync(Guid Id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, Id);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            await dbCollection.DeleteOneAsync(filter);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}