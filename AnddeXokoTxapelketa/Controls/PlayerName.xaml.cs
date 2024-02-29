using System.Windows;
using System.Windows.Controls;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour PlayerName.xaml
    /// </summary>
    public partial class PlayerName : UserControl
    {
        #region Declarations
        #region Events
        public event EventHandler? ExitEvent;
        #endregion
        #endregion
        #region Properties
        public string Value
        {
            get { return Player.Text; }
            set { Player.Text = value; }
        }
        #endregion
        #region Events
        private new void LostFocus(object sender, RoutedEventArgs e)
        {
            ExitEvent?.Invoke(this, new EventArgs());
        }
        #endregion
        public PlayerName()
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
