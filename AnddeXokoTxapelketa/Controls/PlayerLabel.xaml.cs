using System.Windows;
using System.Windows.Controls;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour PlayerLabel.xaml
    /// </summary>
    public partial class PlayerLabel : UserControl
    {
        #region Properties
        public string Value
        {
            set { Player.Text = value; }
        }
        #endregion
        public PlayerLabel()
        {
            InitializeComponent();
        }
        #region Methods
        public void Enable()
        {
            CZ.IsEnabled = true;
            Player.Visibility = Visibility.Visible;
        }
        public void Disable()
        {
            CZ.IsEnabled = false;
            Player.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
