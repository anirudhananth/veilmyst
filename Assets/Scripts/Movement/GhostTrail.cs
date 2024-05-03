using UnityEngine;
using DG.Tweening;

public class GhostTrail : MonoBehaviour
{
    private Movement move;
    private AnimationScript anim;
    private SpriteRenderer sr;
    private Transform ghostsParent;
    private GameObject player;
    public Color trailColor;
    public Color fadeColor;
    public float ghostInterval;
    public float fadeTime;

    private void Start()
    {
        anim = FindObjectOfType<AnimationScript>();
        move = FindObjectOfType<Movement>();
        player = GameObject.Find("Player");
        sr = GetComponent<SpriteRenderer>();
        ghostsParent = transform;
    }

    public void ShowGhost()
    {
        Sequence s = DOTween.Sequence();

        for (int i = 0; i < ghostsParent.childCount; i++)
        {
            Transform currentGhost = ghostsParent.GetChild(i);
            s.AppendCallback(()=> currentGhost.position = new(player.transform.position.x, player.transform.position.y - 0.25f, 0));
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().flipX = anim.sr.flipX);
            s.AppendCallback(()=>currentGhost.GetComponent<SpriteRenderer>().sprite = anim.sr.sprite);
            s.Append(currentGhost.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 0));
            s.AppendCallback(() => FadeSprite(currentGhost));
            s.AppendInterval(ghostInterval);
        }
    }

    public void FadeSprite(Transform current)
    {
        current.GetComponent<SpriteRenderer>().material.DOKill();
        current.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }

}
