using UnityEngine;

public class BaseemntKeyTrigger : MonoBehaviour
{
    [Header("basement timeline 2")]
    [SerializeField] GameObject timeLine;

    [Header("basement Lock")]
    [SerializeField] GameObject basementLock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"�浹�� ������Ʈ: {collision.gameObject.name}");
        Debug.Log($"����ϴ� ������Ʈ: {basementLock.name}");
        Debug.Log($"���� ������Ʈ�ΰ�? {collision.gameObject == basementLock}");

        if (collision.gameObject == basementLock)
        {
            timeLine.SetActive(true);
            basementLock.GetComponent<Animator>().SetBool("isOpen", true);
            Destroy(gameObject);
        }
    }
}
