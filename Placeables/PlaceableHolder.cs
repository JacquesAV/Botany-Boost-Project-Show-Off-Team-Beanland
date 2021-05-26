using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlaceableHolder : MonoBehaviour
{
    private List<GameObject> placeables=new List<GameObject>();//list of all placeable prefabs
    //List of each kind of placeable
    private readonly List<GameObject> flowers = new List<GameObject>(), bushes = new List<GameObject>(), trees = new List<GameObject>(), floorings = new List<GameObject>(), ornaments=new List<GameObject>();

    void Awake()
    {
        FilterPlaceables();
    }

    //Filters all the placeables into repsective type list
    private void FilterPlaceables()
    {
        //retrieve placeables from resource folder
        placeables = Resources.LoadAll("Prefabs", typeof(GameObject)).Cast<GameObject>().ToList();
        //check through placeable data for different types of placeables.
        foreach (GameObject placeable in placeables)
        {
            if (placeable.TryGetComponent(out PlaceableData temp))
            {
                switch (temp.GetPlaceableType())
                {
                    case "Flower":
                        flowers.Add(placeable);
                        break;
                    case "Bush":
                        bushes.Add(placeable);
                        break;
                    case "Tree":
                        trees.Add(placeable);
                        break;
                    case "Flooring":
                        floorings.Add(placeable);
                        break;
                    case "Ornament":
                        ornaments.Add(placeable);
                        break;
                    default:
                        DebugManager.DebugLog("The placeable: " + placeable.name + " is not of the correct type");
                        break;
                }
            }
            else
            {
                throw new System.Exception("GameObject: " + placeable.name + " does not have placeable data");
            }
        }
    }

    #region Getters
    public List<GameObject> GetFlowers()
    {
        return flowers;
    }
    public List<GameObject> GetBushes()
    {
        return bushes;
    }
    public List<GameObject> GetTrees()
    {
        return trees;
    }
    public List<GameObject> GetFlooring()
    {
        return floorings;
    }
    public List<GameObject> GetOrnaments()
    {
        return ornaments;
    }

    public List<GameObject> GetPlaceableByType(string placeableType)
    {
        switch (placeableType)
        {
            case "Flower":
                return flowers;
            case "Bush":
                return bushes;
            case "Tree":
                return trees;
            case "Flooring":
                return floorings;
            case "Ornament":
                return ornaments;
            case "None":           
                DebugManager.DebugLog("Tried to display None placeablyType");
                return null;
            default:
                throw new System.Exception("Could not find a matching placeableType");
        }
    }
    #endregion
}
