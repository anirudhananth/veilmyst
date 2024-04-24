using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Objective: Enemy
{
    public string sceneName = "1-2";

    public override void OnDeath()
    {
        if (isDead) return;
        Disable();
        isDead = true;
        StartCoroutine(PostDeath());
    }

    private IEnumerator PostDeath()
    {
        Vector3 pos = transform.position;
        pos.z = GameManager.Instance.CamPos.z;
        GameManager.Instance.CamPos = pos;
        GameManager.Instance.PixelPerUnit = 24;
        Die(2f);
        yield return new WaitForSeconds(1f);
        MainManager.LoadScene(sceneName);
    }
}