using AnddeXokoTxapelketa.Models;

namespace AnddeXokoTxapelketa.EventsArgs
{
    public class FinalEventArgs : EventArgs
    {
        public string TournamentName { get; set; } = string.Empty;
        public List<PlayerGeneral> GeneralRanking { get; set; } = [];
    }
}
