using AnddeXokoTxapelketa.Classes;
using AnddeXokoTxapelketa.EventsArgs;
using AnddeXokoTxapelketa.Models;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class Start : UserControl
    {
        #region Declarations
        private readonly string _root = ConfigurationManager.AppSettings["root"];
        #region Events
        public event EventHandler<TournamentEventArgs>? OpenTournamentEvent;
        public event EventHandler? CloseApplicationtEvent;
        #endregion
        #endregion
        #region Events
        private void UCIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                lvTournaments.ItemsSource = Tools.GetTournaments(_root);
            }
        }
        #endregion
        public Start()
        {
            InitializeComponent();
            List<Tournament> items = new()
            {
                new Tournament() { Name = "Neskak" },
                new Tournament() { Name = "Mutikoak" }
            };
            lvTournaments.ItemsSource = items;
        }
        private void OpenTournamentClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                Tournament tournament = (Tournament)button.DataContext;
                if (tournament != null)
                {
                    OpenTournamentEvent?.Invoke(
                        this,
                        new TournamentEventArgs()
                        {
                            Tournament = tournament
                        });
                }
            }
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            CloseApplicationtEvent?.Invoke(this, new EventArgs());
        }
    }
}
