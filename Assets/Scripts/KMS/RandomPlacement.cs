using UnityEngine;

public class RandomPlacement : MonoBehaviour
{
    [Header("배치 설정")]
    [Tooltip("배치할 평평한 오브젝트(예: 플랫폼, 평면)")]
    public GameObject placementSurface;

    [Tooltip("배치할 오브젝트 프리팹 배열")]
    public GameObject[] objectPrefab;

    [Tooltip("배치할 오브젝트 개수")]
    public int numberOfObjects = 100;

    void Start()
    {
        if (placementSurface == null || objectPrefab == null || objectPrefab.Length == 0)
        {
            Debug.LogError("placementSurface 또는 objectPrefab이 할당되지 않았습니다.");
            return;
        }

        // placementSurface에 Collider가 있어야 Bounds를 가져올 수 있음
        Collider surfaceCollider = placementSurface.GetComponent<Collider>();
        if (surfaceCollider == null)
        {
            Debug.LogError("placementSurface에 Collider 컴포넌트가 없습니다.");
            return;
        }

        // Collider의 Bounds를 이용해 배치 영역 결정 (월드 좌표)
        Bounds bounds = surfaceCollider.bounds;

        // 배치 표면의 상단 방향(법선)은 placementSurface의 up 방향으로 가정
        Quaternion alignRotation = Quaternion.FromToRotation(Vector3.up, placementSurface.transform.up);

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Bounds 내의 랜덤한 x, z 좌표 선택
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);

            // y 좌표는 Collider의 최상단(bounds.max.y)을 사용하고, 약간의 오프셋을 추가
            Vector3 finalPosition = new Vector3(randomX, bounds.max.y, randomZ) + new Vector3(0, 0.1f, 0);

            // 표면의 기울기에 맞춰 회전하고, 추가로 y축을 기준으로 랜덤 회전 적용
            Quaternion randomYRotation = Quaternion.Euler(0, Random.Range(-45f, 45f), 0);
            Quaternion finalRotation = alignRotation * randomYRotation;

            // 배열에서 랜덤 프리팹 선택 및 생성
            GameObject prefab = RandomPrefab();
            Instantiate(prefab, finalPosition, finalRotation);
        }
    }

    // objectPrefab 배열에서 랜덤 프리팹 선택
    private GameObject RandomPrefab()
    {
        int randomIndex = Random.Range(0, objectPrefab.Length);
        return objectPrefab[randomIndex];
    }
}