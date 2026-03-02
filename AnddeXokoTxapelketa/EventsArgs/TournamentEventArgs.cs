using AnddeXokoTxapelketa.Interfaces;

namespace AnddeXokoTxapelketa.EventsArgs
{
    public class TournamentEventArgs : EventArgs
    {
        #region Properties
        public ITournament Tournament { get; set; }
        #endregion
    }
}
