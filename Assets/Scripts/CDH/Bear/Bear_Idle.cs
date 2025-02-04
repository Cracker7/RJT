using UnityEngine;

public class AnimationLoopCounter : MonoBehaviour
{
    public Animator animator;
    public int loopPunchCount = 0;
    public int loopDanceCount = 0;
    public int maxLoops = 3;
    public int minLoop = 1;

    // 애니메이션 이벤트에서 호출할 함수
    void PunchCount()
    {
        loopDanceCount = 0;
        animator.SetBool("isDanceEnd", false);
        ++loopPunchCount;
        Debug.Log(loopPunchCount);

        if (loopPunchCount >= maxLoops)
        {
            Debug.Log("PunchEnd!");
            // 다른 애니메이션으로 전환
            animator.SetBool("isPunchEnd", true);
        }
    }

    void DanceCount()
    {
        loopPunchCount = 0;
        animator.SetBool("isPunchEnd", false);
        ++loopDanceCount;
        if (loopDanceCount >= minLoop)
        {
            animator.SetBool("isDanceEnd", true);
        }
    }
}
