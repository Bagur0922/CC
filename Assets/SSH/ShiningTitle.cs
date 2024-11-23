using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShiningTitle : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;

    [SerializeField]private float colorValue = 0;
    [SerializeField]private float colorChangeAmount;
    // Update is called once per frame
    void Update()
    {
        colorValue = (colorValue + Time.deltaTime * colorChangeAmount) % 510;
        if (colorValue > 255)
        {
            text.color = new Color(255/255f, (510 - colorValue)/255f, 0);
        }
        else
        {
            text.color = new Color(255/255f, colorValue/255f, 0);
        }
        
    }
}
