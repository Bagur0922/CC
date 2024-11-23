using UnityEngine;
using System.Collections;
using UnityEngine.TextCore.Text;

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

        [SerializeField] private float detectionAngle = 90f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float shootingCooldown = 1f;
        [SerializeField] private int maxDetectableEnemies = 10; // 최대 감지 가능한 적 수

        [Header("Shooting Settings")] [SerializeField]
        private float damage = 10f;

        [SerializeField] private Transform firePoint;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private GameObject bullet;

        private bool canShoot = true;
        private Transform currentTarget;
        private Collider[] hitColliders; // 재사용할 배열

        private void Start()
        {
            // 시작시 배열 한번만 생성
            hitColliders = new Collider[maxDetectableEnemies];
        }

        private void Update()
        {
            if (canShoot)
                DetectAndShoot();
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

            float closestDistance = float.MaxValue;
            Transform closestEnemy = null;

            // 실제로 감지된 collider 수만큼만 순회
            for (int i = 0; i < numColliders; i++)
            {
                if (hitColliders[i] == null) continue;

                Vector3 directionToTarget = (hitColliders[i].transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                if (angleToTarget <= detectionAngle * 0.5f)
                {
                    if (!Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, detectionRange))
                        continue;

                    if (hit.collider != hitColliders[i])
                        continue;

                    float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = hitColliders[i].transform;
                    }
                }
            }

            currentTarget = closestEnemy;

            if (currentTarget != null && canShoot)
            {
                Vector3 targetDirection = (currentTarget.position - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, targetDirection, Time.deltaTime * 5f);

                Shoot();
                StartCoroutine(ShootingCooldown());
            }
        }

        private void Shoot()
        {
            if (muzzleFlash != null)
                muzzleFlash.Play();

            Vector3 rayOrigin = firePoint != null ? firePoint.position : transform.position;
            Vector3 rayDirection = (currentTarget.position - rayOrigin).normalized;

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, detectionRange, enemyLayer))
            {
                IDamageable target = hit.collider.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                    Debug.Log($"Hit enemy: {hit.collider.name}");
                }

                // Bullet bullet = Instantiate(bullet, transform.position, Quaternion.identity);
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

            // 감지 각도 표시
            Vector3 rightDir = Quaternion.Euler(0, detectionAngle * 0.5f, 0) * transform.forward;
            Vector3 leftDir = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, rightDir * detectionRange);
            Gizmos.DrawRay(transform.position, leftDir * detectionRange);

            // 현재 타겟 표시
            if (currentTarget != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, currentTarget.position);
            }
        }
    }
}