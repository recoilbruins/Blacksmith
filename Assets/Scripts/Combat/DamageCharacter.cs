using BlackSmithAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCharacter : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    private Animator animator;
    private float forwardDirection = 0f;
    private float rightDirection = 0f;

    private static string FORWARD_ANIMATION = "GetHitForward";
    private static string BACKWARD_ANIMATION = "GetHitBack";
    private static string RIGHT_ANIMATION = "GetHitR";
    private static string LEFT_ANIMATION = "GetHitL";

    private bool isAnimationPlaying = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void TakeDamage()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isAnimationPlaying)
        {
            AnimateCharacter();
        }
    }

    private void AnimateCharacter()
    {
        CalculateDotProducts();
        if (forwardDirection == 1f)
        {
            PlayAnimation(FORWARD_ANIMATION);
        }
        else if (forwardDirection == -1f)
        {
            PlayAnimation(BACKWARD_ANIMATION);
        }
        else if (rightDirection == 1f)
        {
            PlayAnimation(RIGHT_ANIMATION);
        }
        else if (rightDirection == -1f)
        {
            PlayAnimation(LEFT_ANIMATION);
        }
        isAnimationPlaying = true;
        StartCoroutine(ResetAnimator());
    }

    private void CalculateDotProducts()
    {
        Vector3 dist = (enemy.position - transform.position).normalized;
        forwardDirection = Vector3.Dot(dist, transform.forward);
        rightDirection = Vector3.Dot(dist, transform.right);

        //Forward or Back
        if (forwardDirection > 0.2)
        {
            forwardDirection = 1f;
        }
        else if (forwardDirection < -0.2f)
        {
            forwardDirection = -1f;
        }

        // Right Or Left
        if (rightDirection > 0.2f)
        {
            rightDirection = 1f;
        }
        else if (rightDirection < -0.2f)
        {
            rightDirection = -1f;
        }
    }

    private void PlayAnimation(string animationName, bool isAnimationLocked = true)
    {
        animator.SetBool("isAnimationLocked", isAnimationLocked);
        animator.CrossFade(animationName, 0.2f);
    }

    private IEnumerator ResetAnimator()
    {
        yield return new WaitForSeconds(1f);
        isAnimationPlaying = false;
    }
}
