using UnityEngine;

public class CloneSpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float stayTime = 3f;
    [SerializeField] private Vector3 spawnOffset = new Vector3(1f, 0, 0);
    [SerializeField] private bool spawnOnlyOnce = true;

    private float timer;
    private bool isPlayerInside;
    private bool hasSpawned;

    private void Update()
    {
        if (isPlayerInside && !hasSpawned)
        {
            timer += Time.deltaTime;

            if (timer >= stayTime)
            {
                SpawnClone();
                if (spawnOnlyOnce) hasSpawned = true;
                timer = 0f;
            }
        }
    }

    private void SpawnClone()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 spawnPos = playerTransform.position + spawnOffset;
        Instantiate(clonePrefab, spawnPos, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerInside = true;
            timer = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerInside = false;
            timer = 0f;
        }
    }
}
