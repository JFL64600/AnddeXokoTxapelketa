using AnddeXokoTxapelketa.EventsArgs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class ScoreDialog : UserControl
    {
        #region Declarations
        private int _row = 0;
        private int _column = 0;
        #region Events
        public event EventHandler<ScoreDialogEventArgs>? ExitScoreDialog;
        #endregion
        #endregion
        #region Events
        private void ValidClick(object sender, RoutedEventArgs e)
        {
            Close(false);
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close(true);
        }
        #endregion
        #region Methods
        public void Init(int row, string rowPlayerName, int rowScore, int column, string columnPlayerName, int columnScore)
        {
            _row = row;
            Player1.Value = rowPlayerName;
            Score1.Value = rowScore;
            _column = column;
            Player2.Value = columnPlayerName;
            Score2.Value = columnScore;
        }
        #endregion
        public ScoreDialog()
        {
            InitializeComponent();
        }
        #region Privates
        private void Close(bool canceled)
        {
            ExitScoreDialog?.Invoke(
               this,
               new ScoreDialogEventArgs()
               {
                   Canceled = canceled,
                   Row = _row,
                   Column = _column,
                   ScorePlayer1 = Score1.Value,
                   ScorePlayer2 = Score2.Value,
               });
            ((Popup)Parent).IsOpen = false;
        }
        #endregion
    }
}
