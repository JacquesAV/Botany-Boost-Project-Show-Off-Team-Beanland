using UnityEngine;

public class PlaceableData : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite displayImage = null;
    [Header("Data")]
    [SerializeField] private string placeableName = "";
    [SerializeField] private PlaceableType placeableType;
    private PlaceableOrientation placeableOrientation;
    [SerializeField] private Vector2 placeableDimensions = Vector2.zero;
    [SerializeField] private int cost = 0;

    //The different kinds of placeables that are present in the game
    private enum PlaceableType
    {
        Flower,
        Bush,
        Tree,
        Flooring,
        Ornament
    }  

    private struct PlaceableStats
    {
        //Stats need to be added with designers help
    }

    #region Getters
    public int GetCost()
    {
        return cost;
    }

    public string GetPlaceableType()
    {
        return placeableType.ToString();
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }

    public Sprite GetDisplayImage()
    {
        return displayImage;
    }

    public Vector2 GetDimensions()
    {
        return placeableDimensions;
    }

    public string GetOrientation()
    {
        return placeableOrientation.ToString();
    }

    public string GetName()
    {
        return placeableName;
    }
    #endregion

}
//Orientation of the placeables
public enum PlaceableOrientation
{
    Left,
    Right,
    Forward,
    Back
}
