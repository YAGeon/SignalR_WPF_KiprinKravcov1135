using Microsoft.AspNetCore.SignalR;

namespace SignalRServer
{
    internal class MyHub : Hub
    {
        public MyHub(Rooms rooms)
        {
            this.rooms = rooms;
            rooms.SetStart(async (x, y, id) => {
                await clientsByNickname[x].SendAsync("opponent", y, id);
                await clientsByNickname[y].SendAsync("opponent", x, id);
                await clientsByNickname[x].SendAsync("maketurn", "x");
            });
        }

        static Dictionary<string, ISingleClientProxy> clientsByNickname = new();
        private readonly Rooms rooms;

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("hello", "Придумай ник");
            Console.WriteLine("Новенький");
            return base.OnConnectedAsync();
        }

        public void Nickname(string nickname)
        {
            var check = clientsByNickname.Keys.FirstOrDefault(s => s == nickname);
            if (check != null)
            {
                Clients.Caller.SendAsync("hello", "Придумай другой ник");
                return;
            }
            else
            {
                clientsByNickname.Add(nickname, Clients.Caller);
                rooms.AddNewClient(nickname);
            }
        }

        public async void MakeTurn(Turn turn)
        {
            string next = rooms.GetNextPlayer(turn);
            string turnResult = rooms.MakeTurn(turn);
            await clientsByNickname[next].SendAsync("opponent_turn", turn);
            if (turnResult == "next")
            {
                string nextchar = rooms.GetChar(turn);
                await clientsByNickname[next].SendAsync("maketurn", nextchar);
            }
            else if (turnResult == "nobody")
            {
                string first = rooms.GetNextPlayer(turn);
                await clientsByNickname[first].SendAsync("gameresult", turnResult);
                await clientsByNickname[next].SendAsync("gameresult", turnResult);
                rooms.ClearGame(turn.GameId);
            }
            else if (turnResult == "wins")
            {
                string first = rooms.GetNextPlayer(turn);
                await clientsByNickname[first].SendAsync("gameresult", "win");
                await clientsByNickname[next].SendAsync("gameresult", "lose");
                rooms.ClearGame(turn.GameId);
            }
        }

        public void NextGame(string answer, string nick)
        {
            if (answer == "yeap")
            {
                rooms.AddNewClient(nick);
            }
            else
            {
                clientsByNickname.Remove(nick);
            }
        }
    }
}
