using UnityEngine;

namespace Code.Turrets.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particlePrefab;
        public BulletDataSO data;

        private Rigidbody2D _rigid;
        private Vector2 _direction;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Transform newTarget, float offset = 0)
        {
            _direction = (newTarget.position - transform.position).normalized;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            float radian = (angle + offset) * Mathf.Deg2Rad;
            _rigid.rotation = angle + offset;
            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
            
            _rigid.linearVelocity = direction * data.bulletSpeed;

            Destroy(gameObject, 3.0f);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
            
            Enemy.Enemy enemy = collision.GetComponent<Enemy.Enemy>();
            if (enemy != null)
            {
                enemy.HandleHit(data.bulletDamage);
            }
            
            Instantiate(particlePrefab, transform.position, Quaternion.identity).Play();
            Destroy(gameObject);
        }
    }
}