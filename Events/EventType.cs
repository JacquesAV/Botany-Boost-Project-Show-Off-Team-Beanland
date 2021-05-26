using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defines the different event types to be used in event data in enumeration form
public enum EventType 
{
    RECREIVEDEBUG=0,
    CLICKEDPLACEABLEGUI,
    DAYPASSED,
    WEEKPASSED,
    PLANTINFECTED,
    PLANTCURED,
    PLANTINVADED,
    PLANTGASSED,
    TOTALSCORESUPDATED,
    OBJECTBOUGHTSCORES,
    OBJECTSOLDSCORES,
    OBJECTBUYREQUEST,
    OBJECTBUYREQUESTRESULT,
    CURRENTHOVEREDTILE
}
