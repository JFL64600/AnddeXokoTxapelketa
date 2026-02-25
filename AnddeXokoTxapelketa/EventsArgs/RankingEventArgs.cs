using AnddeXokoTxapelketa.Interfaces;

namespace AnddeXokoTxapelketa.EventsArgs
{
    public class RankingEventArgs : EventArgs
    {
        public ITournament Tournament { get; set; }
    }
}
