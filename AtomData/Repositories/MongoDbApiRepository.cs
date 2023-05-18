using AtomData.Interfaces;
using AtomWeb.Models;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using AtomData.Models;
using AtomData.Services;
using AtomConfiguration;

namespace AtomData.Repositories
{
    public class MongoDbApiRepository : IMongoDbApiRepository
    {
        public async Task<string> GetDbObjectAsync(ApiGetModel getModel)
        {
            try
            {
                getModel.Time = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                var json = JsonConvert.SerializeObject(getModel).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/GetDbItem", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return decrypted;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public async Task<DbUpdateApiResponse?> UpdateDbObjectAsync(ApiPostModel postModel)
        {
            try
            {
                postModel.Time = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                var json = JsonConvert.SerializeObject(postModel).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/UpdateDbItem", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<DbUpdateApiResponse>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }
    }
}
