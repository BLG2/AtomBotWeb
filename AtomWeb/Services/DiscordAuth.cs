using AtomConfiguration;
using AtomData.Models;
using AtomWeb.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AtomWeb.Services
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class DiscordAuth
    {
        public static async Task<DiscordLoginTokenModel> GetAuthTokenAsync(string authUrlCode)
        {
            var values = new Dictionary<string, string>
             {
                  { "client_id", PrivateConfig.BotId},
                  { "client_secret", PrivateConfig.client_secret },
                  { "redirect_uri", PrivateConfig.redirect_uri },
                  { "grant_type", "authorization_code" },
                  { "code", authUrlCode },
                  { "scope",  "identify%20guilds" }
              };


            var postData = new FormUrlEncodedContent(values);
            using var client = new HttpClient();
            var response = await client.PostAsync("https://discordapp.com/api/oauth2/token", postData);
            var ApiResultString = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode != HttpStatusCode.OK) return null;
            if (!string.IsNullOrEmpty(ApiResultString))
            {
                var loginTokenResp = JsonConvert.DeserializeObject<DiscordLoginTokenModel>(ApiResultString);
                if (loginTokenResp != null)
                {
                    return loginTokenResp;
                }
            }
            return null;
        }


        public static async Task<DiscordUser?> GetUserWithAccesTokenAsync(string accesToken)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesToken);
                var response = await client.GetAsync("https://discordapp.com/api/users/@me");
                var responseString = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode != HttpStatusCode.OK) return null;
                var deSerialized = JsonConvert.DeserializeObject<DiscordUser>(responseString);
                return deSerialized ?? null;
            }
            catch (Exception)
            {
                throw new Exception("Somthing went whrong getting userInfo with accesToken.");
            }
        }
        public static async Task<List<DiscordGuild>?> GetUserGuildsWithAccesTokenAsync(string accesToken)
        {
            try
            {
                if (accesToken != null)
                {
                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesToken);
                    var response = await client.GetAsync("https://discordapp.com/api/users/@me/guilds");
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode != HttpStatusCode.OK) return null;
                    var deSerialized = JsonConvert.DeserializeObject<List<DiscordGuild>>(responseString);
                    return deSerialized ?? null;
                }
                return null;
            }
            catch (Exception)
            {
                throw new Exception("Somthing went whrong getting guilds with accesToken.");
            }

        }




    }
}
