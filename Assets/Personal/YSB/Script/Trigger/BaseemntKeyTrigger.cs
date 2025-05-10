using UnityEngine;

public class BaseemntKeyTrigger : MonoBehaviour
{
    [Header("basement timeline 2")]
    [SerializeField] GameObject timeLine;

    [Header("basement Lock")]
    [SerializeField] GameObject basementLock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"충돌한 오브젝트: {collision.gameObject.name}");
        Debug.Log($"기대하는 오브젝트: {basementLock.name}");
        Debug.Log($"같은 오브젝트인가? {collision.gameObject == basementLock}");

        if (collision.gameObject == basementLock)
        {
            timeLine.SetActive(true);
            basementLock.GetComponent<Animator>().SetBool("isOpen", true);
            Destroy(gameObject);
        }
    }
}
