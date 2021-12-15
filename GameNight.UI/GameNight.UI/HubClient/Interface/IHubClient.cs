using GameNight.Models.Dice;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace GameNight.UI.HubClient.Interface
{
    public interface IHubClient
    {
        Task JoinGame(string lobbyKey, string password, string userName, Guid? adminKey = null);

        Task LeaveGame(string lobbyKey);

        void NextTurn(string lobbyKey);

        Task SendPlayersDetails(string lobbyKey, string user, object details);

        void StartGame(string lobbyKey, Guid? adminKey);

        void StartRound(string lobbyKey, Guid? adminKey);

        void SubmitToJudge(string lobbyKey, object submission);

        void UpdateScore(string lobbyKey, string userName);

        void SetupHandlerForViewModel(Func<HubConnection, Action> handler);

        Task LeaveAllGames();
    }
}
