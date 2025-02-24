using AnddeXokoTxapelketa.Classes;
using AnddeXokoTxapelketa.Models;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class Rank : UserControl
    {
        #region Declarations
        private readonly string _root = ConfigurationManager.AppSettings["root"];
        #region Events
        public event EventHandler? CloseRankEvent;
        #endregion
        #endregion
        #region Methods
        public void Init(Tournament tournament)
        {
            GeneralRanking generalRanking = new();
            tbTitle.Text = $"{tournament.Name} - Andde Xoko Txapelketa";
            for (int i = 0; i < tournament.Girls.Count; i++)
            {
                tournament.Girls[i].Players.Sort();
                string groupName = $"N{i + 1}";
                ((RankGroup)FindName(groupName)).SetGroup(groupName, tournament.Girls[i]);
                int rank = 1;
                foreach (Player player in tournament.Girls[i].Players)
                {
                    if (string.IsNullOrWhiteSpace(player.Name))
                    {
                        continue;
                    }
                    generalRanking.Girls.Add(new PlayerGeneral
                    {
                        Name = player.Name,
                        Results = player.Results,
                        Position = rank,
                        Group = groupName
                    });
                    rank++;
                }
            }
            for (int i = 0; i < tournament.Boys.Count; i++)
            {
                tournament.Boys[i].Players.Sort();
                string groupName = $"M{i + 1}";
                ((RankGroup)FindName(groupName)).SetGroup(groupName, tournament.Boys[i]);
                int rank = 1;
                foreach (Player player in tournament.Boys[i].Players)
                {
                    if (string.IsNullOrWhiteSpace(player.Name)) {
                        continue;
                    }
                    generalRanking.Boys.Add(new PlayerGeneral
                    {
                        Name = player.Name,
                        Results = player.Results,
                        Position = rank,
                        Group = groupName
                    });
                    rank++;
                }
            }
            Tools.SaveGeneralRanking(_root, tournament.Name, generalRanking);
        }
        #endregion
        public Rank()
        {
            InitializeComponent();
        }
        #region Privates
        private void CloseRankClick(object sender, RoutedEventArgs e)
        {
            CloseRankEvent?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
