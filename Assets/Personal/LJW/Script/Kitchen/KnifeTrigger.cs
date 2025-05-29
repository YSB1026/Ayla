using UnityEngine;

public class KnifeTrigger : MonoBehaviour
{
    [SerializeField] private KnifeController knife; // ¶³¾î¶ß¸± Ä®

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            knife.Drop();                    // Ä® ¶³¾î¶ß¸®±â
            Destroy(gameObject);            // Æ®¸®°Å´Â 1È¸¿ëÀÌ¸é ÆÄ±«
        }
    }
}
