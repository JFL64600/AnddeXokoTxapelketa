using System.Windows.Controls;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Diagonal.xaml
    /// </summary>
    public partial class RankPlayerLabel : UserControl
    {
        #region Properties
        public string Value
        {
            get { return Player.Text; }
            set { Player.Text = value; }
        }
        #endregion
        public RankPlayerLabel()
        {
            InitializeComponent();
        }
    }
}
