using GameNight.Models.Dice;
using GameNight.UI.HubClient.Interface;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameNight.UI.HubClient
{
    public class HubClient : IHubClient
    {
        HubConnection connection;
        private bool _isConnected => connection?.State == HubConnectionState.Connected;

        public HubClient()
        {
            connection = new HubConnectionBuilder()
                .WithUrl(new Uri("http://gamenight.jessepecar.com/lobby"))
                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10) })
                .Build();

            Task.Run(async () => await connection.StartAsync()).ContinueWith(async (tsk) =>
            {
                await ConfigureHandler();
            });
        }

        private async Task ConfigureHandler()
        {
            connection.On("InvalidGameRequest", async () =>
            {
                //Handle Invalid Game Requests
                await App.Current.MainPage.DisplayAlert("Error", "You have submitted an invalid game request", "OK");
            });
        }

        public void SetupHandlerForViewModel(Action<HubConnection> handler)
        {
            handler.Invoke(connection);
        }

        public async Task JoinGame(string lobbyKey, string password, string userName, Guid? adminKey = null)
        {
            try
            {
                if (!_isConnected || connection.State != HubConnectionState.Connected)
                {
                    await connection.StartAsync();
                }
                await connection.InvokeAsync("JoinGame", lobbyKey, password, userName, App.DeviceKey, adminKey);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "An unknown issue occured", "Ok");
            }
        }

        public async Task LeaveGame(string lobbyKey)
        {
            await connection.InvokeAsync("LeaveGame", lobbyKey, App.DeviceKey);
        }

        public async Task LeaveAllGames()
        {
            await connection.InvokeAsync(nameof(LeaveAllGames), App.DeviceKey);
        }

        public void NextTurn(string lobbyKey)
        {
            connection.InvokeAsync("NextTurn", lobbyKey);
        }

        public async Task SendPlayersDetails(string lobbyKey, string user, object details)
        {
            try
            {
                if (!_isConnected || connection.State != HubConnectionState.Connected)
                {
                    await connection.StartAsync();
                }
                await connection.InvokeAsync("SendPlayersDetails", lobbyKey, user, details);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "An unknown issue occured", "Ok");
            }
        }

        public void StartGame(string lobbyKey, Guid? adminKey)
        {
            connection.InvokeAsync("StartGame", lobbyKey, adminKey);
        }

        public void StartRound(string lobbyKey, Guid? adminKey)
        {
            connection.InvokeAsync("StartRound", lobbyKey, adminKey);
        }

        public void SubmitToJudge(string lobbyKey, object submission)
        {
            connection.InvokeAsync("SubmitToJudge", lobbyKey, submission);
        }

        public void UpdateScore(string lobbyKey, string userName)
        {
            connection.InvokeAsync("UpdateScore", lobbyKey, userName);
        }
    }
}
