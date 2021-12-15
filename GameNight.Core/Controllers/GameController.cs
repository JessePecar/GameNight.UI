using GameNight.Core.Http.Interfaces;
using GameNight.Models.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameNight.Core.Controllers
{
    public class GameController : BaseController
    {
        public GameController(IGameHttpClient client) : base(client)
        {
        }

        public async Task<GameManager> InitializeNewGame(string password, GameType gameType = GameType.TableTopRPG)
        {
            return await Client.GetAsync<GameManager>(
                "Game", 
                nameof(InitializeNewGame),
                GetParams(new { Key = "password", Value = password }, new { Key = "gameType", Value = gameType }));
        }

        public async Task<bool> CloseGame(string lobbyKey, Guid adminKey)
        {
            return await Client.PostForResponseAsync<GameManager>(
                "Game",
                nameof(CloseGame),
                new GameManager { AdminKey = adminKey, LobbyKey = lobbyKey }, 
                null);
        }

       
    }
}
