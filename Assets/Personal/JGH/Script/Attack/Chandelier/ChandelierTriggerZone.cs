using UnityEngine;

public class ChandelierTriggerZone : MonoBehaviour
{
    public ChandelierDropper chandelierDropper;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            chandelierDropper.DropChandelier();
        }
    }
}

