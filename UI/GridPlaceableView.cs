using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridPlaceableView : MonoBehaviour
{
    [SerializeField] private PlaceableHolder placeableHolder;//A class that is used to link up the view with holder.

    [SerializeField] private GridLayoutGroup placeableLayoutGroup;//Links to a GridLayoutGroup in the Unity scene

    [SerializeField] private GameObject placeableUIPrefab; //A prefab to display an item in the view

    [SerializeField] private List<GameObject> toolkitButtons; //Prefabs that each hold different event functionalities

    private ViewConfig viewConfig; //To set up the grid view, we need to know how many columns the grid view has, in the current setup,
                                   //this information can be found in a ViewConfig scriptable object, which serves as a configuration file for
                                   //view
    private Controller controller;
    private enum SelectedPlaceableType
    {
        None,
        Flower,
        Bush,
        Tree,
        Flooring,
        Ornament
    }

    private SelectedPlaceableType selectedPlaceable=SelectedPlaceableType.None;

    private void Start()
    {
        viewConfig = Resources.Load<ViewConfig>("ViewConfig");//Load the ViewConfig scriptable object from the Resources folder
        Debug.Assert(viewConfig != null);
        SetupPlaceableIconView();
        PopulatePlaceableIconView();
        controller = gameObject.AddComponent<Controller>();
    }
    #region PlaceablePopulation
    //------------------------------------------------------------------------------------------------------------------------
    //                                                  SetupItemIconView()
    //------------------------------------------------------------------------------------------------------------------------
    //Setup the grid view according to the ViewConfig object's requirements,sets the constraint mode and column count,
    //can make cosmetic adjustments to the GridLayoutGroup by adding like cellSize, spacing, padding, etc.
    private void SetupPlaceableIconView()
    {
        placeableLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;//Set the constraint mode of the GridLayoutGroup
        placeableLayoutGroup.constraintCount = viewConfig.gridViewColumnCount; //Set the column count according to the ViewConfig object
    }

    //------------------------------------------------------------------------------------------------------------------------
    //                                                  RepopulateItems()
    //------------------------------------------------------------------------------------------------------------------------        
    //Clears the grid view and repopulates it with new icons (updates the visible icons)
    private void RepopulatePlaceableIconView()
    {
        ClearIconView();
        PopulatePlaceableIconView();
    }

    //------------------------------------------------------------------------------------------------------------------------
    //                                                  PopulateItems()
    //------------------------------------------------------------------------------------------------------------------------        
    //Adds one icon for each item in the grid
    private void PopulatePlaceableIconView()
    {
        List<GameObject> placebles = placeableHolder.GetPlaceableByType(selectedPlaceable.ToString());
        if (placebles != null)
        {
            foreach (GameObject placeable in placeableHolder.GetPlaceableByType(selectedPlaceable.ToString()))
            {
                if (placeable.TryGetComponent(out PlaceableData temp))
                {
                    AddPlaceableToView(temp);
                }
            } 
        }
    }

    //------------------------------------------------------------------------------------------------------------------------
    //                                                  AddItemToView()
    //------------------------------------------------------------------------------------------------------------------------        
    //Adds a new placeable container to the view
    private void AddPlaceableToView(PlaceableData placeable)
    {
        GameObject newItemIcon = Instantiate(placeableUIPrefab);
        newItemIcon.transform.SetParent(placeableLayoutGroup.transform);
        newItemIcon.transform.localScale = Vector3.one*1.5f;//The scale would automatically change in Unity so we set it back to Vector3.one.

        GridViewPlaceableContainer placeableContainer = newItemIcon.GetComponent<GridViewPlaceableContainer>();
        Debug.Assert(placeableContainer != null);
        placeableContainer.Initialize(placeable);
    }

    //------------------------------------------------------------------------------------------------------------------------
    //                                                  ClearIconView()
    //------------------------------------------------------------------------------------------------------------------------        
    //Removes all existing icons in the gridview
    private void ClearIconView()
    {
        Transform[] allIcons = placeableLayoutGroup.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in allIcons)
        {
            if (child != placeableLayoutGroup.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    //------------------------------------------------------------------------------------------------------------------------
    //                                                  RepopulateToolkit()
    //------------------------------------------------------------------------------------------------------------------------        
    //Clears the grid view and repopulates it with toolkit button/icons
    private void RepopulateToolkitView()
    {
        ClearIconView();
        PopulateToolkitView();
    }

    //------------------------------------------------------------------------------------------------------------------------
    //                                                  PopulateToolkitView()
    //------------------------------------------------------------------------------------------------------------------------        
    //Adds one button for each intended toolkit functionality
    private void PopulateToolkitView()
    {
        if (toolkitButtons.Count > 0 && toolkitButtons != null)
        {
            foreach (GameObject buttonObject in toolkitButtons)
            {
                if (buttonObject!=null)
                {
                    GameObject newButton = Instantiate(buttonObject);
                    newButton.transform.SetParent(placeableLayoutGroup.transform);
                    newButton.transform.localScale = Vector3.one; //The scale would automatically change in Unity so we set it back to Vector3.one.
                }
            }
        }
    }

    #endregion

    #region SelectedSetters
    public void SetToToolKit()
    {
        selectedPlaceable = SelectedPlaceableType.None;
        RepopulateToolkitView();
    }
    public void SetToNone()
    {
        selectedPlaceable = SelectedPlaceableType.None;
        RepopulatePlaceableIconView();
    }
    public void SetToFlower()
    {
        selectedPlaceable = SelectedPlaceableType.Flower;
        RepopulatePlaceableIconView();
    }
    public void SetToBush()
    {
        selectedPlaceable = SelectedPlaceableType.Bush;
        RepopulatePlaceableIconView();
    }
    public void SetToTree()
    {
        selectedPlaceable = SelectedPlaceableType.Tree;
        RepopulatePlaceableIconView();
    }
    public void SetToFlooring()
    {
        selectedPlaceable = SelectedPlaceableType.Flooring;
        RepopulatePlaceableIconView();
    }
    public void SetToOrnament()
    {
        selectedPlaceable = SelectedPlaceableType.Ornament;
        RepopulatePlaceableIconView();
    }

    #endregion
}
