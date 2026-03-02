using AnddeXokoTxapelketa.Interfaces;
using AnddeXokoTxapelketa.Models.New;

namespace AnddeXokoTxapelketa.EventsArgs.New
{
    public class FinalEventArgs : EventArgs, IFinalEventArgs
    {
        #region Properties
        public string TournamentName { get; set; } = string.Empty;
        public Leagues Leagues { get; set; } = new();
        #endregion
    }
}
