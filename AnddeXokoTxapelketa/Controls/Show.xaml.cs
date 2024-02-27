using AnddeXokoTxapelketa.Classes;
using AnddeXokoTxapelketa.Models;
using MaterialDesignThemes.Wpf;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        #region Events
        public event EventHandler? CloseTournamentEvent;
        #endregion
        #endregion
        #region Events
        private void BPreviousClick(object sender, RoutedEventArgs e)
        {
            //_currentGroup--;
            //ShowGroup();
        }
        private void BNextClick(object sender, RoutedEventArgs e)
        {
            //_currentGroup++;
            //ShowGroup();
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            TextBlock tbLabel = (TextBlock)FindName($"{tb.Name}Label");
            tbLabel.Text = tb.Text;
        }
        private void ExitEvent(object sender, EventArgs e)
        {
            Score score = (Score)sender;
            Regex rx = new(@"ScoreR([0-9]{1})C([0-9]{1})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(score.Name);
            if (matches.Count > 0)
            {
                //string row = matches[0].Groups[1].Value;
                _ = int.TryParse(matches[0].Groups[1].Value, out int row);
                _ = int.TryParse(matches[0].Groups[2].Value, out int column);
                _tournament.Boys[0].Players[row - 1].Results[column - ((row > column) ? 1 : 2)] = score.Value;
                Tools.SaveTournament(_root, _tournament);
                //TextBox tb = (TextBox)FindName($"TBPlayer{row}");
                //MessageBox.Show(tb.Text);
            }
            MessageBox.Show(((Score)sender).Value.ToString());
        }
        #endregion
        public Show()
        {
            InitializeComponent();
        }
        #region Methods
        public void SetTournament(Tournament tournament)
        {
            _tournament = tournament;
            tbTitle.Text = $"{tournament.Name} - Andde Xoko Txapelketa";
            //foreach (Player player in _tournament.Girls[0].Players) { 

            //}
            _currentGroup = 0;
            SetGroup(_tournament.Girls);
        }
        private void SetGroup(List<Models.Group> groups)
        {
            //int i;
            for (int i = 1; i <= 8; i++)
            {
                if (i <= groups[_currentGroup].Players.Count)
                {
                    ((TextBox)FindName($"TBPlayer{i}")).Text = groups[_currentGroup].Players[i - 1].Name;
                    for (int j = 1; j <= groups[_currentGroup].Players[i - 1].Results.Count; j++)
                    {
                        if (j == i)
                        {
                            continue;
                        }
                        ((Score)FindName($"ScoreR{i}C{j}")).Value = groups[_currentGroup].Players[i - 1].Results[j - 1];
                    }
                }
                else
                {
                    DisablePlayer(i);
                }
            }
        }
        #endregion
        #region Privates
        private void CloseTournamentClick(object sender, RoutedEventArgs e)
        {
            CloseTournamentEvent?.Invoke(this, new EventArgs());
            //EnablePlayer(8);
        }
        #endregion

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                //DisablePlayer(8);
            }
        }
        private void DisablePlayer(int index)
        {
            //DisablePlayer($"mdCZ{index}0"); //Player
            //TextBox tb = (TextBox)FindName($"TBPlayer{index}");
            //tb.Visibility = Visibility.Hidden;
            //DisablePlayer($"mdCZ0{index}"); //Player label
            //TextBlock tbLabel = (TextBlock)FindName($"TBPlayer{index}Label");
            //tbLabel.Visibility = Visibility.Hidden;
            DisablePlayerDiagonal(index); //No result
        }
        private void DisablePlayer(string name)
        {
            ColorZone cz = (ColorZone)FindName(name);
            cz.IsEnabled = false;
            cz.Background = Brushes.Beige;
        }
        private void DisablePlayerDiagonal(int index)
        {
            Diagonal diagonal = (Diagonal)FindName($"Diagonal{index}");
            diagonal.IsEnabled = false;
            diagonal.Mode = ColorZoneMode.PrimaryLight;
        }
        private void EnablePlayer(int index)
        {
            EnablePlayer($"mdCZ{index}0"); //Player
            TextBox tb = (TextBox)FindName($"TBPlayer{index}");
            tb.Visibility = Visibility.Visible;
            EnablePlayer($"mdCZ0{index}"); //Player label
            TextBlock tbLabel = (TextBlock)FindName($"TBPlayer{index}Label");
            tbLabel.Visibility = Visibility.Visible;
            EnablePlayerDiagonal(index); // No result
        }
        private void EnablePlayer(string name)
        {
            ColorZone cz = (ColorZone)FindName(name);
            cz.IsEnabled = true;
            cz.ClearValue(BackgroundProperty);
            cz.Mode = ColorZoneMode.Standard;
        }
        private void EnablePlayerDiagonal(int index)
        {
            ColorZone cz = (ColorZone)FindName($"mdCZ{index}{index}");
            cz.IsEnabled = true;
            cz.Mode = ColorZoneMode.PrimaryMid;
        }

        private void ScoreR1C2_ExitEvent(object sender, EventArgs e)
        {

        }
    }
}
