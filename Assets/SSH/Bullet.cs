using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SSH
{
    public class Bullet : MonoBehaviour
    {
        private Transform _target;

        public void SetTarget(Transform target)
        {
            this._target = target;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
