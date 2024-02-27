using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logique d'interaction pour Diagonal.xaml
    /// </summary>
    public partial class Diagonal : UserControl
    {
        #region Properties
        public ColorZoneMode Mode { get { return CZ.Mode; } set { CZ.Mode = value; } }
        #endregion
        public Diagonal()
        {
            InitializeComponent();
        }
    }
}
