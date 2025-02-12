using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public Terrain[] terrain;           // 터레인 참조
    public GameObject[] objectPrefab;   // 배치할 오브젝트 프리팹
    public int numberOfObjects = 100;     // 배치할 오브젝트 개수

    void Start()
    {
        if (terrain == null || objectPrefab == null)
        {
            Debug.LogError("Terrain 또는 objectPrefab이 할당되지 않았습니다.");
            return;
        }

        foreach (var t in terrain)
        {
            TerrainData terrainData = t.terrainData;
            Vector3 terrainSize = terrainData.size;
            Vector3 terrainPosition = t.transform.position;

            for (int i = 0; i < numberOfObjects; i++)
            {
                // 터레인 영역 내의 랜덤 x, z 좌표 (터레인 로컬 좌표 기준)
                float randomX = Random.Range(0f, terrainSize.x);
                float randomZ = Random.Range(0f, terrainSize.z);

                // 월드 좌표로 변환
                Vector3 samplePosition = new Vector3(randomX + terrainPosition.x, 0f, randomZ + terrainPosition.z);

                // 해당 위치의 높이(y) 값 얻기
                float terrainHeight = t.SampleHeight(samplePosition) + terrainPosition.y;
                // 기본 배치 위치 (터레인 표면)
                Vector3 finalPosition = new Vector3(samplePosition.x, terrainHeight, samplePosition.z);

                // 터레인의 정규화된 좌표 (0 ~ 1)
                float normalizedX = randomX / terrainSize.x;
                float normalizedZ = randomZ / terrainSize.z;
                // 해당 지점의 법선(normal) 얻기
                Vector3 terrainNormal = terrainData.GetInterpolatedNormal(normalizedX, normalizedZ);

                // 터레인의 법선에 맞게 오브젝트의 회전 설정
                Quaternion alignRotation = Quaternion.FromToRotation(Vector3.up, terrainNormal);
                // 수평 회전은 랜덤하게 적용 (옵션)
                Quaternion randomYRotation = Quaternion.Euler(0, Random.Range(-45f, 45f), 0);
                Quaternion finalRotation = alignRotation * randomYRotation;

                // 랜덤 프리팹 선택
                GameObject prefab = RandomPrefab();

                // 오브젝트 생성
                GameObject instance = Instantiate(prefab, finalPosition + new Vector3(0,0.1f,0), alignRotation);

                // // Collider를 이용해 오브젝트가 터레인 위에 정확히 위치하도록 Y 오프셋 조정
                // Collider col = instance.GetComponent<Collider>();
                // if (col != null)
                // {
                //     // collider.bounds.min.y는 오브젝트의 월드 좌표상 최저점
                //     float colliderBottom = col.bounds.min.y;
                //     // 터레인 표면 높이가 finalPosition.y로 계산되었으므로,
                //     // 두 값의 차이를 구해 오브젝트를 위로 이동시킵니다.
                //     float yAdjustment = finalPosition.y - colliderBottom;
                //     instance.transform.position += new Vector3(0, yAdjustment/5, 0);
                // }
            }
        }
    }

    // 배열에서 랜덤 프리팹 선택
    private GameObject RandomPrefab()
    {
        int randomIndex = Random.Range(0, objectPrefab.Length);
        return objectPrefab[randomIndex];
    }
}
