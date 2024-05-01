using System;
using UnityEngine;

public delegate void HandleShow(bool show);

public abstract class Showable : MonoBehaviour
{
    public event HandleShow OnShow;
    public virtual void SetShow(bool show)
    {
        OnShow?.Invoke(show);
    }

    public void RegisterShow(HandleShow handler)
    {
        OnShow += handler;
    }

    public void UnregisterShow(HandleShow handler)
    {
        OnShow -= handler;
    }

    public abstract void Toggle();
}