using AnddeXokoTxapelketa.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class FinalTable : UserControl
    {
        #region Declarations
        private readonly string[] _leagues = ["Tournoi 1", "Tournoi 2", "Tournoi 3"];
        private readonly int[] _heads = [1, 8, 4, 5, 3, 6, 7, 2];
        private readonly int[] _chalengers = [16, 9, 13, 12, 14, 11, 10, 15];
        private readonly List<Models.FinalTable> _finalTables = [];
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
                _finalTables.Add(new Models.FinalTable { Name = league });
                for (int j = 0; j < _heads.Length; j++)
                {
                    _finalTables.Last().Add(new FinalTableMatch
                    {
                        Palyer1Name = generalRanking[_heads[j] - 1].Name,
                        Palyer2Name = generalRanking[_chalengers[j] - 1].Name,
                    });
                    if (!string.IsNullOrWhiteSpace(generalRanking[_heads[j] - 1].Group) && !string.IsNullOrWhiteSpace(generalRanking[_chalengers[j] - 1].Group))
                    {
                        _finalTables.Last().Last().InError = !generalRanking[_heads[j] - 1].Group.Equals(generalRanking[_chalengers[j] - 1].Group);
                    }
                    _heads[j] += 2 * _heads.Length;
                    _chalengers[j] += 2 * _chalengers.Length;
                }
            }
            BPrevious.IsEnabled = false;
            BNext.IsEnabled = true;
            SetFinalTable(_finalTables[2]);
        }
        private void SetFinalTable(Models.FinalTable finalTable)
        {
            TBGroup.Text = finalTable.Name;
            for (int i = 1; i <= finalTable.Count; i++)
            {
                ((PlayerLabel)FindName($"PlayerName{i}1")).Value = finalTable[i - 1].Palyer1Name;
                ((PlayerLabel)FindName($"PlayerName{i}1")).Player.HorizontalAlignment = HorizontalAlignment.Right;
                ((PlayerLabel)FindName($"PlayerName{i}2")).Value = finalTable[i - 1].Palyer2Name;
                //if (finalTable[i - 1].InError)
                //{
                //    ((PlayerLabel)FindName($"PlayerName{i}1")).Player.Foreground = new SolidColorBrush(Colors.Red);
                //    ((PlayerLabel)FindName($"PlayerName{i}2")).Player.Foreground = new SolidColorBrush(Colors.Red);
                //}
            }
        }
        #endregion
        public FinalTable()
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
