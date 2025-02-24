using UnityEngine;

public class PerfectBearObject : InteractableObject
{

    private void Start()
    {
        // 부모 콜라이더의 최상단 위치 계산
        float topY = col.bounds.max.y;                
        //float spawnOffset = 2f; // 살짝 위로 띄우는 거리

        Vector3 spawnPosition = new Vector3(
            transform.position.x,
            topY,
            transform.position.z
        );

        Resources.Load<GameObject>("HeartParticle");

        // Star 생성 및 부모 설정
        GameObject heart = Instantiate(star, spawnPosition, Quaternion.identity, transform);

        heart.transform.localPosition = transform.InverseTransformPoint(spawnPosition);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        AudioManager.instance.PlaySfx(AudioManager.sfx.bear);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        AudioManager.instance.PlaySfx(AudioManager.sfx.bear);
    }


}
