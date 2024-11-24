using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    public class Enemy : MonoBehaviour
    {
        [SerializeField] float heightOffset;
    public bool isPlayer = true;
        public float hp;
        public float maxHp;

        [SerializeField] GameObject hpBar;
        GameObject bar;

        Image usingBar;

        [SerializeField] Sprite enemyBar;

        // Start is called before the first frame update
        void Start()
        {
            hp = maxHp;

            Vector3 pos = transform.position + Vector3.up * heightOffset;

            bar = Instantiate(hpBar, transform);
            bar.transform.position = pos;
            usingBar = bar.GetComponent<whatisBar>().bar;
        }
        public void switchEnemy()
        {
            usingBar.sprite = enemyBar;
        }
        // Update is called once per frame
        void Update()
        {
        if (!GameManager.Instance.racing) bar.SetActive(false);
        else bar.SetActive(true);
            usingBar.fillAmount = hp / maxHp;
        }

        public void TakeDamage(float damage)
        {
            hp -= damage;
        }
    }

