using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class CandleTrigger : MonoBehaviour
{
    [Header("Fire ������Ʈ")]
    public GameObject fireObject;

    [Header("Light2D ������Ʈ")]
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
            if (!isLit) // �̹� ���� ���� ������ �ѱ�
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
