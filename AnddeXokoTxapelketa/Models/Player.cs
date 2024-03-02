namespace AnddeXokoTxapelketa.Models
{
    public class Player : IComparable
    {
        public string Name { get; set; } = string.Empty;
        public List<int> Results { get; set; } = [];

        public int CompareTo(object? obj)
        {
            int totalMy = Results.Sum(x => x);
            int victoriesMy = (from int result in Results
                             where result == 3
                             select result).Count();
            int pointsMy = (from int result in Results
                          where result != 3
                          select result).Sum();
            Player player = (Player)obj;
            int totalAgainst = player.Results.Sum(x => x);
            int victoriesAgainst = (from int result in player.Results
                               where result == 3
                               select result).Count();
            int pointsAgainst = (from int result in player.Results
                            where result != 3
                            select result).Sum();
            if (totalMy > totalAgainst)
            {
                return -1;
            }
            if (totalMy < totalAgainst)
            {
                return 1;
            }
            if (victoriesMy > victoriesAgainst)
            {
                return -1;
            }
            if (victoriesMy < victoriesAgainst)
            {
                return 1;
            }
            if (pointsMy > pointsAgainst)
            {
                return -1;
            }
            if (pointsMy < pointsAgainst)
            {
                return 1;
            }
            return string.Compare(Name, player.Name);
        }
    }
}
