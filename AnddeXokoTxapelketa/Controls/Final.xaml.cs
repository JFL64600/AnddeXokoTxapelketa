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
        private readonly string[] _leagues = ["Retegi ll", "Titin lll", "Altuna lll"];
        private readonly int[] _heads = [1, 8, 4, 5, 3, 6, 7, 2];
        private readonly int[] _chalengers = [16, 9, 13, 12, 14, 11, 10, 15];
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
            SetFinalTable(_finalTables[_currentFinalTable]);
        }
        private void BNextClick(object sender, RoutedEventArgs e)
        {
            BPrevious.IsEnabled = true;
            BNext.IsEnabled = true;
            _currentFinalTable++;
            BNext.IsEnabled = _currentFinalTable != _finalTables.Count - 1;
            SetFinalTable(_finalTables[_currentFinalTable]);
        }
        #endregion
        #region Methods
        public void SetGeneralRanking(string tournamentName, List<PlayerGeneral> generalRanking)
        {
            tbTitle.Text = $"{tournamentName} - Andde Xoko Txapelketa";
            foreach (string league in _leagues)
            {
                _finalTables.Add(new FinalTable { Name = league });
                for (int j = 0; j < _heads.Length; j++)
                {
                    PlayerGeneral player1 = (_heads[j] >= generalRanking.Count) ? null : generalRanking[_heads[j] - 1];
                    PlayerGeneral player2 = (_chalengers[j] >= generalRanking.Count) ? null : generalRanking[_chalengers[j] - 1];
                    _finalTables.Last().Add(new FinalTableMatch
                    {
                        Palyer1Name = player1?.Name,
                        Palyer2Name = player2?.Name,
                    });
                    if (!string.IsNullOrWhiteSpace(player1?.Group) && !string.IsNullOrWhiteSpace(player2?.Group))
                    {
                        _finalTables.Last().Last().InError = player1.Group.Equals(player2.Group);
                    }
                    _heads[j] += 2 * _heads.Length;
                    _chalengers[j] += 2 * _chalengers.Length;
                }
            }
            BPrevious.IsEnabled = false;
            BNext.IsEnabled = true;
            SetFinalTable(_finalTables[_currentFinalTable]);
        }
        private void SetFinalTable(FinalTable finalTable)
        {
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
