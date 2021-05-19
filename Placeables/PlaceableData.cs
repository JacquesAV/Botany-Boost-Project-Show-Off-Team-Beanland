using UnityEngine;

public class PlaceableData : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite displayImage = null;

    [Header("Data")]
    [SerializeField] private PlaceableType placeableType;
    private PlaceableOrientation placeableOrientation = PlaceableOrientation.Forward; //Default rotation
    [SerializeField] private Vector2 placeableDimensions = Vector2.zero;
    [SerializeField] private int cost = 0;
    [SerializeField] private string placeableName;
    [SerializeField] private int biodiversity;
    [SerializeField] private int carbonIntake;
    [SerializeField] private int attractiveScore; 
    [SerializeField] private InsectType insectType;
    [SerializeField] private int insectAttractiveness;

    public PlaceableData Initialize(PlaceableData baseData)
    {
        //Set information based on pre-created placeable data script
        prefab = baseData.prefab;
        displayImage = baseData.displayImage;
        placeableType = baseData.placeableType;
        placeableOrientation = baseData.placeableOrientation;
        placeableDimensions = baseData.placeableDimensions;
        cost = baseData.cost;
        placeableName= baseData.placeableName;
        return this;
    }

    //The different kinds of placeables that are present in the game
    private enum PlaceableType
    {
        Flower,
        Bush,
        Tree,
        Flooring,
        Ornament
    }  

    private enum InsectType
    {
        Bee,
        Butterfly,
        Beetle
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

    public PlaceableOrientation GetOrientation()
    {
        return placeableOrientation;
    }

    public string GetName()
    {
        return placeableName;
    }
    #endregion

    public void SetOrientation(PlaceableOrientation newOrientation)
    {
        //Update the stores orientation
        placeableOrientation = newOrientation;
    }
}

//Orientation of the placeables
public enum PlaceableOrientation
{
    Forward = 0,
    Right,
    Back,
    Left
}
