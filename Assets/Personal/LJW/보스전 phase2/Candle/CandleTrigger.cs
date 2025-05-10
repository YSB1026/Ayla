using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class CandleTrigger : MonoBehaviour
{
    [Header("Fire 오브젝트")]
    public GameObject fireObject;

    [Header("Light2D 컴포넌트")]
    public Light2D light2D;

    private bool playerInRange = false;
    public bool isLit = false;
    public float litTime;

    void Start()
    {
        TurnOff();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.L))
        {
            if (!isLit) // 이미 켜져 있지 않으면 켜기
            {
                Phase2_Manager.Instance.TryLightCandle(this);
            } 
        }
    }

    public void TurnOn()
    {
        isLit = true;
        litTime = Time.time;
        if (fireObject != null) fireObject.SetActive(true);
        if (light2D != null) light2D.enabled = true;
    }

    public void TurnOff()
    {
        isLit = false;
        if (fireObject != null) fireObject.SetActive(false);
        if (light2D != null) light2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
