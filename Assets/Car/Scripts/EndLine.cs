using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLine : MonoBehaviour
{
    private void Update()
    {
        if (Vector3.Distance(GameManager.Instance.controller.gameObject.transform.position, transform.position) > 20)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            //Debug.Log(Vector3.Distance(GameManager.Instance.controller.gameObject.transform.position, transform.position));
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.gameEnd();
        }
    }
}
