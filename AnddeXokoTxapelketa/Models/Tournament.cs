namespace AnddeXokoTxapelketa.Models
{
    public class Tournament
    {
        public string Name { get; set; } = string.Empty;
        public List<Group> Girls { get; set; } = [];
        public List<Group> Boys { get; set; } = [];
    }
}