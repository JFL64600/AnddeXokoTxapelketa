using AnddeXokoTxapelketa.Models;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class Show : UserControl
    {
        #region Declarations
        private Tournament _tournament;
        #region Events
        public event EventHandler? CloseTournamentEvent;
        #endregion
        #endregion
        #region Events
        private void BPreviousClick(object sender, RoutedEventArgs e)
        {
            //_currentGroup--;
            //ShowGroup();
        }
        private void BNextClick(object sender, RoutedEventArgs e)
        {
            //_currentGroup++;
            //ShowGroup();
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            TextBlock tbLabel = (TextBlock)FindName($"{tb.Name}Label");
            tbLabel.Text = tb.Text;
        }
        #endregion
        public Show()
        {
            InitializeComponent();

        }
        #region Methods
        public void SetTournament(Tournament tournament)
        {
            _tournament = tournament;
            tbTitle.Text = $"{tournament.Name} - Andde Xoko Txapelketa";
        }
        #endregion
        #region Privates
        private void CloseTournamentClick(object sender, RoutedEventArgs e)
        {
            CloseTournamentEvent?.Invoke(this, new EventArgs());
            //EnablePlayer(8);
        }
        #endregion

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                DisablePlayer(8);
            }
        }
        private void DisablePlayer(int index)
        {
            DisablePlayer($"mdCZ{index}0"); //Player
            TextBox tb = (TextBox)FindName($"TBPlayer{index}");
            tb.Visibility = Visibility.Hidden;
            DisablePlayer($"mdCZ0{index}"); //Player label
            TextBlock tbLabel = (TextBlock)FindName($"TBPlayer{index}Label");
            tbLabel.Visibility = Visibility.Hidden;
            DisablePlayerDiagonal(index); //No result
        }
        private void DisablePlayer(string name)
        {
            ColorZone cz = (ColorZone)FindName(name);
            cz.IsEnabled = false;
            cz.Background = Brushes.Beige;
        }
        private void DisablePlayerDiagonal(int index)
        {
            ColorZone cz = (ColorZone)FindName($"mdCZ{index}{index}");
            cz.IsEnabled = false;
            cz.Mode = ColorZoneMode.PrimaryLight;
        }
        private void EnablePlayer(int index)
        {
            EnablePlayer($"mdCZ{index}0"); //Player
            TextBox tb = (TextBox)FindName($"TBPlayer{index}");
            tb.Visibility = Visibility.Visible;
            EnablePlayer($"mdCZ0{index}"); //Player label
            TextBlock tbLabel = (TextBlock)FindName($"TBPlayer{index}Label");
            tbLabel.Visibility = Visibility.Visible;
            EnablePlayerDiagonal(index); // No result
        }
        private void EnablePlayer(string name)
        {
            ColorZone cz = (ColorZone)FindName(name);
            cz.IsEnabled = true;
            cz.ClearValue(BackgroundProperty);
            cz.Mode = ColorZoneMode.Standard;
        }
        private void EnablePlayerDiagonal(int index)
        {
            ColorZone cz = (ColorZone)FindName($"mdCZ{index}{index}");
            cz.IsEnabled = true;
            cz.Mode = ColorZoneMode.PrimaryMid;
        }
    }
}
