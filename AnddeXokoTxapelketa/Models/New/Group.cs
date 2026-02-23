using AnddeXokoTxapelketa.Classes;

namespace AnddeXokoTxapelketa.Models.New
{
    public class Group
    {
        #region Properties
        public string Name { get; set; } = string.Empty;
        public Tools.GroupType Type { get; set; } = Tools.GroupType.Girls;
        public List<int> Players { get; set; } = [];
        public List<Rotation> Rotations { get; set; } = [];
        #endregion
    }
}
