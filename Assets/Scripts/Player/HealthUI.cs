using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void BreakHeart()
    {
        StartCoroutine(PlayBreak());
    }

    private IEnumerator PlayBreak()
    {
        anim.SetTrigger("HeartBreak");
        yield return new WaitForSeconds(0.3f); // thời gian khớp với animation
        gameObject.SetActive(false);
    }
}
