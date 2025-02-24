using AnddeXokoTxapelketa.Models;

namespace AnddeXokoTxapelketa.EventsArgs
{
    public class FinalEventArgs : EventArgs
    {
        public string TournamentName { get; set; } = string.Empty;
        public Leagues Leagues { get; set; } = new();
        public GeneralRanking GeneralRanking { get; set; } = new();
    }
}
