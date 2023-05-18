using AtomData.Interfaces;
using AtomData.Models;
using AtomWeb.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AtomServices
{
    public class MongoDbApiServices : IMongoDbApiRepository
    {
        private readonly IMongoDbApiRepository _mongoDbRepo;
        public MongoDbApiServices(IMongoDbApiRepository mongoDbRepo)
        {
            _mongoDbRepo = mongoDbRepo;
        }

        public async Task<string> GetDbObjectAsync(ApiGetModel getModel) => await _mongoDbRepo.GetDbObjectAsync(getModel);
        public async Task<DbUpdateApiResponse?> UpdateDbObjectAsync(ApiPostModel postModel) => await _mongoDbRepo.UpdateDbObjectAsync(postModel);

    }
}
