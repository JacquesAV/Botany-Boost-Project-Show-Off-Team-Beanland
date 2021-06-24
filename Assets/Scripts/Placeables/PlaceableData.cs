using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class PlaceableData : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject prefab;//the prefab that will be created on visual
    [SerializeField] private Sprite displayImage = null;//the image displayed in the book
    [SerializeField] private bool isUprightObject = false;

    [Header("Data")]
    [SerializeField] private PlaceableType placeableType;
    private PlaceableOrientation placeableOrientation = PlaceableOrientation.Forward; //Default rotation/orientation for an object
    private Vector2 orientatedPlaceableDimensions = Vector2.one; //Current dimensions based on the orientation
    [SerializeField] private Vector2 placeableDimensions = Vector2.one;
    [SerializeField] private int cost = 0;//how much it costs to place

    private string placeableName = null;//the name of the plant
    [SerializeField] private string placeableNameEnglish;
    [SerializeField] private string placeableNameDutch;

    [SerializeField] private int biodiversity;//the biodiversity the plant provides
    [SerializeField] private int carbonIntake;//the carbon intake the plant provides
    [SerializeField] private int appeal;//how appealing the plant is
    [SerializeField] private InsectType insectType;
    [SerializeField] private int insectAttractiveness;//how many insects attracted

    private string plantInfoText = null;//the text shown on the extra info section
    [SerializeField] private string plantInfoTextEnglish;//the text shown on the extra info section
    [SerializeField] private string plantInfoTextDutch;//the text shown on the extra info section

    private string[] plantSummarisedText = null;//The summary of text shown when hovering
    [SerializeField] private string[] plantSummarisedTextEnglish;
    [SerializeField] private string[] plantSummarisedTextDutch;

    [SerializeField] private bool yieldsProduce;//if the object produces food
    [SerializeField] private int produceYield;//how much it produces
    [SerializeField] private bool attractsBirds;//if the object attracts birds

    [Header("Spread Effects")]
    [SerializeField] private int lifespanInDays = 5;//how long the plant can live when diseased or fighting invasive species

    [Range(0f, 1f)] [SerializeField] private float invaderSpawnChance = 0.25f;//the chance of invasive species spawning
    [Range(0f, 1f)] [SerializeField] private float invaderChanceSoftCap = 0.1f; //The cap to how high a invader spread chance can go

    [Range(0f, 1f)] [SerializeField] private float baseDiseaseChance = 0.25f;//The base chance of being infected
    [Range(0f, 1f)] [SerializeField] private float diseaseChanceSoftCap = 0.1f;//The cap to how high a disease spread chance can go

    public PlaceableData Initialize(PlaceableData baseData)
    {
        //Set information based on pre-created placeable data script
        prefab = baseData.prefab;
        displayImage = baseData.displayImage;
        isUprightObject = baseData.isUprightObject;

        placeableType = baseData.placeableType;
        placeableOrientation = baseData.placeableOrientation;
        placeableDimensions = baseData.placeableDimensions;
        cost = baseData.cost;

        //Set descriptive text
        placeableNameEnglish = baseData.placeableNameEnglish;
        placeableNameDutch = baseData.placeableNameDutch;
        plantInfoTextEnglish = baseData.plantInfoTextEnglish;
        plantInfoTextDutch = baseData.plantInfoTextDutch;
        plantSummarisedTextEnglish = baseData.plantSummarisedTextEnglish;
        plantSummarisedTextDutch = baseData.plantSummarisedTextDutch;
        UpdateLanguageBasedInformation(); //Update the base writing using the current language settings

        biodiversity = baseData.biodiversity;
        carbonIntake = baseData.carbonIntake;
        appeal = baseData.appeal;
        insectType = baseData.insectType;
        insectAttractiveness = baseData.insectAttractiveness;
        yieldsProduce = baseData.yieldsProduce;
        produceYield = baseData.produceYield;
        attractsBirds = baseData.attractsBirds;

        lifespanInDays = baseData.lifespanInDays;
        invaderSpawnChance = baseData.invaderSpawnChance;
        baseDiseaseChance = baseData.baseDiseaseChance;
        diseaseChanceSoftCap = baseData.diseaseChanceSoftCap;
        return this;
    }

    //Update based on the current locale
    public void UpdateLanguageBasedInformation()
    {
        UpdateLanguageBasedInformation(LocalizationSettings.SelectedLocale);
    }

    //Update based on a given locale
    public void UpdateLanguageBasedInformation(Locale locale)
    {
        //Distinguish between the current localization settings
        switch (locale.ToString())
        {
            case "English (en)":
                //Update to english based information
                placeableName = placeableNameEnglish;
                plantInfoText = plantInfoTextEnglish;
                plantSummarisedText = plantSummarisedTextEnglish;
                break;

            case "Dutch (nl)":
                //Update to dutch based information
                placeableName = placeableNameDutch;
                plantInfoText = plantInfoTextDutch;
                plantSummarisedText = plantSummarisedTextDutch;
                break;
        }
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

    public bool GetIsUprightObject()
    {
        return isUprightObject;
    }

    public PlaceableType GetPlaceableType()
    {
        return placeableType;
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

    public int GetAppeal()
    {
        return appeal;
    }

    public InsectType GetInsectType()
    {
        return insectType;
    }

    public int GetInsectAttractiveness()
    {
        return insectAttractiveness;
    }

    public string GetExtraInfoText()
    {
        return plantInfoText;
    }

    public string[] GetSummarisedText()
    {
        return plantSummarisedText;
    }

    public bool GetYieldsProduce()
    {
        return yieldsProduce;
    }

    public bool GetAttractsBirds()
    {
        return attractsBirds;
    }

    public int GetLifespan()
    {
        return lifespanInDays;
    }

    public float GetInvaderSpreadChance()
    {
        return invaderSpawnChance;
    }

    public float GetInvaderSoftCap()
    {
        return invaderChanceSoftCap;
    }

    public float GetBaseDiseaseChance()
    {
        return baseDiseaseChance;
    }

    public float GetDiseaseSoftCap()
    {
        return diseaseChanceSoftCap;
    }

    public int GetProduceYield()
    {
        return produceYield;
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
//What insect will be attracted by the plant
public enum InsectType
{
    None,
    Bee,
    Butterfly,
    Beetle
}

//The different kinds of placeables that are present in the game
public enum PlaceableType
{
    Flower,
    Bush,
    Tree,
    Flooring,
    Ornament,
    None
}
