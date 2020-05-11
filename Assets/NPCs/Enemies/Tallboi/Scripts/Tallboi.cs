using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tallboi : MonoBehaviour
{
    public float actionFrequency = 0.5f;
    public float flourishChance = 0.5f;

    private bool cycleActive = true;
    private Animator animator;
    private WaitForSecondsRealtime wait;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        wait = new WaitForSecondsRealtime(actionFrequency);
    }

    private void Start()
    {
        StartCoroutine(FlourishRoutine());
    }

    private void TryFlourish()
    {
        if (Random.value < flourishChance)
            animator.SetTrigger("Flourish");
    }

    protected IEnumerator FlourishRoutine()
    {
        yield return wait;
        TryFlourish();

        if (cycleActive)
            StartCoroutine(FlourishRoutine());
    }

}
