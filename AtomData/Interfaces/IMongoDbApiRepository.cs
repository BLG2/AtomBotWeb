using AtomData.Models;
using AtomWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AtomData.Interfaces
{
    public interface IMongoDbApiRepository
    {
        Task<string> GetDbObjectAsync(ApiGetModel getModel);
        Task<DbUpdateApiResponse?> UpdateDbObjectAsync(ApiPostModel postModel);
    }
}
