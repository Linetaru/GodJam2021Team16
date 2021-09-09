using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CycleDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private DayNightCycleManager cycleManager;

    // Update is called once per frame
    void Update()
    {
        textDisplay.text =  cycleManager.getCurrentDay().ToString();
           
    }
}
