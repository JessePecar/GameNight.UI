using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameNight.Core.Http.Interfaces
{
    public interface IGameHttpClient
    {
        Task<TGet> GetAsync<TGet>(string controller, string action, Dictionary<string, string> paramList);
        Task<TGet> PostAsync<TGet, TPost>(string controller, string action, TPost postObject, Dictionary<string, string> paramList);
        Task<bool> PostForResponseAsync<TPost>(string controller, string action, TPost postObject, Dictionary<string, string> paramList);
    }
}
