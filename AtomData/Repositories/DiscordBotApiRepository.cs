using AtomConfiguration;
using AtomData.Entities;
using AtomData.Enums;
using AtomData.Interfaces;
using AtomData.Models;
using AtomData.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace AtomData.Repositories
{
    public class DiscordBotApiRepository : IDiscordBotApiRepository
    {



        public async Task<NewTicketMessageApiReply> CloseTicketAsync(string guildId, string memberId, string dbTicketObjectId)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { GuildId = guildId, MemberId = memberId, DbTicketObjectId = dbTicketObjectId }).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/CloseTicket", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<NewTicketMessageApiReply>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with CloseTicketAsync api.");
            }
            return null;
        }


        public async Task<NewTicketMessageApiReply> ReopenTicketAsync(string guildId, string memberId, string dbTicketObjectId)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { GuildId = guildId, MemberId = memberId, DbTicketObjectId = dbTicketObjectId }).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/ReopenTicket", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<NewTicketMessageApiReply>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with CloseTicketAsync api.");
            }
            return null;
        }


        public async Task<NewTicketMessageApiReply> DeleteTicketAsync(string guildId, string memberId, string dbTicketObjectId)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { GuildId = guildId, MemberId = memberId, DbTicketObjectId = dbTicketObjectId }).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/DeleteTicket", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<NewTicketMessageApiReply>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with CloseTicketAsync api.");
            }
            return null;
        }




        public async Task<NewTicketMessageApiReply> SendTicketMessageAsync(string guildId, string memberId, string dbTicketObjectId, string message)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { GuildId = guildId, MemberId = memberId, DbTicketObjectId = dbTicketObjectId, Message = message }).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/SendTicketMessage", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<NewTicketMessageApiReply>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with SendTicketMessageAsync api.");
            }
            return null;
        }




        public async Task<List<BotCommand>> GetAllBotCommandsAsync()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(PrivateConfig.BotApiUrl + "/GetAllCommands");
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<List<BotCommand>>(decrypted);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("No connection could be made because the target machine actively refused it"))
                    throw new Exception("Bot is curently offline.");
                else
                    throw new Exception("Error interacting with Commands api.");
            }
            return new List<BotCommand>();
        }

        public async Task<BotStatsModel> GetBotStatsAsync()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(PrivateConfig.BotApiUrl + "/GetBotStats");
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<BotStatsModel>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with BotStats api.");
            }
            return new BotStatsModel();
        }

        public async Task<GuildInfoModel> GetGuildInfoAsync(string guildId, string memberId)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { GuildId = guildId, UserId = memberId }).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/GetGuildInfo", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<GuildInfoModel>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with GuildInfo api.");
            }
            return null;
        }

        public async Task<List<DiscordGuild>> GetMutualDiscordServersAsync(DiscordApiPostMutualServersModel postModel)
        {
            try
            {
                var json = JsonConvert.SerializeObject(postModel).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/CheckMutualServers", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<List<DiscordGuild>>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with MutualServer api.");
            }
            return null;
        }

        public async Task SendDiscordMessageAsync(string guildId, DiscordMessageApiTypeEnum Type)
        {

            try
            {
                var json = JsonConvert.SerializeObject(new { GuildId = guildId, Type = Type.ToString() }).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();

                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/SendGuildMessage", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with GuildInfo api.");
            }
        }

        public async Task SendLoginMessageAsync(DiscordUser user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/SendLoginMsg", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                //ignored
            }
        }

        public async Task<SetMemberRolesApiReplyModel> SetMemberRolesAsync(SetMemberRolesApiSendModel model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/SetMemberRolesAsync", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<SetMemberRolesApiReplyModel>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with GuildInfo api.");
            }
            return new SetMemberRolesApiReplyModel { Message = "Somthing went whrong.", Succes = false };
        }

        public async Task<StartGiveawayApiReplyModel> StartGiveaway(Giveaway model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/StartGiveaway", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    return JsonConvert.DeserializeObject<StartGiveawayApiReplyModel>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with Commands api.");
            }
            return new StartGiveawayApiReplyModel { messageId = null };
        }

        public async Task<UserGuildValidationApiModel> ValidateUserGuildAsync(string guildId, string userId)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new { GuildId = guildId, UserId = userId }).ToString();
                var encrypted = Xor.Encrypt(json);
                var reSerialized = JsonConvert.SerializeObject(new { Data = encrypted }).ToString();
                var postData = new StringContent(reSerialized, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(PrivateConfig.BotApiUrl + "/ValidateGuildUser", postData);
                var ApiResultString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(ApiResultString))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(ApiResultString);
                    var decrypted = Xor.Decrypt(apiResponse?.Data ?? "");
                    if (userId == PrivateConfig.BotOwnerId)
                    {
                        return new UserGuildValidationApiModel { success = true, message = "" };
                    }
                    return JsonConvert.DeserializeObject<UserGuildValidationApiModel>(decrypted);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error interacting with GuildInfo api.");
            }
            return new UserGuildValidationApiModel { success = false, message = "Error from API." };
        }

    }
}
