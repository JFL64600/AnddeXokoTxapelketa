using AnddeXokoTxapelketa.Models;

namespace AnddeXokoTxapelketa.EventsArgs
{
    public class RankingEventArgs : EventArgs
    {
        public Tournament Tournament { get; set; } = new();
    }
}
