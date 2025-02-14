using UnityEngine;

public class Upheel_Rock : MonoBehaviour
{
    public float destroyHeight = -10f; // 이 높이보다 내려가면 자동 삭제
    public GameObject explosionEffect; // 파편 효과 프리팹


    public float minGravity = 30f;  // 최소 중력
    public float maxGravity = 60f; // 최대 중력
    public float minPushForce = 5f;  // 최소 추가 속도
    public float maxPushForce = 50f; // 최대 추가 속도

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //rb.useGravity = false;
        // 돌마다 다른 중력 효과 적용
        float randomGravity = Random.Range(minGravity, maxGravity);
        rb.AddForce(Vector3.down * randomGravity * 1000f, ForceMode.Acceleration);

        // 랜덤한 힘을 추가해서 돌마다 속도 차이를 둠
        float randomPush = Random.Range(minPushForce, maxPushForce);
        rb.AddForce(new Vector3(Random.Range(-1f, 1f), -2, 0) * randomPush, ForceMode.Impulse); // 좌우로 살짝 흔들리면서 아래로 떨어짐
    }

    private void Update()
    {
        // 돌이 너무 아래로 떨어지면 삭제
        if (transform.position.y <= destroyHeight)
        {
            DestroyRock();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어와 부딪히면 삭제 및 파편 효과 생성
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void DestroyRock()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity); // 파편 효과 생성
        }
        Destroy(gameObject);
    }
}
