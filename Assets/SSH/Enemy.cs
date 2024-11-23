using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SSH
{ 
    public class Enemy : MonoBehaviour, IDamageable
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void TakeDamage(float damage)
        {
            Debug.Log($"damaged + {damage}");
        }
    }
}
