using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectViewHighlight : MonoBehaviour
{
    private bool hasValidRenderers = true;
    private Dictionary<MeshRenderer, Color> meshRenders = new Dictionary<MeshRenderer, Color>();

    public void Start()
    {
        //Get the mesh renderers attached to the object children
        GetMeshRenderersInChildren();
    }

    private void GetMeshRenderersInChildren()
    {
        //Get starting components
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>().ToList())
        {
            //Check that a valid material and color is present, else break the entire process
            if (renderer.material == null || renderer.material.color == null)
            {
                //Set to false valid renderers
                hasValidRenderers = false;
                break;
            }
            else
            {
                //Add to the dictionary with the original color
                meshRenders.Add(renderer, renderer.material.color);
            }
        }

        //If the dictionary is empty or null, consider it invalid
        if(meshRenders == null || meshRenders.Count == 0)
        {
            //Set to false valid renderers
            hasValidRenderers = false;
        }

        //Debug a warning about the sucess of the process
        if (!GetHasValidRenderers())
        {
            return;
        }
    }

    public void SetRedHighlight()
    {
        //Error handling
        if (!GetHasValidRenderers())
        {
            return;
        }

        //Set the object colors to red
        foreach (KeyValuePair<MeshRenderer, Color> renderer in meshRenders)
        {
            renderer.Key.material.color = Color.red;
        }
    }

    public void ResetHighlight()
    {
        //Error handling
        if (!GetHasValidRenderers())
        {
            return;
        }

        //Reset the object colors
        foreach (KeyValuePair<MeshRenderer, Color> renderer in meshRenders)
        {
            renderer.Key.material.color = renderer.Value;
        }
    }

    private bool GetHasValidRenderers()
    {
        //Check if valid
        if (!hasValidRenderers)
        {
            //Debug
            Debug.LogWarning("There were no mesh renderers found in the object, please make sure one is attached to the models.");
            return false;
        }
        else
        {
            return true;
        }
    }
}
