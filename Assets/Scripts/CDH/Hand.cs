using UnityEngine;

public class HandFollowBird : MonoBehaviour
{
    public Rigidbody handRb; // 손의 Rigidbody
    public Rigidbody birdRb; // 새의 Rigidbody
    public Animator birdAnimator; // 새의 애니메이터
    public ConfigurableJoint joint; // 손과 새를 연결하는 조인트

    void Start()
    {
       

        // 새의 Rigidbody를 Joint에 연결
        joint.connectedBody = birdRb;

        // Joint 설정 (손이 늘어나지 않도록 제한)
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.linearLimit = new SoftJointLimit { limit = 0.01f }; // 제한 거리 조절

        // 앵커 위치 조정 (손과 새 다리가 정확히 연결되도록)
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = new Vector3(0, -0.1f, 0); // 새 다리 아래쪽에 연결
    }

    void FixedUpdate()
    {
        // 애니메이션이 Transform.position을 직접 조작하는 경우, Rigidbody를 동기화
        birdRb.MovePosition(birdAnimator.transform.position);
    }
}
