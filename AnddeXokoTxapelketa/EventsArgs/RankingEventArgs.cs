using AnddeXokoTxapelketa.Interfaces;

namespace AnddeXokoTxapelketa.EventsArgs
{
    public class RankingEventArgs : EventArgs
    {
        #region Properties
        public ITournament Tournament { get; set; }
        #endregion
    }
}
