using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectSpawner : MonoBehaviour
{
    public GameObject particleEffectPrefab; // 추가할 파티클 프리팹
    public float detectionRadius = 300f; // 이펙트를 적용할 거리
    public LayerMask drivableLayer; //carbody
    private Dictionary<GameObject, GameObject> activeEffects = new Dictionary<GameObject, GameObject>();

    void Update()
    {
        SpawnEffectsOnNearbyObjects();
    }

    void SpawnEffectsOnNearbyObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, drivableLayer);

        HashSet<GameObject> detectedObjects = new HashSet<GameObject>();

        foreach (var col in colliders)
        {
            GameObject target = col.gameObject;
            detectedObjects.Add(target);

            if (!activeEffects.ContainsKey(target)) // 아직 이펙트가 없다면 추가
            {
                GameObject effectInstance = Instantiate(particleEffectPrefab, target.transform);
                effectInstance.transform.localPosition = Vector3.zero; // 부모 기준 위치 조정
                activeEffects[target] = effectInstance;
            }
        }

        // 멀어진 오브젝트의 이펙트 제거
        List<GameObject> toRemove = new List<GameObject>();
        foreach (var kvp in activeEffects)
        {
            if (!detectedObjects.Contains(kvp.Key)) // 탐지 범위를 벗어났다면 제거
            {
                Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }

        // 딕셔너리에서 제거
        foreach (var obj in toRemove)
        {
            activeEffects.Remove(obj);
        }
    }
}
