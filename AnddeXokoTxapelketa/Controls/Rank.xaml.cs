using AnddeXokoTxapelketa.Classes;
using AnddeXokoTxapelketa.Interfaces;
using AnddeXokoTxapelketa.Models;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AnddeXokoTxapelketa.Controls
{
    public partial class Rank : UserControl
    {
        #region Declarations
        private readonly string _root = ConfigurationManager.AppSettings["root"];
        #region Events
        public event EventHandler? CloseRankEvent;
        #endregion
        #endregion
        #region Methods
        public void Init(ITournament tournament)
        {
            tbTitle.Text = $"{tournament.Name} - Andde Xoko Txapelketa";
            if (tournament is Tournament)
            {
                GeneralRanking generalRanking = new();
                for (int i = 0; i < ((Tournament)tournament).Girls.Count; i++)
                {
                    ((Tournament)tournament).Girls[i].Players.Sort();
                    string groupName = $"N{i + 1}";
                    ((RankGroup)FindName(groupName)).SetGroup(groupName, ((Tournament)tournament).Girls[i]);
                    int rank = 1;
                    foreach (Player player in ((Tournament)tournament).Girls[i].Players)
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
                for (int i = 0; i < ((Tournament)tournament).Boys.Count; i++)
                {
                    ((Tournament)tournament).Boys[i].Players.Sort();
                    string groupName = $"M{i + 1}";
                    ((RankGroup)FindName(groupName)).SetGroup(groupName, ((Tournament)tournament).Boys[i]);
                    int rank = 1;
                    foreach (Player player in ((Tournament)tournament).Boys[i].Players)
                    {
                        if (string.IsNullOrWhiteSpace(player.Name))
                        {
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
            else if (tournament is Models.New.Tournament)
            {
                Tools.GroupType savedGroupType = Tools.GroupType.Girls;
                int i = 1;
                foreach (Models.New.Group group in ((Models.New.Tournament)tournament).Groups.OrderBy(c => c.Type).ThenBy(c => c.Name))
                {
                    if (savedGroupType != group.Type)
                    {
                        savedGroupType = group.Type;
                        i = 1;
                    }
                    List<Models.New.SortablePlayer> players = [];
                    foreach (Models.New.Rotation rotation in group.Rotations)
                    {
                        foreach (Models.New.Match match in rotation.Matches)
                        {
                            Models.New.Player player1 = GetPlayerFromID((Models.New.Tournament)tournament, group, match.Scores[0].ID);
                            Models.New.SortablePlayer sortablePlater = players.FirstOrDefault(c => c.ID == match.Scores[0].ID);
                            if (sortablePlater == null)
                            {
                                sortablePlater = new()
                                {
                                    ID = match.Scores[0].ID,
                                    Name = player1.Name
                                };
                                players.Add(sortablePlater);
                            }
                            Models.New.Player player2 = GetPlayerFromID((Models.New.Tournament)tournament, group, match.Scores[1].ID);
                            sortablePlater = players.FirstOrDefault(c => c.ID == match.Scores[1].ID);
                            if (sortablePlater == null)
                            {
                                sortablePlater = new()
                                {
                                    ID = match.Scores[1].ID,
                                    Name = player2.Name
                                };
                                players.Add(sortablePlater);
                            }
                            if (player1 is null || string.IsNullOrWhiteSpace(player1.Name) || player2 is null || string.IsNullOrWhiteSpace(player2.Name))
                            {
                                continue;
                            }
                            if (match.Scores[0].Points == 3)
                            {
                                Models.New.SortablePlayer player = players.FirstOrDefault(c => c.ID == match.Scores[0].ID);
                                player.Victories += 1;
                                player.PointsAll += match.Scores[0].Points;
                                player.PointsConceded += match.Scores[1].Points;
                                player = players.FirstOrDefault(c => c.ID == match.Scores[1].ID);
                                player.PointsAll += match.Scores[1].Points;
                                player.PointsInDefeats += match.Scores[1].Points;
                            }
                            else if (match.Scores[1].Points == 3)
                            {
                                Models.New.SortablePlayer player = players.FirstOrDefault(c => c.ID == match.Scores[1].ID);
                                player.PointsAll += match.Scores[1].Points;
                                player.Victories += 1;
                                player.PointsConceded += match.Scores[0].Points;
                                player = players.FirstOrDefault(c => c.ID == match.Scores[0].ID);
                                player.PointsAll += match.Scores[0].Points;
                                player.PointsInDefeats += match.Scores[0].Points;
                            }
                        }
                    }
                    string filename = string.Empty;
                    switch (group.Type)
                    {
                        case Tools.GroupType.Boys:
                            filename = $"boys.rank.{i}.json";
                            break;
                        case Tools.GroupType.Girls:
                            filename = $"girls.rank.{i}.json";
                            break;
                    }
                    players.Sort();
                    Tools.SetObjects(players, Path.Combine(_root, tournament.Name, filename));
                    i++;
                    ((RankGroup)FindName(group.Name.Replace(" ", string.Empty))).SetGroup(group.Name, players);
                }
            }
        }
        #endregion
        public Rank()
        {
            InitializeComponent();
        }
        #region Privates
        private Models.New.Player GetPlayerFromID(Models.New.Tournament tournament, Models.New.Group group, int playerIDInScore)
        {
            switch (group.Type)
            {
                case Tools.GroupType.Boys:
                    return tournament.Boys.FirstOrDefault(c => c.ID == group.Players[playerIDInScore - 1]);
                case Tools.GroupType.Girls:
                    return tournament.Girls.FirstOrDefault(c => c.ID == group.Players[playerIDInScore - 1]);
                default:
                    return null;
            }
        }
        private void CloseRankClick(object sender, RoutedEventArgs e)
        {
            CloseRankEvent?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
