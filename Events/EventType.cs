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
    GOTSICK,
    GOTCURED,
    GOTINVADED,
    GOTGASSED,
    TOTALSCORESUPDATED,
    OBJECTPLACEDSCORES,
    OBJECTREMOVEDSCORES
}
