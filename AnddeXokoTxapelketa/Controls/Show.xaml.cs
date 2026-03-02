using AnddeXokoTxapelketa.Classes;
using AnddeXokoTxapelketa.EventsArgs;
using AnddeXokoTxapelketa.Interfaces;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnddeXokoTxapelketa.Controls
{
    public partial class Show : UserControl
    {
        #region Declarations
        private readonly string _root = ConfigurationManager.AppSettings["root"];
        private readonly int _maxPlayers = 10;
        private ITournament _tournament;
        private int _currentGroup = 0;
        private bool _isBoysCurrent = false;
        #region Events
        public event EventHandler? CloseTournamentEvent;
        #endregion
        #endregion
        #region Events
        private void BPreviousClick(object sender, RoutedEventArgs e)
        {
            BPrevious.IsEnabled = true;
            BNext.IsEnabled = true;
            if (_tournament is Models.Tournament)
            {
                if (_isBoysCurrent)
                {
                    if (_currentGroup == 0)
                    {
                        _isBoysCurrent = false;
                        _currentGroup = ((Models.Tournament)_tournament).Girls.Count - 1;
                    }
                    else
                    {
                        _currentGroup--;
                    }
                }
                else
                {
                    _currentGroup--;
                    BPrevious.IsEnabled = _currentGroup != 0;
                }
                SetGroup(_isBoysCurrent ? ((Models.Tournament)_tournament).Boys : ((Models.Tournament)_tournament).Girls);
            }
            else if (_tournament is Models.New.Tournament)
            {
                _currentGroup--;
                BPrevious.IsEnabled = _currentGroup != 0;
                SetGroup(((Models.New.Tournament)_tournament).Groups[_currentGroup]);
            }
        }
        private void BNextClick(object sender, RoutedEventArgs e)
        {
            BPrevious.IsEnabled = true;
            BNext.IsEnabled = true;
            if (_tournament is Models.Tournament)
            {
                if (_isBoysCurrent)
                {
                    _currentGroup++;
                    BNext.IsEnabled = _currentGroup != ((Models.Tournament)_tournament).Boys.Count - 1;
                }
                else
                {
                    if (_currentGroup < ((Models.Tournament)_tournament).Girls.Count - 1)
                    {
                        _currentGroup++;
                    }
                    else
                    {
                        _isBoysCurrent = true;
                        _currentGroup = 0;
                    }
                }
                SetGroup(_isBoysCurrent ? ((Models.Tournament)_tournament).Boys : ((Models.Tournament)_tournament).Girls);
            }
            else if (_tournament is Models.New.Tournament)
            {
                _currentGroup++;
                BNext.IsEnabled = _currentGroup != ((Models.New.Tournament)_tournament).Groups.Count - 1;
                SetGroup(((Models.New.Tournament)_tournament).Groups[_currentGroup]);
            }
        }
        private void PlayerNameExitEvent(object sender, EventArgs e)
        {
            PlayerName playerName = (PlayerName)sender;
            PlayerLabel playerLabel = (PlayerLabel)FindName(playerName.Name.Replace("Name", "Label"));
            playerLabel.Value = playerName.Value;
            _ = int.TryParse(playerName.Name.Last().ToString(), out int index);
            (_isBoysCurrent ? ((Models.Tournament)_tournament).Boys : ((Models.Tournament)_tournament).Girls)[_currentGroup].Players[index - 1].Name = playerName.Value;
            //Tools.SaveTournament(_root, ((Tournament)_tournament));
        }
        private void ExitScoreDialogEvent(object sender, ScoreDialogEventArgs e)
        {
            if (!e.Canceled)
            {
                if (_tournament is Models.Tournament)
                {
                    ((Score)FindName($"ScoreR{e.Row}C{e.Column}")).Value = e.ScorePlayer1;
                    ((Score)FindName($"ScoreR{e.Column}C{e.Row}")).Value = e.ScorePlayer2;
                    (_isBoysCurrent ? ((Models.Tournament)_tournament).Boys : ((Models.Tournament)_tournament).Girls)[_currentGroup].Players[e.Row - 1].Results[e.Column - ((e.Row > e.Column) ? 1 : 2)] = e.ScorePlayer1;
                    (_isBoysCurrent ? ((Models.Tournament)_tournament).Boys : ((Models.Tournament)_tournament).Girls)[_currentGroup].Players[e.Column - 1].Results[e.Row - ((e.Column > e.Row) ? 1 : 2)] = e.ScorePlayer2;
                    Tools.SaveTournament(_root, ((Models.Tournament)_tournament));
                }
                else if (_tournament is Models.New.Tournament)
                {
                    if (e.Row < e.Column)
                    {
                        ((Score)FindName($"ScoreR{e.Row}C{e.Column}")).Value = e.ScorePlayer1;
                        ((Score)FindName($"ScoreR{e.Column}C{e.Row}")).Value = e.ScorePlayer2;
                    }
                    else
                    {
                        ((Score)FindName($"ScoreR{e.Row}C{e.Column}")).Value = e.ScorePlayer2;
                        ((Score)FindName($"ScoreR{e.Column}C{e.Row}")).Value = e.ScorePlayer1;
                    }
                    foreach (Models.New.Rotation rotation in ((Models.New.Tournament)_tournament).Groups[_currentGroup].Rotations)
                    {
                        foreach (Models.New.Match match in rotation.Matches)
                        {
                            if (e.Row < e.Column)
                            {
                                if (match.Scores[0].ID == e.Row && match.Scores[1].ID == e.Column)
                                {
                                    match.Scores[0].Points = e.ScorePlayer1;
                                    match.Scores[1].Points = e.ScorePlayer2;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (match.Scores[0].ID == e.Column && match.Scores[1].ID == e.Row)
                                {
                                    match.Scores[0].Points = e.ScorePlayer2;
                                    match.Scores[1].Points = e.ScorePlayer1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    string filename = string.Empty;
                    switch (((Models.New.Tournament)_tournament).Groups[_currentGroup].Type)
                    {
                        case Tools.GroupType.Boys:
                            filename = $"boys.group.{_currentGroup - 2}.json";
                            break;
                        case Tools.GroupType.Girls:
                            filename = $"girls.group.{_currentGroup + 1}.json";
                            break;
                    }
                    if (!string.IsNullOrWhiteSpace(filename))
                    {
                        using StreamWriter sw = new(Path.Combine(_root, _tournament.Name, filename), false, Encoding.UTF8);
                        sw.Write(System.Text.Json.JsonSerializer.Serialize(((Models.New.Tournament)_tournament).Groups[_currentGroup]));
                    }
                    MessageBox.Show("test");
                }
            }
        }
        private void OpenScore(object sender, MouseButtonEventArgs e)
        {
            Score score = (Score)sender;
            Regex rx = new(@"ScoreR([0-9]{1})C([0-9]{1})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(score.Name);
            if (matches.Count > 0)
            {
                _ = int.TryParse(matches[0].Groups[1].Value, out int row);
                _ = int.TryParse(matches[0].Groups[2].Value, out int column);
                if (_tournament is Models.Tournament)
                {
                    (_isBoysCurrent ? ((Models.Tournament)_tournament).Boys : ((Models.Tournament)_tournament).Girls)[_currentGroup].Players[row - 1].Results[column - ((row > column) ? 1 : 2)] = score.Value;
                    Tools.SaveTournament(_root, (Models.Tournament)_tournament);
                    ScoreDialog scoreDialog = (ScoreDialog)popup.Child;
                    scoreDialog.Init(
                        row,
                        (_isBoysCurrent ? ((Models.Tournament)_tournament).Boys : ((Models.Tournament)_tournament).Girls)[_currentGroup].Players[row - 1].Name,
                        score.Value,
                        column,
                        (_isBoysCurrent ? ((Models.Tournament)_tournament).Boys : ((Models.Tournament)_tournament).Girls)[_currentGroup].Players[column - 1].Name,
                        ((Score)FindName($"ScoreR{column}C{row}")).Value);
                    popup.IsOpen = true;
                }
                else if (_tournament is Models.New.Tournament)
                {
                    int rowPlayerID = 0;
                    int rowScore = 0;
                    int columnPlayerID = 0;
                    int columnScore = 0;
                    foreach (Models.New.Rotation rotation in ((Models.New.Tournament)_tournament).Groups[_currentGroup].Rotations)
                    {
                        foreach (Models.New.Match match in rotation.Matches)
                        {
                            if (row < column)
                            {
                                if (match.Scores[0].ID == row && match.Scores[1].ID == column)
                                {
                                    rowPlayerID = match.Scores[0].ID;
                                    rowScore = score.Value;
                                    columnPlayerID = match.Scores[1].ID;
                                    columnScore = ((Score)FindName($"ScoreR{column}C{row}")).Value;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (match.Scores[0].ID == column && match.Scores[1].ID == row)
                                {
                                    rowPlayerID = match.Scores[0].ID;
                                    rowScore = ((Score)FindName($"ScoreR{column}C{row}")).Value;
                                    columnPlayerID = match.Scores[1].ID;
                                    columnScore = score.Value;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    ScoreDialog scoreDialog = (ScoreDialog)popup.Child;
                    scoreDialog.Init(
                        row,
                        GetPlayerNameFromID(rowPlayerID),
                        rowScore,
                        column,
                        GetPlayerNameFromID(columnPlayerID),
                        columnScore);
                    popup.IsOpen = true;
                }
            }
        }
        private string GetPlayerNameFromID(int playerIDInScore)
        {
            switch (((Models.New.Tournament)_tournament).Groups[_currentGroup].Type)
            {
                case Tools.GroupType.Boys:
                    return ((Models.New.Tournament)_tournament).Boys.FirstOrDefault(c => c.ID == ((Models.New.Tournament)_tournament).Groups[_currentGroup].Players[playerIDInScore - 1]).Name;
                case Tools.GroupType.Girls:
                    return ((Models.New.Tournament)_tournament).Girls.FirstOrDefault(c => c.ID == ((Models.New.Tournament)_tournament).Groups[_currentGroup].Players[playerIDInScore - 1]).Name;
                default:
                    return string.Empty;
            }
        }
        #endregion
        #region Methods
        public void Init(ITournament tournament)
        {
            _tournament = tournament;
            tbTitle.Text = $"{tournament.Name} - Andde Xoko Txapelketa";
            _currentGroup = 0;
            _isBoysCurrent = false;
            BPrevious.IsEnabled = false;
            BNext.IsEnabled = true;
            if (tournament is Models.Tournament)
            {
                SetGroup(((Models.Tournament)_tournament).Girls);
            }
            else if (tournament is Models.New.Tournament)
            {
                SetGroup(((Models.New.Tournament)_tournament).Groups[0]);
            }
        }
        private void SetGroup(List<Models.Group> groups)
        {
            TBGroup.Text = $"{(_isBoysCurrent ? "M" : "N")} {_currentGroup + 1}";
            for (int i = 1; i <= _maxPlayers; i++)
            {
                if (i <= groups[_currentGroup].Players.Count)
                {
                    EnablePlayer(i);
                    ((PlayerName)FindName($"PlayerName{i}")).Value = groups[_currentGroup].Players[i - 1].Name;
                    ((PlayerLabel)FindName($"PlayerLabel{i}")).Value = groups[_currentGroup].Players[i - 1].Name;
                    int index = 0;
                    for (int j = 1; j <= groups[_currentGroup].Players[i - 1].Results.Count + 1; j++)
                    {
                        if (j == i)
                        {
                            continue;
                        }
                        ((Score)FindName($"ScoreR{i}C{j}")).Value = groups[_currentGroup].Players[i - 1].Results[index];
                        index++;
                    }
                }
                else
                {
                    DisablePlayer(i);
                }
            }
        }
        private void SetGroup(Models.New.Group group)
        {
            TBGroup.Text = group.Name;
            List<Models.New.Player> players = (group.Type == Tools.GroupType.Boys) ?
                ((Models.New.Tournament)_tournament).Boys : ((Models.New.Tournament)_tournament).Girls;
            for (int i = 1; i <= _maxPlayers; i++)
            {
                if (i > group.Players.Count)
                {
                    DisablePlayer(i);
                    continue;
                }
                Models.New.Player player = players.FirstOrDefault(c => c.ID == group.Players[i - 1]);
                if (player != null && !string.IsNullOrEmpty(player.Name))
                {
                    EnablePlayer(i);
                    ((PlayerName)FindName($"PlayerName{i}")).Value = player.Name;
                    ((PlayerLabel)FindName($"PlayerLabel{i}")).Value = player.Name;
                }
                else
                {
                    DisablePlayer(i);
                }
            }
            foreach (Models.New.Rotation rotation in group.Rotations)
            {
                foreach (Models.New.Match match in rotation.Matches)
                {
                    int id1 = match.Scores[0].ID;
                    int id2 = match.Scores[1].ID;
                    Score score = (Score)FindName($"ScoreR{id1}C{id2}");
                    if (score.CZ.IsEnabled)
                    {
                        score.Value = match.Scores[0].Points;
                    }
                    score = (Score)FindName($"ScoreR{id2}C{id1}");
                    if (score.CZ.IsEnabled)
                    {
                        score.Value = match.Scores[1].Points;
                    }
                }
            }
        }
        #endregion
        public Show()
        {
            InitializeComponent();
        }
        #region Privates
        private void EnablePlayer(int index)
        {
            ((PlayerName)FindName($"PlayerName{index}")).Enable();
            ((PlayerLabel)FindName($"PlayerLabel{index}")).Enable();
            ((Diagonal)FindName($"Diagonal{index}")).Enable();
            for (int i = 1; i <= _maxPlayers; i++)
            {
                if (i == index)
                {
                    continue;
                }
                ((Score)FindName($"ScoreR{index}C{i}")).Enable();
                ((Score)FindName($"ScoreR{i}C{index}")).Enable();
            }
        }
        private void DisablePlayer(int index)
        {
            ((PlayerName)FindName($"PlayerName{index}")).Disable();
            ((PlayerLabel)FindName($"PlayerLabel{index}")).Disable();
            ((Diagonal)FindName($"Diagonal{index}")).Disable();
            for (int i = 1; i <= _maxPlayers; i++)
            {
                if (i == index)
                {
                    continue;
                }
                ((Score)FindName($"ScoreR{index}C{i}")).Disable();
                ((Score)FindName($"ScoreR{i}C{index}")).Disable();
            }
        }
        private void CloseTournamentClick(object sender, RoutedEventArgs e)
        {
            CloseTournamentEvent?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
