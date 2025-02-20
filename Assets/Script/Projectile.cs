using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 1f;
    private Transform target;

    public void Initialize(Transform enemy, float newSpeed, float newDamage)
    {
        target = enemy;
        speed = newSpeed;
        damage = newDamage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            if (target.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(damage); // NEW: Applies the upgraded damage
            }
            Destroy(gameObject);
        }
    }
}
