using AnddeXokoTxapelketa.Classes;
using AnddeXokoTxapelketa.EventsArgs;
using AnddeXokoTxapelketa.Models;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Text;
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
        public event EventHandler<RankingEventArgs>? OpenRankingEvent;
        public event EventHandler<FinalEventArgs>? OpenFinalTableEvent;
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
        }
        #region Privates
        private void OpenTournamentClick(object sender, RoutedEventArgs e)
        {
            Tournament? tournament = GetTournament(sender);
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
        private void GenerateRotationsClick(object sender, RoutedEventArgs e)
        {
            Tournament? tournament = Tools.CloneTournament(GetTournament(sender));
            if (tournament != null)
            {
                Tools.GenerateRotations(_root, tournament);
            }
        }
        private void OpenRankingClick(object sender, RoutedEventArgs e)
        {
            Tournament? tournament = Tools.CloneTournament(GetTournament(sender));
            if (tournament != null)
            {
                OpenRankingEvent?.Invoke(
                    this,
                    new RankingEventArgs()
                    {
                        Tournament = tournament
                    });
            }
        }
        private void OpenFinalTableClick(object sender, RoutedEventArgs e)
        {
            Tournament? tournament = GetTournament(sender);
            if (tournament != null)
            {
                Leagues leagues = Tools.GetLeagues(_root, tournament.Name);
                if (leagues != null)
                {
                    GeneralRanking generalRanking = Tools.GetGeneralRanking(_root, tournament.Name);
                    if (generalRanking != null)
                    {
                        OpenFinalTableEvent?.Invoke(
                            this,
                            new FinalEventArgs()
                            {
                                TournamentName = tournament.Name,
                                Leagues = leagues,
                                GeneralRanking = generalRanking
                            });
                    }
                }
            }
        }
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            CloseApplicationtEvent?.Invoke(this, new EventArgs());
        }
        private static Tournament? GetTournament(object sender)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                return (Tournament?)button.DataContext;
            }
            return null;
        }
        #endregion
    }
}
