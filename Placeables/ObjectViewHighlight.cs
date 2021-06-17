using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectViewHighlight : MonoBehaviour
{
    private MeshRenderer meshRender = null;
    private Color origionalMeshColor;

    public void Start()
    {
        //Get starting components
        meshRender = gameObject.GetComponent<MeshRenderer>();
        if (meshRender != null)
        {
            origionalMeshColor = meshRender.material.color;
        }
        else
        {
            Debug.LogWarning("There was no mesh renderer found on the object, please make sure one is attached to the model.");
        }
    }

    public void SetRedHighlight()
    {
        //Error handling
        if (meshRender == null) 
        {
            Debug.LogWarning("There was no mesh renderer found on the object, please make sure one is attached to the model.");
            return;
        }

        //Flash the object red until the timer runs out
        meshRender.material.color = Color.red;
    }

    public void ResetHighlight()
    {
        //Error handling
        if (meshRender == null) 
        {
            Debug.LogWarning("There was no mesh renderer found on the object, please make sure one is attached to the model.");
            return;
        }
        //Reset back to the normal coloration
        meshRender.material.color = origionalMeshColor;
    }
}
