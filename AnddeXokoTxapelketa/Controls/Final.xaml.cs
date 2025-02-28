using AnddeXokoTxapelketa.EventsArgs;
using AnddeXokoTxapelketa.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class Final : UserControl
    {
        #region Declarations
        private readonly List<FinalTable> _finalTables = [];
        private int _currentFinalTable = 0;
        #region Events
        public event EventHandler? CloseFinalTableEvent;
        #endregion
        #endregion
        #region Events
        private void BPreviousClick(object sender, RoutedEventArgs e)
        {
            BPrevious.IsEnabled = true;
            BNext.IsEnabled = true;
            _currentFinalTable--;
            BPrevious.IsEnabled = _currentFinalTable != 0;
            ShowFinalTable(_finalTables[_currentFinalTable]);
        }
        private void BNextClick(object sender, RoutedEventArgs e)
        {
            BPrevious.IsEnabled = true;
            BNext.IsEnabled = true;
            _currentFinalTable++;
            BNext.IsEnabled = _currentFinalTable != _finalTables.Count - 1;
            ShowFinalTable(_finalTables[_currentFinalTable]);
        }
        #endregion
        #region Methods
        public void Init(FinalEventArgs e)
        {
            tbTitle.Text = $"{e.TournamentName} - Andde Xoko Txapelketa";
            foreach (League league in e.Leagues.Girls)
            {
                SetFinalTable(league, e.GeneralRanking.Girls);
            }
            foreach (League league in e.Leagues.Boys)
            {
                SetFinalTable(league, e.GeneralRanking.Boys);
            }
            BPrevious.IsEnabled = false;
            BNext.IsEnabled = true;
            ShowFinalTable(_finalTables[_currentFinalTable]);
        }
        private void SetFinalTable(League league, List<PlayerGeneral> playerGenerals)
        {
            _finalTables.Add(new FinalTable { Name = league.Name });
            for (int j = 0; j < league.Heads.Length; j++)
            {
                PlayerGeneral player1 = (league.Heads[j] > playerGenerals.Count) ? null : playerGenerals[league.Heads[j] - 1];
                PlayerGeneral player2 = (league.Chalengers[j] > playerGenerals.Count) ? null : playerGenerals[league.Chalengers[j] - 1];
                _finalTables.Last().Add(new FinalTableMatch
                {
                    Palyer1Name = player1?.Name,
                    Palyer2Name = player2?.Name,
                });
                if (!string.IsNullOrWhiteSpace(player1?.Group) && !string.IsNullOrWhiteSpace(player2?.Group))
                {
                    _finalTables.Last().Last().InError = player1.Group.Equals(player2.Group);
                }
            }
        }
        private void ShowFinalTable(FinalTable finalTable)
        {
            for (int i = 1; i <= 8; i++)
            {
                ((PlayerLabel)FindName($"PlayerName{i}1")).Value = string.Empty;
                ((PlayerLabel)FindName($"PlayerName{i}2")).Value = string.Empty;
            }
            TBGroup.Text = finalTable.Name;
            for (int i = 1; i <= finalTable.Count; i++)
            {
                ((PlayerLabel)FindName($"PlayerName{i}1")).Value = finalTable[i - 1].Palyer1Name;
                ((PlayerLabel)FindName($"PlayerName{i}1")).Player.HorizontalAlignment = HorizontalAlignment.Right;
                ((PlayerLabel)FindName($"PlayerName{i}2")).Value = finalTable[i - 1].Palyer2Name;
                if (finalTable[i - 1].InError)
                {
                    ((PlayerLabel)FindName($"PlayerName{i}1")).Player.Tag = ((PlayerLabel)FindName($"PlayerName{i}1")).Player.Foreground;
                    ((PlayerLabel)FindName($"PlayerName{i}1")).Player.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    if (((PlayerLabel)FindName($"PlayerName{i}1")).Player.Tag is not null)
                    {
                        ((PlayerLabel)FindName($"PlayerName{i}1")).Player.Foreground = (SolidColorBrush)((PlayerLabel)FindName($"PlayerName{i}1")).Player.Tag;
                    }
                }
                ((PlayerLabel)FindName($"PlayerName{i}2")).Player.Tag = ((PlayerLabel)FindName($"PlayerName{i}1")).Player.Tag;
                ((PlayerLabel)FindName($"PlayerName{i}2")).Player.Foreground = ((PlayerLabel)FindName($"PlayerName{i}1")).Player.Foreground;
            }
        }
        #endregion
        public Final()
        {
            InitializeComponent();
        }
        #region Privates
        private void CloseFinalTableClick(object sender, RoutedEventArgs e)
        {
            CloseFinalTableEvent?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
