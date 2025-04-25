using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject sprite; 
    public ParticleSystem particles;

    private void Start()
    {
        StartCoroutine(waitForIt());
    }

    IEnumerator waitForIt()
    {
        yield return new WaitForSeconds(0.5f);
        play();
    }

    void play()
    {
        sprite.gameObject.SetActive(false);
        particles.Emit(9999);
    }
}
