using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuotaDisplay : MonoBehaviour
{
    [SerializeField] private GameObject bioWarningSymbol;
    [SerializeField] private GameObject carbWarningSymbol;
    [SerializeField] private GameObject appealWarningSymbol;

    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.QuotaResponse, OnQoutaMet);
        EventManager.currentManager.Subscribe(EventType.QuotaIncreased, OnQoutaUpdated);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Subscribe(EventType.QuotaResponse, OnQoutaMet);
        EventManager.currentManager.Subscribe(EventType.QuotaIncreased, OnQoutaUpdated);
    }

    private void OnQoutaMet(EventData eventData)
    {
        //biodiversity
        if (eventData is BiodiversityQuota bioQuotaResult)
        {
            if (bioWarningSymbol != null)
            {
                //Handle the incoming data
                bioWarningSymbol.SetActive(bioQuotaResult.biodiversityMet);
            }
        }
        else
        {
            DebugManager.DebugLog("There was no biodiversity warning symbol set, please do so");
        }

        //carbon intake
        if (eventData is CarbonIntakeQuota carbQuotaResult)
        {
            if (carbWarningSymbol != null)
            {
                //Handle the incoming data
                carbWarningSymbol.SetActive(carbQuotaResult.carbonMet);
            }
        }
        else
        {
            DebugManager.DebugLog("There was no carbon intake warning symbol set, please do so");
        }

        //appeal
        if (eventData is AppealQuota appealQuotaResult)
        {
                if (appealWarningSymbol != null)
                {
                //Handle the incoming data
                appealWarningSymbol.SetActive(appealQuotaResult.appealMet);
                }
        }
        else
        {
            DebugManager.DebugLog("There was no appeal warning symbol set, please do so");
        }
    }

    private void OnQoutaUpdated(EventData eventData)
    {
        if (eventData is QuotaUpdated quotaScore)
        {
            if (scoreText != null)
            {
                //Handle the incoming data
                scoreText.text = quotaScore.weeklyQuotaScore.ToString();
            }
            else
            {
                DebugManager.DebugLog("There was no quota score text set, please do so");
            }
        }
    }
}
