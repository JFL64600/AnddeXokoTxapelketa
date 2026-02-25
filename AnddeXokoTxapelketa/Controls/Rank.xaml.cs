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
                int i = 1;
                foreach (Models.New.Group group in ((Models.New.Tournament)tournament).Groups)
                {
                    List<Models.New.SortablePlayer> players = [];
                    foreach (Models.New.Rotation rotation in group.Rotations)
                    {
                        foreach (Models.New.Match match in rotation.Matches)
                        {
                            if (match.Scores[0].Points != 3 && match.Scores[1].Points != 3)
                            {
                                continue;
                            }
                            if (match.Scores[0].Points == 3)
                            {
                                Models.New.SortablePlayer player = players.FirstOrDefault(c => c.ID == match.Scores[0].ID);
                                if (player == null)
                                {
                                    player = new()
                                    {
                                        ID = match.Scores[0].ID,
                                        Name = GetPlayerNameFromID((Models.New.Tournament)tournament, group, match.Scores[0].ID)
                                    };
                                    players.Add(player);
                                }
                                player.Victories += 1;
                                player.ConcededPoints += match.Scores[1].Points;
                                player = players.FirstOrDefault(c => c.ID == match.Scores[1].ID);
                                if (player == null)
                                {
                                    player = new()
                                    {
                                        ID = match.Scores[1].ID,
                                        Name = GetPlayerNameFromID((Models.New.Tournament)tournament, group, match.Scores[1].ID)
                                    };
                                    players.Add(player);
                                }
                                player.Points += match.Scores[1].Points;
                            }
                            else if (match.Scores[1].Points == 3)
                            {
                                Models.New.SortablePlayer player = players.FirstOrDefault(c => c.ID == match.Scores[1].ID);
                                if (player == null)
                                {
                                    player = new()
                                    {
                                        ID = match.Scores[1].ID,
                                        Name = GetPlayerNameFromID((Models.New.Tournament)tournament, group, match.Scores[1].ID)
                                    };
                                    players.Add(player);
                                }
                                player.Victories += 1;
                                player.ConcededPoints += match.Scores[0].Points;
                                player = players.FirstOrDefault(c => c.ID == match.Scores[0].ID);
                                if (player == null)
                                {
                                    player = new()
                                    {
                                        ID = match.Scores[0].ID,
                                        Name = GetPlayerNameFromID((Models.New.Tournament)tournament, group, match.Scores[0].ID)
                                    };
                                    players.Add(player);
                                }
                                player.Points += match.Scores[0].Points;
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
        private string GetPlayerNameFromID(Models.New.Tournament tournament, Models.New.Group group, int playerIDInScore)
        {
            switch (group.Type)
            {
                case Tools.GroupType.Boys:
                    return tournament.Boys.FirstOrDefault(c => c.ID == group.Players[playerIDInScore - 1]).Name;
                case Tools.GroupType.Girls:
                    return tournament.Girls.FirstOrDefault(c => c.ID == group.Players[playerIDInScore - 1]).Name;
                default:
                    return string.Empty;
            }
        }
        private void CloseRankClick(object sender, RoutedEventArgs e)
        {
            CloseRankEvent?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
