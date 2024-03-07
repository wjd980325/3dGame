using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class YieldInstructionCache
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    public static readonly Dictionary<float, WaitForSeconds> waitforseconds =
        new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if(!waitforseconds.TryGetValue(seconds, out wfs))
        {
            waitforseconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        }
        return wfs;
    }
}
