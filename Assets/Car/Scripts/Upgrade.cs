using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] GameObject[] upgradeLevels;
    [SerializeField] int[] upgradeCost;
    [SerializeField] bool isDmg;

    private void Start()
    {
        if (isDmg)
        {
            for (int i = 0; i < GameManager.Instance.upgradeDmg; i++)
            {
                upgradeLevels[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.upgradeHp; i++)
            {
                upgradeLevels[i].SetActive(true);
            }
        }
    }
    void gang()
    {
        if (isDmg)
        {
            for (int i = 0; i < GameManager.Instance.upgradeDmg; i++)
            {
                upgradeLevels[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.upgradeHp; i++)
            {
                upgradeLevels[i].SetActive(true);
            }
        }
    }
    public void Pressed()
    {
        if (isDmg)
        {
            if (GameManager.Instance.coin > upgradeCost[GameManager.Instance.upgradeDmg ])
            {
                GameManager.Instance.coin -= upgradeCost[GameManager.Instance.upgradeDmg];
                GameManager.Instance.upgradeDmg += 1;
                gang();
            }
        }
        else
        {
            if (GameManager.Instance.coin > upgradeCost[GameManager.Instance.upgradeHp])
            {
                GameManager.Instance.coin -= upgradeCost[GameManager.Instance.upgradeHp];
                GameManager.Instance.upgradeHp += 1;
                gang();
            }
        }
        
    }
}
