using UnityEngine;

public class KnifeTrigger : MonoBehaviour
{
    [SerializeField] private KnifeController[] knives; // ¶³¾î¶ß¸± Ä®

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")/*collision.GetComponent<Player>() != null*/)
        {
            foreach (var knife in knives)
            {
                knife.Drop();                    // Ä® ¶³¾î¶ß¸®±â
                Destroy(gameObject);            // Æ®¸®°Å´Â 1È¸¿ëÀÌ¸é ÆÄ±«
            }
        }
    }
}
