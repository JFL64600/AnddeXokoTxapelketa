using AnddeXokoTxapelketa.Controls;
using AnddeXokoTxapelketa.EventsArgs;
using System.IO;
using System.Text;
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
        #region Methods
        public void OpenTournamentEventHandler(object sender, TournamentEventArgs e)
        {
            ((Show)((MaterialDesignThemes.Wpf.Transitions.TransitionerSlide)tMain.Items[2]).Content).Init(e.Tournament);
            tMain.SelectedIndex = 2;
        }
        public void OpenRankingEventHandler(object sender, RankingEventArgs e)
        {
            ((Rank)((MaterialDesignThemes.Wpf.Transitions.TransitionerSlide)tMain.Items[3]).Content).Init(e.Tournament);
            tMain.SelectedIndex = 3;
        }
        public void OpenFinalEventArgs(object sender, FinalEventArgs e)
        {
            ((Final)((MaterialDesignThemes.Wpf.Transitions.TransitionerSlide)tMain.Items[4]).Content).Init(e);
            tMain.SelectedIndex = 4;
        }
        public void CloseTournamentEventHandler(object sender, EventArgs e)
        {
            tMain.SelectedIndex = 0;
        }
        public void CloseRankEventHandler(object sender, EventArgs e)
        {
            tMain.SelectedIndex = 0;
        }
        public void CloseFinalEventHandler(object sender, EventArgs e)
        {
            tMain.SelectedIndex = 0;
        }
        public void CloseApplicationtEventHandler(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            start.OpenTournamentEvent += new EventHandler<TournamentEventArgs>(OpenTournamentEventHandler);
            start.OpenRankingEvent += new EventHandler<RankingEventArgs>(OpenRankingEventHandler);
            start.OpenFinalTableEvent += new EventHandler<FinalEventArgs>(OpenFinalEventArgs);
            start.CloseApplicationtEvent += new EventHandler(CloseApplicationtEventHandler);
            show.CloseTournamentEvent += new EventHandler(CloseTournamentEventHandler);
            rank.CloseRankEvent += new EventHandler(CloseRankEventHandler);
            final.CloseFinalTableEvent += new EventHandler(CloseFinalEventHandler);
            //Models.New.Group group = new() { Name = "Group", Type = Classes.Tools.GroupType.Girls };
            //group.Players.Add(1);
            //group.Players.Add(2);
            //Models.New.Rotation r = new() { Name = "Rotation" };
            //r.Matches.Add(new Models.New.Match());
            //group.Rotations.Add(r);
            //using StreamWriter sw = new(Path.Combine("D:\\Documents\\JF\\AnddeXokoTxapelketa\\2026", "test.json"), false, Encoding.UTF8);
            //sw.Write(System.Text.Json.JsonSerializer.Serialize(group));
        }
    }
}