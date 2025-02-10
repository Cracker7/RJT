using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public Terrain[] terrain;           // 터레인 참조
    public GameObject[] objectPrefab;   // 배치할 오브젝트 프리팹
    public int numberOfObjects = 100; // 배치할 오브젝트 개수

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

            for (int i = 0; i < numberOfObjects; i++)
            {
                // 터레인 영역 내의 랜덤 x, z 좌표 생성 (터레인의 로컬 좌표 기준)
                float randomX = Random.Range(0f, terrainSize.x);
                float randomZ = Random.Range(0f, terrainSize.z);

                // 월드 좌표로 변환 (터레인이 월드 좌표상 다른 위치에 있을 경우 고려)
                Vector3 terrainPosition = t.transform.position;
                Vector3 samplePosition = new Vector3(randomX + terrainPosition.x, 0, randomZ + terrainPosition.z);

                // 해당 위치의 높이(y) 값 얻기
                float y = t.SampleHeight(samplePosition) + terrainPosition.y;

                // 최종 배치 위치
                Vector3 finalPosition = new Vector3(samplePosition.x, y, samplePosition.z);

                // 오브젝트 생성
                Instantiate(RandomPrefab(), finalPosition, Quaternion.identity);
            }
        }
    }

    private GameObject RandomPrefab()
    {
        // 랜덤으로 오브젝트 프리팹을 선택
        int randomIndex = Random.Range(0, objectPrefab.Length);
        return objectPrefab[randomIndex];
    }
}
