using GameNight.Core.Http.Interfaces;
using System.Collections.Generic;

namespace GameNight.Core.Controllers
{
    public abstract class BaseController
    {
        protected readonly IGameHttpClient Client;
        public BaseController(IGameHttpClient client)
        {
            Client = client;
        }

        protected Dictionary<string, string> GetParams(params object[] vals)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var val in vals)
            {
                result.Add(
                    val.GetType().GetProperty("Key").GetValue(val).ToString(),
                    val.GetType().GetProperty("Value").GetValue(val).ToString());
            }

            return result;
        }
    }
}
