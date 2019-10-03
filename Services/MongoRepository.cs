using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

using apiaccounts.Models;

public class MongoRepository {
    //delcaring mongo db
    public readonly IMongoDatabase _db;

    public MongoRepository(IOptions<Settings> setting) {
        try {
            var client = new MongoClient(setting.Value.ConnectionString);
            if (client != null)
                _db = client.GetDatabase(setting.Value.Database);
        }
        catch(Exception ex) {
            throw new Exception("Can not access to MongoDb server.", ex);
        }
    }

    public IMongoCollection<User> users => _db.GetCollection<User>("Users");

}
