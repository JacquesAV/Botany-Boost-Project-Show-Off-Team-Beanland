using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] List<string> tutorials = null;
    [SerializeField] TextMeshProUGUI tutText = null;
    private int tutorialIndex = 0;
    private int prevIndexVal = -1;

    private float _waitTime = 7f;

    //Localized string table
    public LocalizedStringTable tutorialStringTable = new LocalizedStringTable { TableReference = "Tutorial Text" };

    [SerializeField] string completionScene = "Start Scene";

    //camera rotation tut
    private float topNRightBarrier = 0.97f;
    private float botNLeftBarrier = 0.03f;

    [Header("book display")]
    [SerializeField] private List<GameObject> bookDisplays = null;
    [SerializeField] private GameObject bookArrow;
    [SerializeField] private Button treeButton;

    [Header("Speed up")]
    [SerializeField] private List<Button> speedButtons=null;
    [SerializeField] private GameObject speedArrow;
    [SerializeField] private GameObject speedUpDisplay;

    private PlantEffects plant;
    private bool plantInfected = false;
    private bool plantInvaded = false;
    [SerializeField] private GameObject toolkit;

    [SerializeField] private GameObject missionTab;
    [SerializeField] private GameObject missionArrow;

    [Header("Score Display")]
    [SerializeField] private List<GameObject> scores=null;

    private bool subscribedToObjectBought = false;
    private bool subscribedToPlaceableGUI = false;
    private bool weekPassed = false;
    private bool unsubscribedFromObjectBougth = false;
    private bool unsubMission = false;
    private bool plantGassedUnsub = false;

    private float timeMissionWait = 3f;

    private void Start()
    {
        //tree button
        treeButton.onClick.AddListener(
            delegate
            {
                if(tutorialIndex==5)
                tutorialIndex++;
            }
        );

        //speed up buttons
        foreach (Button button in speedButtons)
        {
            button.onClick.AddListener(
            delegate
            {
                if (tutorialIndex == 11)
                    tutorialIndex++;
            }
            );
        }
    }

    void Update()
    {
        if (timeMissionWait <= 0)
        {
            missionTab.SetActive(false);
        }
        else
        {
            timeMissionWait--;
        }
        //Debug.Log("Index size: " + tutorials.Length + " CurrentIndex: " + _tutorialIndex);
        TutorialCompleted();
        CurrentTutorial();
        if (Input.GetKeyDown(KeyCode.F5))
        {
            tutorialIndex++;
        }
    }

    private void TutorialCompleted()
    {
        //when the index has reached the end it changed back to the main menu
        if (tutorials.Count == tutorialIndex)
        {
            SceneManager.LoadScene(completionScene);
        }
    }
    public void CurrentTutorial()
    {
        //Debug.Log("size of tut: " + tutorials.Count + " curr index: " + tutorialIndex);
        //used to keep track of what the current part of the tutorial is
        if (tutorials.Count > tutorialIndex)
            tutText.SetText(tutorials[tutorialIndex]);

        switch (tutorialIndex)
        {
            case 0://Hello Botanist, welcome to Botany Boost! You have been hired by the City Council to 
                //manage the local park, let’s take you around
                NextTutTimer();
                break;
            case 1://Use your WASD keys to move around the park
                if (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.S))
                {
                    tutorialIndex++;
                }
                break;
            case 2://You can also move your mouse to the corners of the screen to see explore the park
                if (Input.mousePosition.y >= Screen.height * topNRightBarrier|| Input.mousePosition.y <= Screen.height * botNLeftBarrier|| Input.mousePosition.x <= Screen.width * botNLeftBarrier|| Input.mousePosition.x >= Screen.width * topNRightBarrier)
                {
                    tutorialIndex++;
                }
                break;
            case 3://To rotate your view, you can hold Q or E
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                {
                    tutorialIndex++;
                }
                break;
            case 4://To zoom in and out, press + - or scroll up and down using your mouse
                if (Input.GetAxis("Mouse ScrollWheel") > 0f|| Input.GetAxis("Mouse ScrollWheel") < 0f||Input.GetKeyDown(KeyCode.Equals)||Input.GetKeyDown(KeyCode.Minus))
                {
                    tutorialIndex++;
                }
                break;
            case 5://Click the tree bookmark on the botany book to open it
                foreach (GameObject bookDisplay in bookDisplays)
                {
                    if (bookDisplay != null)
                    {
                        bookDisplay.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("Tutorial warning: No display for the book was selected");
                    }

                    if (bookArrow != null)
                    {
                        bookArrow.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("Tutorial warning: No display for the book arrow was selected");
                    }
                }
                break;
            case 6://Select a tree from the book, it has a price at the bottom and when planted you 
                //gain the respective amount of the 3 stats to the side
                if (!subscribedToPlaceableGUI)
                {
                    EventManager.currentManager.Subscribe(EventType.CLICKEDPLACEABLEGUI, OnTutPass);
                    subscribedToPlaceableGUI = true;
                    if (bookArrow != null)
                    {
                        bookArrow.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("Tutorial warning: No display for the book arrow was selected");
                    }
                }
                break;
            case 7://Place the plant anywhere in your park, you can try to rotate the item using < or >. 
                //If you are satisfied with the orientation, click on where you want to place the item in the park.
                foreach (GameObject gameObject in scores)
                {
                    gameObject.SetActive(true);
                }
                if (!subscribedToObjectBought)
                {
                    EventManager.currentManager.Unsubscribe(EventType.CLICKEDPLACEABLEGUI, OnTutPass);
                    EventManager.currentManager.Subscribe(EventType.OBJECTBOUGHTSCORES, OnTutPass);
                    subscribedToObjectBought = true;
                }
                break;
            case 8://See how those scores add up?
                if (!unsubscribedFromObjectBougth)
                {
                    EventManager.currentManager.Unsubscribe(EventType.OBJECTBOUGHTSCORES, OnTutPass);
                    unsubscribedFromObjectBougth = true;
                }
                NextTutTimer(7);
                break;
            case 9://You can hover over plants in your book to get extra information, you can also get 
                //interesting facts about the plant at the bottom right when selecting it.
                NextTutTimer(12);
                break;
            case 10://Take a look at these weekly missions and try to complete one to get extra money.
                if (!weekPassed)
                {
                    //EventManager.currentManager.AddEvent(new WeekHasPassed());
                    weekPassed = true;
                    EventManager.currentManager.Subscribe(EventType.MISSIONCOMPLETED, OnTutPass);

                    //Display arrow that points to the mission
                    if (missionArrow != null)
                    {
                        missionArrow.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("Tutorial warning: No image for the mission arrow was selected");
                    }
                }
                missionTab.SetActive(true);
                break;
            case 11://You can change the speed of time with these buttons
                speedUpDisplay.SetActive(true);
                if (!unsubMission)
                {
                    EventManager.currentManager.Unsubscribe(EventType.MISSIONCOMPLETED, OnTutPass);
                    unsubMission = true;

                    //Hide arrow that points to the mission
                    if (missionArrow != null)
                    {
                        missionArrow.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("Tutorial warning: No image for the mission arrow was selected");
                    }

                    //Display arrow that points to speed
                    if (speedArrow != null)
                    {
                        speedArrow.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("Tutorial warning: No image for the speed arrow was selected");
                    }
                }
                break;
            case 12://Do you see the green particles? It means the plants are sick. 
                //You can cure them using your tools from the toolkit!
                if (!plantInfected)
                {
                    plant = GameObject.FindGameObjectWithTag("Plant").GetComponent<PlantEffects>();
                    EventManager.currentManager.Subscribe(EventType.PLANTCURED, OnTutPass);
                    plant.SetIsSick(true);
                    toolkit.SetActive(true);
                    plantInfected = true;

                    //Hide arrow that points to speed
                    if (speedArrow != null)
                    {
                        speedArrow.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("Tutorial warning: No image for the speed arrow was selected");
                    }
                }
                break;
            case 13://Do you see the red particles? It means the plants have attracted invasive species. 
                //You will need to get rid of them with your spray!
                if (!plantInvaded)
                {
                    EventManager.currentManager.Unsubscribe(EventType.PLANTCURED, OnTutPass);
                    EventManager.currentManager.Subscribe(EventType.PLANTGASSED, OnTutPass);
                    plant.SetIsInvaded(true);
                    plantInvaded = true;
                }
                break;
            case 14://Play as long as you can by passing the increasing weekly quota (biodiversity, appeal, 
                //carbon intake). The longer you play, the higher score you’ll achieve.
                if (!plantGassedUnsub)
                {
                    EventManager.currentManager.Unsubscribe(EventType.PLANTGASSED, OnTutPass);
                    plantGassedUnsub = true;
                }
                NextTutTimer(7);
                break;
            case 15://This is the end of the tutorial, good luck!
                NextTutTimer(4);
                break;
            case 16:
                //EXIT TUTORIAL
                break;

        }
    }
    public void NextTutTimer(float waitTime=7)
    {
        //this is to make it so that the next part of the tutorial wont instantly be skipped
        if (_waitTime <= 0)
        {
            tutorialIndex++;
            _waitTime = waitTime;
        }
        else
        {
            _waitTime -= Time.deltaTime;
        }
    }

    private void OnTutPass(EventData eventData)
    {
        tutorialIndex++;
    }

    void OnEnable()
    {
        tutorialStringTable.TableChanged += LoadStrings;
    }

    void OnDisable()
    {
        tutorialStringTable.TableChanged -= LoadStrings;
    }

    private void LoadStrings(StringTable stringTable)
    {
        //Update string array to have the correct translation text
        foreach (string tutorialText in tutorials.ToArray())
        {
            int index = tutorials.IndexOf(tutorialText);
            tutorials[index] = GetLocalizedString(stringTable, "Tutorial Index " + index);
        }
    }

    static string GetLocalizedString(StringTable table, string entryName)
    {
        //Get the entry from the table
        var entry = table.GetEntry(entryName);

        //Return the string
        return entry.GetLocalizedString().ToString();
    }
}
