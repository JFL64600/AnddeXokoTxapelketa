using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Score.xaml
    /// </summary>
    public partial class Score : UserControl
    {
        #region Declarations
        private readonly string[] AuthorizedValues = ["0", "1", "2", "3"];
        #region Events
        public event EventHandler? ExitEvent;
        #endregion
        #endregion
        #region Properties
        public int Value
        {
            get
            {
                _ = int.TryParse(TBScore.Text, out int score);
                return score;
            }
            set { TBScore.Text = value.ToString(); }
        }
        #endregion
        #region Events
        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = TBScore.Text.Length == 1 || !AuthorizedValues.Contains(e.Text);
        }
        private new void LostFocus(object sender, RoutedEventArgs e)
        {
            ExitEvent?.Invoke(this, new EventArgs());
        }
        #endregion
        public Score()
        {
            InitializeComponent();
        }
    }
}
