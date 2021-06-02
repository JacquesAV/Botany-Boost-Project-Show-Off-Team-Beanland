using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEnlarge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform rectTransform=null;
    [SerializeField] private float enlargeScale=1;

    private Vector2 originalSize;
    //Used to enlarge and shrink ui elements.
    private void Start()
    {
        originalSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
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
