using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SSH
{
    public class Bullet : MonoBehaviour
    {
        private Transform target;
        private float damage;
        private float speed;
        
        
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
        public void SetDamage(float damage)
        {
            this.damage = damage;
        }
        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        private void Update()
        {
            transform.Translate((target.position - transform.position).normalized * (Time.deltaTime * speed));
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("»ç¶óÁü");
            if (other.GetComponent<Enemy>() != null && target == other.transform)
            {
                target.GetComponent<Enemy>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
