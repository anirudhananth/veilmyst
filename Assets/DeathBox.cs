using UnityEngine;

[RequireComponent(typeof(LethalCollision))]
public class DeathBox : MonoBehaviour
{
    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
}
