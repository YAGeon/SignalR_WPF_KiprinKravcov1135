namespace SignalRServer
{
    public class Game
    {
        public string ID { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string Turn { get; set; }

        public string[,] Field { get; set; } = new string[3, 3];
    }
}
