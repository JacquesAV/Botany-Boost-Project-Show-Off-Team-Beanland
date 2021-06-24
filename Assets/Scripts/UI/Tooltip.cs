using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private GameObject tooltipObject;//Text object that displays the details
    private bool isTextActive = false;//tells whether the text is on or off
    [SerializeField] private string displayText = "Text not entered";//text to display on the tooltip
    private GameObject tooltip;//the tooltip object
    private TextMeshProUGUI descriptionText;//the text displayed on the tooltip
    private RectTransform backgroundRectTransform;

    private Transform instantiateLocation;

    private void Awake()
    {
        instantiateLocation = GameObject.Find("Canvas").transform;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipObject != null)
        {
            if (!isTextActive)
            {
                isTextActive = true;
                //when hovering over the object, place the text below the mouse
                Vector3 mousePos = Input.mousePosition;
                tooltip = Instantiate(tooltipObject, new Vector3(mousePos.x, mousePos.y + Screen.height* 0.025f, mousePos.z), tooltipObject.transform.rotation, instantiateLocation);

                backgroundRectTransform = tooltip.transform.Find("Background").GetComponent<RectTransform>();
                descriptionText = tooltip.transform.Find("Text").GetComponent<TextMeshProUGUI>();

                descriptionText.text = displayText;
                float textPaddingSize = 4f;
                Vector2 backgroundSize = new Vector2(descriptionText.preferredWidth + textPaddingSize * 2f, descriptionText.preferredHeight + textPaddingSize * 2f);
                backgroundRectTransform.sizeDelta = backgroundSize;
            }
        }
        else
        {
            Debug.LogWarning("There is no tooltip prefab set, please add one or this code will not function");
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isTextActive)
        {
            isTextActive = false;
            Destroy(tooltip);
        }
    }

    private void FixedUpdate()
    {
        if (tooltipObject != null)
        {
            if (isTextActive)
            {
                Vector3 mousePos = Input.mousePosition;
                tooltip.transform.position = new Vector3(mousePos.x, mousePos.y + Screen.height * 0.025f, mousePos.z);
            }
        }
        else
        {
            Debug.LogWarning("There is no tooltip prefab set, please add one or this code will not function");
        }
    }

    public void SetDisplayText(string displayText)
    {
        this.displayText = displayText;
    }
}
