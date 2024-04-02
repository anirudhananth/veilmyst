using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Movement other;
        if (collision.gameObject.TryGetComponent<Movement>(out other))
        {
            other.Die();
        }
    }
}
