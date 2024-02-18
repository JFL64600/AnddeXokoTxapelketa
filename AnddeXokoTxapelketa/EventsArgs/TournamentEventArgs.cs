using AnddeXokoTxapelketa.Models;

namespace AnddeXokoTxapelketa.EventsArgs
{
    public class TournamentEventArgs : EventArgs
    {
        public Tournament Tournament { get; set; } = new();
    }
}
