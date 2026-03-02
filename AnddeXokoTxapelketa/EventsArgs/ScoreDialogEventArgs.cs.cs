namespace AnddeXokoTxapelketa.EventsArgs
{
    public class ScoreDialogEventArgs : EventArgs
    {
        #region Properties
        public bool Canceled { get; set; } = true;
        public int Row { get; set; } = 0;
        public int Column { get; set; } = 0;
        public int ScorePlayer1 { get; set; } = 0;
        public int ScorePlayer2 { get; set; } = 0;
        #endregion
    }
}
