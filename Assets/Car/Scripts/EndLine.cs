using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLine : MonoBehaviour
{
    private void Update()
    {
        GameManager.Instance.endLineCollider = transform;
        if (GameManager.Instance.controller.gameObject == null) return;
        if (Vector3.Distance(GameManager.Instance.controller.gameObject.transform.position, transform.position) > 20)
        {
            gameObject.GetComponent<BoxCollider>(   ).enabled = true;
        }
        else
        {
            //Debug.Log(Vector3.Distance(GameManager.Instance.controller.gameObject.transform.position, transform.position));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<ArcadeRacerController>() != null)
        {
            GameManager.Instance.gameEnd();
        }
    }
}
