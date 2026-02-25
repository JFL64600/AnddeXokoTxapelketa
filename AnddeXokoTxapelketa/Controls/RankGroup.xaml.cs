using AnddeXokoTxapelketa.Models;
using System.Windows.Controls;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Diagonal.xaml
    /// </summary>
    public partial class RankGroup : UserControl
    {
        #region Methods
        public void SetGroup(string name, Group group)
        {
            GroupName.Text = name;
            int rank = 1;
            for (int i = 0; i < group.Players.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(group.Players[i].Name))
                {
                    continue;
                }
                ((RankPlayerLabel)FindName($"Player{rank}")).Value = $"{rank}. {group.Players[i].Name}";
                rank++;
            }
        }
        public void SetGroup(string name, List<Models.New.SortablePlayer> players)
        {
            GroupName.Text = name;
            int rank = 1;
            foreach (Models.New.SortablePlayer player in players)
            {
                if (string.IsNullOrWhiteSpace(player.Name))
                {
                    continue;
                }
                ((RankPlayerLabel)FindName($"Player{rank}")).Value = $"{rank}. {player.Name}";
                rank++;
            }
        }
        #endregion
        public RankGroup()
        {
            InitializeComponent();
        }
    }
}
