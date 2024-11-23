using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    [SerializeField] Transform endLineCollider;
    public ArcadeRacerController controller;
    public void gameStart()
    {
        endLineCollider.gameObject.GetComponent<BoxCollider>().enabled = false;


    }
    public void gameEnd()
    {

    }
}
