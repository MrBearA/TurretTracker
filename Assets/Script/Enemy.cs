using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform[] waypoints;
    public float baseSpeed = 2f;
    private float currentSpeed;
    private int currentWaypointIndex = 0;
    public GameObject goldCoinPrefab;
    public bool isElite = false;
    private bool isKilledByTurret = false; // Track if the enemy was killed by a turret

    void Start()
    {
        int waveNumber = WaveManager.instance.GetCurrentWave();
        currentSpeed = baseSpeed + (waveNumber * 0.2f);
        if (isElite)
        {
            currentSpeed *= 1.5f;
            transform.localScale *= 1.3f;
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        transform.position = Vector2.MoveTowards(transform.position,
                                                 waypoints[currentWaypointIndex].position,
                                                 currentSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                PlayerHealth.instance.TakeDamage(isElite ? 2 : 1); // Damage player
                Destroy(gameObject); // Don't drop a coin
            }
        }
    }

    public void KillByTurret()
    {
        isKilledByTurret = true;
        Destroy(gameObject); // Calls OnDestroy()
    }

    void OnDestroy()
    {
        if (isKilledByTurret)
        {
            DropGold(); // Only drop gold if killed by a turret
        }
    }

    void DropGold()
    {
        if (TowerManager.instance != null && goldCoinPrefab != null)
        {
            Transform goldUI = GameObject.Find("GoldUI")?.transform;
            if (goldUI != null)
            {
                GameObject coin = Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
                int goldAmount = isElite ? 20 : 10;
                coin.GetComponent<GoldCoin>().Initialize(goldUI, goldAmount, new Vector3(0.5f, 0.5f, 1f));
            }
            else
            {
                Debug.LogError("GoldUI not found! Make sure the Gold UI text object is named 'GoldUI'.");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy took " + damage + " damage!");
        KillByTurret(); // Enemy dies instantly since it's a one-hit-kill game
    }
}
