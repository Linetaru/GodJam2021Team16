using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplosionAnimatorController : MonoBehaviour
{
    private Animator explosionAnimator;
    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        explosionAnimator = this.GetComponent<Animator>();
    }

    public void OnDayStart()
    {
        explosionAnimator.SetTrigger("Unvolve");
    }

    public void OnNightStart()
    {
        explosionAnimator.SetTrigger("Evolve");
    }

    //Animation Event
    public void Evolve()
    {
        playerAnimator.SetTrigger("Evolve");
    }

    //Animation Event
    public void Unvolve()
    {
        playerAnimator.SetTrigger("Unvolve");
    }
}
