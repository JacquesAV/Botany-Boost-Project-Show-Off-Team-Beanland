using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveStateChangeButton : MonoBehaviour
{
    public void SetToInactive()
    {
        EventManager.currentManager.AddEvent(new ActivateInteractionState(PlayerInteractionState.inactive));
    }
}
