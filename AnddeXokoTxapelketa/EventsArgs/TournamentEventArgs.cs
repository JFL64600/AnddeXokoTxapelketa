using AnddeXokoTxapelketa.Interfaces;

namespace AnddeXokoTxapelketa.EventsArgs
{
    public class TournamentEventArgs : EventArgs
    {
        public ITournament Tournament { get; set; }
    }
}
