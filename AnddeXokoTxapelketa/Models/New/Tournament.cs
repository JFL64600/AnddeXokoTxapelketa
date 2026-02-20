namespace AnddeXokoTxapelketa.Models.New
{
    public class Tournament
    {
        public string Name { get; set; } = string.Empty;
        public List<Player> Girls { get; set; } = [];
        public List<Player> Boys { get; set; } = [];
    }
}