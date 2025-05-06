using System;
using UnityEngine;

public static class PendantEventManager
{
    public static Action<LightColorController.ColorOption> OnPendantCollected;

    public static void TriggerPendantCollected(LightColorController.ColorOption color)
    {
        OnPendantCollected?.Invoke(color);
    }
}
