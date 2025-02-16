using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        #region Methods
        public void Enable()
        {
            CZ.IsEnabled = true;
            TBScore.Visibility = Visibility.Visible;
        }
        public void Disable()
        {
            CZ.IsEnabled = false;
            TBScore.Visibility = Visibility.Hidden;
        }
        #endregion
        public Score()
        {
            InitializeComponent();
        }
    }
}
