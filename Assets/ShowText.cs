using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowText : MonoBehaviour
{
    public string textValue;
    public Text textElement;

    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;
        
        float seconds = (t / 60);

        if (seconds > 10 && seconds < 20)
        {
            textElement.text = textValue;
            
        }
    }
}
