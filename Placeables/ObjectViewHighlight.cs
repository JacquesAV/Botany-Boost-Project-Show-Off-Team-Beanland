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
        origionalMeshColor = meshRender.material.color;
    }

    public void SetRedHighlight()
    {
        //Error handling
        if (meshRender == null) { throw new MissingComponentException(); }

        //Flash the object red until the timer runs out
        meshRender.material.color = Color.red;
    }

    public void ResetHighlight()
    {
        //Error handling
        if (meshRender == null) { throw new MissingComponentException(); }

        //Reset back to the normal coloration
        meshRender.material.color = origionalMeshColor;
    }
}
