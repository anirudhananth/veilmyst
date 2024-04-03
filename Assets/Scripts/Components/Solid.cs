using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Solid : MonoBehaviour
{
    public static List<Solid> Solids;
    public Collider2D Collider { get; private set; }

    private void Start()
    {
        if (Solids == null) Solids = new();
        Solids.Add(this);
        Collider = GetComponent<Collider2D>();
    }
}
