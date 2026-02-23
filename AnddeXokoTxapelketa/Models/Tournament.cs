using AnddeXokoTxapelketa.Interfaces;

namespace AnddeXokoTxapelketa.Models
{
    public class Tournament : ITournament
    {
        #region Properties
        public string Name { get; set; } = string.Empty;
        public List<Group> Girls { get; set; } = [];
        public List<Group> Boys { get; set; } = [];
        #endregion
    }
}