using UnityEngine;
using System.Collections;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

namespace SSH
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
    }

    public class AutoShootingController : MonoBehaviour
    {
        [Header("Detection Settings")] [SerializeField]
        private float detectionRange = 10f;

        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float shootingCooldown = 1f;
        [SerializeField] private int maxDetectableEnemies = 10; // 최대 감지 가능한 적 수

        [Header("Shooting Settings")] [SerializeField]
        private float damage = 10f;
        private float speed = 200f;

        [SerializeField] private Transform firePoint;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private GameObject _bulletObject;

        private bool canShoot = true;
        private Transform currentTarget;
        private Collider[] hitColliders; // 재사용할 배열

        public Stats stat;

        bool attackPlayer = false;

        private void Start()
        {
            damage += GameManager.Instance.upgradeDmg * 5;
            
            // 시작시 배열 한번만 생성
            hitColliders = new Collider[maxDetectableEnemies];
            canShoot = true;

        }

        private void Update()
        {
            DetectAndShoot();
        }
        public void switchTarget()
        {
            enemyLayer = playerLayer;
            attackPlayer = true;
        }
        private void DetectAndShoot()
        {
            // NonAlloc 버전 사용 - 결과 수를 반환
            int numColliders = Physics.OverlapSphereNonAlloc(
                transform.position,
                detectionRange,
                hitColliders,
                enemyLayer
            );

            if (currentTarget != null)
            {
                transform.LookAt(currentTarget);
            }

            float closestDistance = float.MaxValue;
            Transform closestEnemy = null;

            // 실제로 감지된 collider 수만큼만 순회
            for (int i = 0; i < numColliders; i++)
            {
                if (hitColliders[i] == null) continue;

                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitColliders[i].transform;
                }
            }

            currentTarget = closestEnemy;

            if (currentTarget != null && canShoot)
            {
                transform.LookAt(currentTarget);

                Shoot();
                StartCoroutine(ShootingCooldown());
            }
        }

        private void Shoot()
        {
            if (muzzleFlash != null)
                muzzleFlash.Play();

            AudioManager.Instance.PlaySFX("Bang");

            Vector3 rayOrigin = firePoint != null ? firePoint.position : transform.position;
            Vector3 rayDirection = (currentTarget.position - rayOrigin).normalized;

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, detectionRange, enemyLayer))
            {
                
                Transform target = hit.collider.transform;
                if (target != null)
                {
                    print("shooting");

                    Debug.Log($"Found enemy: {hit.collider.name}");
                    Bullet bullet = Instantiate(_bulletObject, firePoint.position, Quaternion.identity)
                        .GetComponent<Bullet>();
                    bullet.SetTarget(hit.transform);
                    bullet.SetDamage(damage);
                    bullet.SetSpeed(speed);
                }


            }
        }

        private IEnumerator ShootingCooldown()
        {
            canShoot = false;
            yield return new WaitForSeconds(shootingCooldown);
            canShoot = true;
        }

        // 시각적 디버깅을 위한 기즈모
        private void OnDrawGizmosSelected()
        {
            // 감지 범위 표시
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            // 현재 타겟 표시
            if (currentTarget != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, currentTarget.position);
            }
        }
    }
}