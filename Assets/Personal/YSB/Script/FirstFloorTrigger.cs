using UnityEngine;

public class FirstFloorTrigger : MonoBehaviour
{
    [SerializeField] GameObject firstFloorObject;

    private void Start()
    {
        if (firstFloorObject.activeSelf)
        {
            firstFloorObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            firstFloorObject.SetActive(true);
        }
    }
}
