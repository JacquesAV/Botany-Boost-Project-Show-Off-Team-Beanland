using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scriptable object saves global configuration for views, to use it in a similar way as a singleton, create ViewConfig object and put it in Resources
/// folder, then classes can use Resources.Load to access the needed values, see examples of this usage in the GridViewKeyboardController and ShopGridBuyView
/// classes
/// </summary>
[CreateAssetMenu(fileName ="Data",menuName ="ScriptableObjects/ViewConfig",order =1)]//Allows creating ViewConfig objects in Assets -> Create menu in the Unity Editor
public class ViewConfig : ScriptableObject
{
    public int gridViewColumnCount;
}
