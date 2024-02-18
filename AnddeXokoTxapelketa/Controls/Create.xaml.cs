using AnddeXokoTxapelketa.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class Create : UserControl
    {
        #region Events
        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = NumberValidation(e.Text, (TextBox)sender);
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            bCreate.IsEnabled = CreateIsEnabled();
        }
        private void CreateClick(object sender, RoutedEventArgs e)
        {
            _ = int.TryParse(tbGirls.Text, out int girls);
            _ = int.TryParse(tbGirlsGroups.Text, out int girlsGroups);
            _ = int.TryParse(tbBoys.Text, out int boys);
            _ = int.TryParse(tbBoysGroups.Text, out int boysGroups);
            Tournament tournament = new()
            {
                Name = tbName.Text,
                Girls = GetGroups(girls, girlsGroups),
                Boys = GetGroups(boys, boysGroups)
            };
            MessageBox.Show("Go !!!");
        }
        #endregion
        public Create()
        {
            InitializeComponent();
        }
        #region Privates
        private bool NumberValidation(string value, TextBox tb)
        {
            System.Text.RegularExpressions.Regex regex = new("[^0-9]+");
            if (!regex.IsMatch(value))
            {
                int maxValue = (tb.Tag.ToString().Equals("groups", StringComparison.InvariantCultureIgnoreCase)) ? 4 : 32;
                return int.Parse($"{tb.Text}{value}") > maxValue;
            }
            return true;
        }
        private List<Group> GetGroups(int players, int groups)
        {
            List<Group> result = [];
            int playersGroup = Math.DivRem(players, groups, out int playersPlus);
            for (int i = 0; i < groups; i++)
            {
                result.Add(new Group());
                int newPlayersGroup = playersGroup;
                if (playersPlus > 0)
                {
                    newPlayersGroup++;
                    playersPlus--;
                }
                for (int j = 0; j < newPlayersGroup; j++)
                {
                    result.Last().Players.Add(new Player() { Results = [.. new int[newPlayersGroup - 1]] });
                }
            }
            return result;
        }
        private bool CreateIsEnabled()
        {
            if (!string.IsNullOrWhiteSpace(tbName.Text))
            {
                _ = int.TryParse(tbGirls.Text, out int girls);
                _ = int.TryParse(tbGirlsGroups.Text, out int girlsGroups);
                _ = int.TryParse(tbBoys.Text, out int boys);
                _ = int.TryParse(tbBoysGroups.Text, out int boysGroups);
                return girls > 0 && girlsGroups > 0 && boys > 0 && boysGroups > 0 && girls >= girlsGroups && boys >= boysGroups;
            }
            return false;
        }
        #endregion
    }
}
