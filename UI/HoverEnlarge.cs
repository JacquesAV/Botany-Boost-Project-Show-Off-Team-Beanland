using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEnlarge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("The Transform you want to enlarge")][SerializeField] private RectTransform rectTransform=null;
    [Header("The scale you want it to enlarge it to")][SerializeField] private float enlargeScale=1;

    private Vector2 originalSize;
    //Used to enlarge and shrink ui elements.
    private void Start()
    {
        originalSize = new Vector2(rectTransform.rect.height, rectTransform.rect.width);
    }
    #region PointerEnterExit
    //Enlarges the RectTransform to be the scale size
    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.sizeDelta = originalSize * enlargeScale;
    }
    //Shrinks the RectTransform to be the scale size
    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.sizeDelta = originalSize;
    }
    #endregion
}
