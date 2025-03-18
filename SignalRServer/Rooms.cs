namespace SignalRServer
{
    internal class Rooms
    {
        string last = string.Empty;

        internal void AddNewClient(string nickname)
        {
            if (string.IsNullOrEmpty(last))
                last = nickname;
            else
            {
                StartNewGame(last, nickname);
                last = string.Empty;
            }
        }

        Action<string, string, string> proc;
        internal void SetStart(Action<string, string, string> proc)
        {
            this.proc = proc;
        }

        Dictionary<string, Game> games = new();
        private void StartNewGame(string x, string y)
        {
            Random random = new Random();
            if (random.Next(100) > 50)
            {
                string z = y;
                y = x;
                x = z;
            }
            var game = new Game { ID = Guid.NewGuid().ToString(), P1 = x, P2 = y, Turn = "x" };
            games.Add(game.ID, game);
            proc(x, y, game.ID);
        }

        internal string GetNextPlayer(Turn turn)
        {
            string result = string.Empty;
            if (games[turn.GameId].Turn == "x")
            {
                games[turn.GameId].Turn = "o";
                result = games[turn.GameId].P2;
            }
            else
            {
                games[turn.GameId].Turn = "x";
                result = games[turn.GameId].P1;
            }

            return result;
        }

        internal string MakeTurn(Turn turn)
        {
            string result = string.Empty;
            // b_1_1
            var cols = turn.Button.Split('_');
            int row = int.Parse(cols[1]);
            int col = int.Parse(cols[2]);
            games[turn.GameId].Field[row, col] = turn.Char;

            bool winner = Winner(games[turn.GameId].Field, turn.Char);
            bool hasTurns = false;
            foreach (var cell in games[turn.GameId].Field)
                if (string.IsNullOrEmpty(cell))
                {
                    hasTurns = true;
                    break;
                }
            if (!winner && hasTurns)
                return "next";
            if (!hasTurns)
                return "nobody";
            else
                return "wins";
        }

        public bool Winner(string[,] array, string find)
        {
            for (int row = 0; row < 3; row++)
            {
                if (array[row, 0] == find &&
                    array[row, 1] == find &&
                    array[row, 2] == find)
                {
                    return true;
                }
            }

            for (int col = 0; col < 3; col++)
            {
                if (array[0, col] == find &&
                    array[1, col] == find &&
                    array[2, col] == find)
                {
                    return true;
                }
            }

            if ((array[0, 0] == find &&
                    array[1, 1] == find &&
                    array[2, 2] == find) ||
                (array[0, 2] == find &&
                    array[1, 1] == find &&
                    array[2, 0] == find))
                return true;
            return false;
        }

        internal string GetChar(Turn turn)
        {
            return games[turn.GameId].Turn;
        }

        internal void ClearGame(string gameId)
        {
            games.Remove(gameId);
        }
    }
}
