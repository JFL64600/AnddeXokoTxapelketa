using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace AnddeXokoTxapelketa.Controls
{
    /// <summary>
    /// Logique d'interaction pour Diagonal.xaml
    /// </summary>
    public partial class Diagonal : UserControl
    {
        public Diagonal()
        {
            InitializeComponent();
        }
        #region Methods
        public void Enable()
        {
            CZ.IsEnabled = true;
            CZ.Mode = ColorZoneMode.PrimaryMid;
        }
        public void Disable()
        {
            CZ.IsEnabled = false;
            CZ.Mode = ColorZoneMode.PrimaryLight;
        }
        #endregion
    }
}
