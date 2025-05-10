using System.Collections.Generic;
using UnityEngine;

public class CandleManager : MonoBehaviour
{
    public static CandleManager Instance;

    public List<CandleTrigger> allCandles = new List<CandleTrigger>();

    private void Awake()
    {
        Instance = this;
    }

    public List<CandleTrigger> GetAllCandles()
    {
        return allCandles;
    }
}
