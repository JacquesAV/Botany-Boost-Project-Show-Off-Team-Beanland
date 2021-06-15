using System.Collections.Generic;

//Comparer for the object views attached to a tile building model
class TileObjectViewComparer : IEqualityComparer<TileBuildingModel>
{
    public bool Equals(TileBuildingModel x, TileBuildingModel y)
    {
        //Compare if two object views are equal based on shared object
        return x.objectView == y.objectView;
    }

    public int GetHashCode(TileBuildingModel obj)
    {
        return obj.objectView.GetHashCode();
    }
}