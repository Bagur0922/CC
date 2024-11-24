using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinText : MonoBehaviour
{
    TextMeshProUGUI tmp;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        tmp.text = GameManager.Instance.coin.ToString();
    }
}
