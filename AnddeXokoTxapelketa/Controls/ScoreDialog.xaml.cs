using AnddeXokoTxapelketa.Classes;
using AnddeXokoTxapelketa.Models;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using static System.Formats.Asn1.AsnWriter;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class ScoreDialog : UserControl
    {
        #region Declarations
        private Score _score;
        #endregion
        #region Events
        private void ValidClick(object sender, RoutedEventArgs e)
        {
            _score.Value = 3;
            Close(false);
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close( true);
        }
        #endregion
        #region Properties
        public bool Canceled { get; private set; } = true;
        #endregion
        #region Methods
        public void Init(Score score, Player playerRow, Player playerColumn)
        {
            _score = score;
            PlayerLabel1.Text = playerRow.Name;
        }
        #endregion
        public ScoreDialog()
        {
            InitializeComponent();
        }
        #region Privates
        private void Close(bool canceled) {
            Canceled = canceled;
            ((Popup)Parent).IsOpen = false;
        }
        #endregion
    }
}
