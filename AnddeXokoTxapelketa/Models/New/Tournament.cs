using AnddeXokoTxapelketa.Interfaces;

namespace AnddeXokoTxapelketa.Models.New
{
    public class Tournament : ITournament
    {
        #region Properties
        public string Name { get; set; } = string.Empty;
        public List<Player> Girls { get; set; } = [];
        public List<Player> Boys { get; set; } = [];
        public List<Group> GirlsGroups { get; set; } = [];
        public List<Group> BoysGroups { get; set; } = [];
        #endregion
    }
}