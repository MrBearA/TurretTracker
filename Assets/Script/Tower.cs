using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    public float detectionRange = 3f;
    public float fireRate = 1f;
    public float bulletSpeed = 5f;
    public float bulletDamage = 1f;
    public int maxTargets = 1;
    public float reloadSpeed = 1f;

    private int maxUpgradeLevel = 3; //Max Level is 3
    public int upgradeLevel = 1; // Default Level

    public Transform turretHead;
    public Transform bulletSpawnPoint;
    public GameObject projectilePrefab;
    private UpgradeUIManager upgradeUIManager;
    private SpriteRenderer spriteRenderer;

    private float fireCooldown = 0f;

    void Start()
    {
        StartCoroutine(FindUpgradeUIManager());
        spriteRenderer = GetComponent<SpriteRenderer>(); //Get sprite renderer
        HighlightTower(); //Ensure correct color at start
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        Enemy target = FindNearestEnemy();
        if (target != null)
        {
            RotateTowardsTarget(target.transform);
            if (fireCooldown <= 0f)
            {
                Shoot(target);
                fireCooldown = reloadSpeed / fireRate;
            }
        }
    }

    //Change Tower Color Based on Level
    void HighlightTower()
    {
        if (spriteRenderer != null)
        {
            if (upgradeLevel == 1)
                spriteRenderer.color = Color.white; // Default (White)
            else if (upgradeLevel == 2)
                spriteRenderer.color = new Color(0.5f, 1f, 0.5f); // Light Green
            else if (upgradeLevel == 3)
                spriteRenderer.color = new Color(1f, 0.5f, 0.5f); // Light Red (Max Level)
        }
    }

    //Increases by ONLY 1 Level Per Click

    public void UpgradeFireRate()
    {
        if (upgradeLevel < maxUpgradeLevel)
        {
            if (TowerManager.instance.SpendGold(100))
            {
                fireRate += 0.5f;
                upgradeLevel++; //Increase by 1, not jump to 3
                Debug.Log("Fire Rate Upgraded to Level " + upgradeLevel);
                HighlightTower();
            }
            else
            {
                Debug.Log("Not Enough Gold!");
            }
        }
        else
        {
            Debug.Log("Already Max Level!");
        }
    }

    public void UpgradeRange()
    {
        if (upgradeLevel < maxUpgradeLevel)
        {
            if (TowerManager.instance.SpendGold(100))
            {
                detectionRange += 0.5f;
                upgradeLevel++;
                Debug.Log("Range Upgraded to Level " + upgradeLevel);
                HighlightTower();
            }
            else
            {
                Debug.Log("Not Enough Gold!");
            }
        }
        else
        {
            Debug.Log("Already Max Level!");
        }
    }

    public void UpgradeBulletSpeed()
    {
        if (upgradeLevel < maxUpgradeLevel)
        {
            if (TowerManager.instance.SpendGold(100))
            {
                bulletSpeed += 1.5f;
                upgradeLevel++;
                Debug.Log("Bullet Speed Upgraded to Level " + upgradeLevel);
                HighlightTower();
            }
            else
            {
                Debug.Log("Not Enough Gold!");
            }
        }
        else
        {
            Debug.Log("Already Max Level!");
        }
    }

    public void UpgradeDamage()
    {
        if (upgradeLevel < maxUpgradeLevel)
        {
            if (TowerManager.instance.SpendGold(100))
            {
                bulletDamage += 0.5f;
                upgradeLevel++;
                Debug.Log("Bullet Damage Upgraded to Level " + upgradeLevel);
                HighlightTower();
            }
            else
            {
                Debug.Log("Not Enough Gold!");
            }
        }
        else
        {
            Debug.Log("Already Max Level!");
        }
    }

    public void UpgradeMultiTarget()
    {
        if (upgradeLevel < maxUpgradeLevel)
        {
            if (TowerManager.instance.SpendGold(100))
            {
                maxTargets += 1;
                upgradeLevel++;
                Debug.Log("Multi-Targeting Upgraded to Level " + upgradeLevel);
                HighlightTower();
            }
            else
            {
                Debug.Log("Not Enough Gold!");
            }
        }
        else
        {
            Debug.Log("Already Max Level!");
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Tower clicked!");

        if (UpgradeUIManager.instance != null)
        {
            Debug.Log("Toggling Upgrade UI!");
            UpgradeUIManager.instance.ShowUpgradeUI(this);
        }
        else
        {
            Debug.LogError("Upgrade UI Manager still not found!");
        }
    }

    public bool IsMaxLevel()
    {
        return upgradeLevel >= maxUpgradeLevel;
    }


    IEnumerator FindUpgradeUIManager()
    {
        yield return new WaitForSeconds(0.1f);
        upgradeUIManager = UpgradeUIManager.instance;

        if (upgradeUIManager == null)
        {
            Debug.LogError("Upgrade UI Manager still not found after waiting! Make sure it's in the scene.");
        }
    }

    Enemy FindNearestEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance <= detectionRange)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }
        return nearest;
    }

    void RotateTowardsTarget(Transform target)
    {
        if (target == null) return;

        Vector2 direction = target.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        turretHead.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    void Shoot(Enemy target)
    {
        if (bulletSpawnPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            projectile.GetComponent<Projectile>().Initialize(target.transform, bulletSpeed, bulletDamage);

            if (maxTargets > 1)
            {
                Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                int count = 1;

                foreach (Enemy enemy in enemies)
                {
                    if (enemy != target && count < maxTargets)
                    {
                        GameObject extraProjectile = Instantiate(projectilePrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                        extraProjectile.GetComponent<Projectile>().Initialize(enemy.transform, bulletSpeed, bulletDamage);
                        count++;
                    }
                }
            }
        }
    }
}
