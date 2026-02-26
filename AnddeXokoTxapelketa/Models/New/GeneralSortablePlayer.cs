using System.Numerics;

namespace AnddeXokoTxapelketa.Models.New
{
    public class GeneralSortablePlayer : SortablePlayer, IComparable
    {
        #region Properties
        public int Position { get; set; } = 0;
        public string Group { get; set; } = string.Empty;
        #endregion
        #region Methods
        public int CompareTo(object? obj)
        {
            GeneralSortablePlayer opponent = (GeneralSortablePlayer)obj;
            if (Position < opponent.Position)
            {
                return -1;
            }
            if (Position > opponent.Position)
            {
                return 1;
            }
            if (PointsAll > opponent.PointsAll)
            {
                return -1;
            }
            if (PointsAll < opponent.PointsAll)
            {
                return 1;
            }
            if (Victories > opponent.Victories)
            {
                return -1;
            }
            if (Victories < opponent.Victories)
            {
                return 1;
            }
            if (PointsInDefeats > opponent.PointsInDefeats)
            {
                return -1;
            }
            if (PointsInDefeats < opponent.PointsInDefeats)
            {
                return 1;
            }
            if (PointsConceded > opponent.PointsConceded)
            {
                return 1;
            }
            if (PointsConceded < opponent.PointsConceded)
            {
                return -1;
            }
            return string.Compare(Name, opponent.Name);
        }
        #endregion
    }
}
