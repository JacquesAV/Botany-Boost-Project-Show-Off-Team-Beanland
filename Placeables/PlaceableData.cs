using UnityEngine;

public class PlaceableData : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite displayImage = null;

    [Header("Data")]
    [SerializeField] private PlaceableType placeableType;
    private PlaceableOrientation placeableOrientation = PlaceableOrientation.Forward; //Default rotation/orientation for an object
    private Vector2 orientatedPlaceableDimensions = Vector2.one; //Current dimensions based on the orientation
    [SerializeField] private Vector2 placeableDimensions = Vector2.one;
    [SerializeField] private int cost = 0;
    [SerializeField] private string placeableName;
    [SerializeField] private int biodiversity;
    [SerializeField] private int carbonIntake;
    [SerializeField] private int attractiveScore;
    [SerializeField] private InsectType insectType;
    [SerializeField] private int insectAttractiveness;

    [Header("Spread Effects")]
    [Range(0.05f,0.25f)][SerializeField] private float invaderSpawnChance = 0.25f;

    [Range(0.1f, 0.75f)] [SerializeField] private float baseDiseaseChance = 0.25f;//The base chance of being infected
    [Range(0.01f,0.5f)][SerializeField] private float diseaseSpreadModifier = 0.1f;//The modifier chance of spread increasing based on plant count


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
        biodiversity = baseData.biodiversity;
        carbonIntake = baseData.carbonIntake;
        attractiveScore = baseData.attractiveScore;
        insectType = baseData.insectType;
        insectAttractiveness = baseData.insectAttractiveness;

        invaderSpawnChance = baseData.invaderSpawnChance;
        baseDiseaseChance = baseData.baseDiseaseChance;
        diseaseSpreadModifier = baseData.diseaseSpreadModifier;
        return this;
    }

    #region Getters
    public GameObject GetPrefab()
    {
        return prefab;
    }
    public Sprite GetDisplayImage()
    {
        return displayImage;
    }

    public string GetPlaceableType()
    {
        return placeableType.ToString();
    }

    public PlaceableOrientation GetOrientation()
    {
        return placeableOrientation;
    }

    public Vector2 GetOrientatedDimensions()
    {
        return orientatedPlaceableDimensions;
    }

    public Vector2 GetDimensions()
    {
        return placeableDimensions;
    }

    public int GetCost()
    {
        return cost;
    }

    public string GetName()
    {
        return placeableName;
    }

    public int GetBiodiversity()
    {
        return biodiversity;
    }

    public int GetCarbonIntake()
    {
        return carbonIntake;
    }

    public int GetAttractiveScore()
    {
        return attractiveScore;
    }

    public InsectType GetInsectType()
    {
        return insectType;
    }

    public int GetInsectAttractiveness()
    {
        return insectAttractiveness;
    }

    public float GetInvaderSpreadChance()
    {
        return invaderSpawnChance;
    }

    public float GetBaseDiseaseChance()
    {
        return baseDiseaseChance;
    }

    public float GetDiseaseSpreadModifier()
    {
        return diseaseSpreadModifier;
    }

    #endregion
    #region Setters
    public void SetOrientation(PlaceableOrientation newOrientation)
    {
        //Update the orientation
        placeableOrientation = newOrientation;
    }
    public void SetOrientatedDimensions(Vector2 givenOrientatedDimension)
    {
        //Update the rotated orientation
        orientatedPlaceableDimensions = givenOrientatedDimension;
    }
    #endregion
}

//Orientation of the placeables
public enum PlaceableOrientation
{
    Forward = 0,
    Right,
    Back,
    Left
}

//The different kinds of placeables that are present in the game
public enum PlaceableType
{
    Flower,
    Bush,
    Tree,
    Flooring,
    Ornament
}

//Different types of insects that are present
public enum InsectType
{
    None,
    Bee,
    Butterfly,
    Beetle
}