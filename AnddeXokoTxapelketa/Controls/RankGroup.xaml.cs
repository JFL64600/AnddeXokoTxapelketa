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
            for (int i = 0; i < group.Players.Count - 1; i++)
            {
                if (string.IsNullOrWhiteSpace(group.Players[i].Name))
                {
                    continue;
                }
                ((RankPlayerLabel)FindName($"Player{i + 1}")).Value = $"{i + 1}. {group.Players[i].Name}";
            }
        }
        #endregion
        public RankGroup()
        {
            InitializeComponent();
        }
    }
}
