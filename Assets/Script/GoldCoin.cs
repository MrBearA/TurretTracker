using UnityEngine;
using System.Collections;

public class GoldCoin : MonoBehaviour
{
    private Transform goldUI; // Target UI position
    private float moveSpeed = 5f;
    private int goldAmount = 10;
    public Vector3 coinSize = new Vector3(0.5f, 0.5f, 1f); // Default smaller size

    public void Initialize(Transform uiTarget, int amount, Vector3? size = null)
    {
        goldUI = uiTarget;
        goldAmount = amount;
        coinSize = size ?? new Vector3(0.5f, 0.5f, 1f); // Use provided size or default
        StartCoroutine(SpawnBounceEffect());
    }

    IEnumerator SpawnBounceEffect()
    {
        float duration = 0.3f;
        float elapsedTime = 0f;
        Vector3 originalScale = Vector3.zero;
        Vector3 targetScale = coinSize;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float bounceFactor = Mathf.Sin(t * Mathf.PI);
            transform.localScale = Vector3.Lerp(originalScale, targetScale, bounceFactor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        StartCoroutine(MoveToUI());
    }

    IEnumerator MoveToUI()
    {
        yield return new WaitForSeconds(0.3f);

        Vector3 startPos = transform.position;
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float easedT = EaseOutQuad(t);
            transform.position = Vector3.Lerp(startPos, goldUI.position, easedT);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = goldUI.position;
        TowerManager.instance.EarnGold(goldAmount);
        Destroy(gameObject);
    }

    float EaseOutQuad(float t)
    {
        return 1 - (1 - t) * (1 - t);
    }
}
