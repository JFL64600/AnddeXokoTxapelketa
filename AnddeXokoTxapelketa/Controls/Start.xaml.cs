using AnddeXokoTxapelketa.Classes;
using AnddeXokoTxapelketa.EventsArgs;
using AnddeXokoTxapelketa.Models;
using Newtonsoft.Json;
using System.Configuration;
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
        private void OpenRankingClick(object sender, RoutedEventArgs e)
        {
            Tournament? tournament = Oups(GetTournament(sender));
            if (tournament != null)
            {
                string path = Path.Combine(_root, tournament.Name);
                using (StreamWriter sw = new(Path.Combine(path, "rankingN.txt"), false, Encoding.UTF8))
                {
                    List<PlayerGeneral> general = [];
                    for (int i = 0; i < tournament.Girls.Count; i++)
                    {
                        tournament.Girls[i].Players.Sort();
                        sw.WriteLine($"N{i + 1}");
                        int rank = 1;
                        foreach (Player player in tournament.Girls[i].Players)
                        {
                            general.Add(new PlayerGeneral
                            {
                                Name = player.Name,
                                Results = player.Results,
                                Position = rank,
                                Group = $"N{i + 1}"
                            });
                            sw.WriteLine($"{rank}. {player.Name}");
                            rank++;
                        }
                    }
                    general.Sort();
                    sw.WriteLine();
                    sw.WriteLine("Général");
                    int rankGeneral = 1;
                    foreach (PlayerGeneral player in general)
                    {
                        sw.WriteLine($"{rankGeneral}. {player.Name} ({player.Group})");
                        rankGeneral++;
                    }
                }

                using (StreamWriter sw = new(Path.Combine(path, "rankingM.txt"), false, Encoding.UTF8))
                {
                    List<PlayerGeneral> general = [];
                    for (int i = 0; i < tournament.Boys.Count; i++)
                    {
                        tournament.Boys[i].Players.Sort();
                        sw.WriteLine($"M{i + 1}");
                        int rank = 1;
                        foreach (Player player in tournament.Boys[i].Players)
                        {
                            general.Add(new PlayerGeneral
                            {
                                Name = player.Name,
                                Results = player.Results,
                                Position = rank,
                                Group = $"M{i + 1}"
                            });
                            sw.WriteLine($"{rank}. {player.Name}");
                            rank++;
                        }
                    }
                    general.Sort();
                    sw.WriteLine();
                    sw.WriteLine("Général");
                    int rankGeneral = 1;
                    foreach (PlayerGeneral player in general)
                    {
                        sw.WriteLine($"{rankGeneral}. {player.Name} ({player.Group})");
                        rankGeneral++;
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
        private static Tournament Oups(Tournament tournament)
        {
            var serialized = JsonConvert.SerializeObject(tournament);
            return JsonConvert.DeserializeObject<Tournament>(serialized);

        }
    }
}
