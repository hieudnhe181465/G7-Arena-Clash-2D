using UnityEngine;
using System.Collections;

public class StunController : MonoBehaviour
{
    public float stunDuration = 1f;

    public Sprite idleSprite;   // sprite idle
    public Sprite stunSprite;   // sprite bị choáng

    private SpriteRenderer sr;
    private bool isStunned = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Stun()
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine());
        }
    }

    IEnumerator StunCoroutine()
    {
        isStunned = true;

        // đổi sprite khi bị đánh
        sr.sprite = stunSprite;

        yield return new WaitForSeconds(stunDuration);

        // trở lại idle
        sr.sprite = idleSprite;

        isStunned = false;
    }
}