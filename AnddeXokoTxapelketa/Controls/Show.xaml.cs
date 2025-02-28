using AnddeXokoTxapelketa.Classes;
using AnddeXokoTxapelketa.EventsArgs;
using AnddeXokoTxapelketa.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class Show : UserControl
    {
        #region Declarations
        private readonly string _root = ConfigurationManager.AppSettings["root"];
        private Tournament _tournament;
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
            if (_isBoysCurrent)
            {
                if (_currentGroup == 0)
                {
                    _isBoysCurrent = false;
                    _currentGroup = _tournament.Girls.Count - 1;
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
            SetGroup(_isBoysCurrent ? _tournament.Boys : _tournament.Girls);
        }
        private void BNextClick(object sender, RoutedEventArgs e)
        {
            BPrevious.IsEnabled = true;
            BNext.IsEnabled = true;
            if (_isBoysCurrent)
            {
                _currentGroup++;
                BNext.IsEnabled = _currentGroup != _tournament.Boys.Count - 1;
            }
            else
            {
                if (_currentGroup < _tournament.Girls.Count - 1)
                {
                    _currentGroup++;
                }
                else
                {
                    _isBoysCurrent = true;
                    _currentGroup = 0;
                }
            }
            SetGroup(_isBoysCurrent ? _tournament.Boys : _tournament.Girls);
        }
        private void PlayerNameExitEvent(object sender, EventArgs e)
        {
            PlayerName playerName = (PlayerName)sender;
            PlayerLabel playerLabel = (PlayerLabel)FindName(playerName.Name.Replace("Name", "Label"));
            playerLabel.Value = playerName.Value;
            _ = int.TryParse(playerName.Name.Last().ToString(), out int index);
            (_isBoysCurrent ? _tournament.Boys : _tournament.Girls)[_currentGroup].Players[index - 1].Name = playerName.Value;
            Tools.SaveTournament(_root, _tournament);
        }
        private void ExitScoreDialogEvent(object sender, ScoreDialogEventArgs e)
        {
            if (!e.Canceled)
            {
                ((Score)FindName($"ScoreR{e.Row}C{e.Column}")).Value = e.ScorePlayer1;
                ((Score)FindName($"ScoreR{e.Column}C{e.Row}")).Value = e.ScorePlayer2;
                (_isBoysCurrent ? _tournament.Boys : _tournament.Girls)[_currentGroup].Players[e.Row - 1].Results[e.Column - ((e.Row > e.Column) ? 1 : 2)] = e.ScorePlayer1;
                (_isBoysCurrent ? _tournament.Boys : _tournament.Girls)[_currentGroup].Players[e.Column - 1].Results[e.Row - ((e.Column > e.Row) ? 1 : 2)] = e.ScorePlayer2;
                Tools.SaveTournament(_root, _tournament);
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
                //(_isBoysCurrent ? _tournament.Boys : _tournament.Girls)[_currentGroup].Players[row - 1].Results[column - ((row > column) ? 1 : 2)] = score.Value;
                //Tools.SaveTournament(_root, _tournament);
                ScoreDialog scoreDialog = (ScoreDialog)popup.Child;
                scoreDialog.Init(
                    row,
                    (_isBoysCurrent ? _tournament.Boys : _tournament.Girls)[_currentGroup].Players[row - 1].Name,
                    score.Value,
                    column,
                    (_isBoysCurrent ? _tournament.Boys : _tournament.Girls)[_currentGroup].Players[column - 1].Name,
                    ((Score)FindName($"ScoreR{column}C{row}")).Value);
                popup.IsOpen = true;
            }
        }
        #endregion
        #region Methods
        public void Init(Tournament tournament)
        {
            _tournament = tournament;
            tbTitle.Text = $"{tournament.Name} - Andde Xoko Txapelketa";
            _currentGroup = 0;
            _isBoysCurrent = false;
            BPrevious.IsEnabled = false;
            BNext.IsEnabled = true;
            SetGroup(_tournament.Girls);
        }
        private void SetGroup(List<Models.Group> groups)
        {
            TBGroup.Text = $"{(_isBoysCurrent ? "M" : "N")} {_currentGroup + 1}";
            for (int i = 1; i <= 8; i++)
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
            for (int i = 1; i <= 8; i++)
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
            for (int i = 1; i <= 8; i++)
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
