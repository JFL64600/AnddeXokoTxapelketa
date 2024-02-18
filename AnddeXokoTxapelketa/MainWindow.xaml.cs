using AnddeXokoTxapelketa.Controls;
using AnddeXokoTxapelketa.EventsArgs;
using System.Windows;
using System.Windows.Input;

namespace AnddeXokoTxapelketa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Events
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            start.OpenTournamentEvent += new EventHandler<TournamentEventArgs>(OpenTournamentEventHandler);
            start.CloseApplicationtEvent += new EventHandler(CloseApplicationtEventHandler);
            show.CloseTournamentEvent += new EventHandler(CloseTournamentEventHandler);
        }
        public void OpenTournamentEventHandler(object sender, TournamentEventArgs e)
        {
            ((Show)((MaterialDesignThemes.Wpf.Transitions.TransitionerSlide)tMain.Items[2]).Content).SetTournament(e.Tournament);
            tMain.SelectedIndex = 2;
        }
        public void CloseTournamentEventHandler(object sender, EventArgs e)
        {
            tMain.SelectedIndex = 0;
        }
        public void CloseApplicationtEventHandler(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}