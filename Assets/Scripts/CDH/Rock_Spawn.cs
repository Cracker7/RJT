using UnityEngine;
using System.Collections;

public class Rock_Spawn : MonoBehaviour
{
    public GameObject rockPrefab;  // 생성할 돌 프리팹
    //public Transform spawnPoint;  // 기준이 될 생성 위치

    public float minSpawnDelay = 1f; // 최소 생성 간격
    public float maxSpawnDelay = 3f; // 최대 생성 간격

    public float minX = -50f; // 돌이 생성될 X 좌표 최소값
    public float maxX = 50f;  // 돌이 생성될 X 좌표 최대값

    private void Start()
    {
        StartCoroutine(SpawnRocks());
    }

    private IEnumerator SpawnRocks()
    {
        while (true)
        {
            //랜덤 돌 생성 시간
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            // X 좌표를 랜덤하게 설정
            float randomX = Random.Range(minX, maxX);
            //Vector3 spawnPosition = new Vector3(randomX, spawnPoint.position.y, spawnPoint.position.z);
            Vector3 spawnPosition = new Vector3(randomX, 1600f, 1580f);

            // 돌 생성
            //Instantiate(rockPrefab, spawnPosition, Quaternion.identity);


            GameObject newRock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

            // 속도 랜덤 적용
            Rigidbody rockRb = newRock.GetComponent<Rigidbody>();
            if (rockRb != null)
            {
                float randomSpeed = Random.Range(5f, 30f); // 속도 범위
                rockRb.linearVelocity = new Vector3(0, -randomSpeed, 0); // 아래로 이동하는 속도 다르게 설정
            }

        }
    }
}
