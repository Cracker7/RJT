using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{
    // 선택 반경 및 감지 반경 설정
    public float selectRadius = 50f;
    public float detectionRadius = 300f;
    public Color highlightColor = Color.green;
    public GameObject particleEffectPrefab;
    public LayerMask targetLayer;

    private GameObject closestTarget = null;
    private Dictionary<GameObject, Dictionary<ParticleSystem, Color>> originalColors = new Dictionary<GameObject, Dictionary<ParticleSystem, Color>>();
    private Dictionary<GameObject, GameObject> activeEffects = new Dictionary<GameObject, GameObject>();

    void Update()
    {
        // 감지 반경 내의 모든 오브젝트를 검색
        Collider[] colliders = Physics.OverlapSphere(transform.position, Mathf.Max(selectRadius, detectionRadius), targetLayer);
        ProcessDetectedObjects(colliders);
    }

    void ProcessDetectedObjects(Collider[] colliders)
    {
        GameObject nearest = null;
        float minDistanceSqr = selectRadius * selectRadius;
        List<GameObject> detectedObjects = new List<GameObject>();

        // 감지된 모든 오브젝트를 순회
        foreach (var col in colliders)
        {
            GameObject target = col.gameObject;
            detectedObjects.Add(target);

            // 현재 오브젝트와의 거리 계산
            float distanceSqr = (transform.position - target.transform.position).sqrMagnitude;
            if (distanceSqr < minDistanceSqr)
            {
                minDistanceSqr = distanceSqr;
                nearest = target;
            }

            // 파티클 효과가 없으면 새로 생성
            if (!activeEffects.ContainsKey(target))
            {
                GameObject effectInstance = Instantiate(particleEffectPrefab, target.transform);
                effectInstance.transform.localPosition = Vector3.zero;
                activeEffects[target] = effectInstance;
            }
        }

        // 가장 가까운 대상이 변경된 경우 처리
        if (nearest != closestTarget)
        {
            // 이전 대상의 원래 색상 복원
            if (closestTarget != null)
            {
                ResetOriginalColors(closestTarget);
            }

            closestTarget = nearest;

            // 새 대상의 색상 변경
            if (closestTarget != null)
            {
                StoreOriginalColors(closestTarget);
                ChangeParticleColor(closestTarget, highlightColor);
            }
        }

        // 감지되지 않은 오브젝트의 파티클 효과 제거
        List<GameObject> toRemove = new List<GameObject>();
        foreach (var kvp in activeEffects)
        {
            if (!detectedObjects.Contains(kvp.Key))
            {
                Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }

        // 제거된 오브젝트 목록에서 삭제
        foreach (var obj in toRemove)
        {
            activeEffects.Remove(obj);
        }
    }

    // 원래 색상 저장
    void StoreOriginalColors(GameObject target)
    {
        if (!originalColors.ContainsKey(target))
        {
            originalColors[target] = new Dictionary<ParticleSystem, Color>();
            foreach (var ps in target.GetComponentsInChildren<ParticleSystem>())
            {
                originalColors[target][ps] = ps.main.startColor.color;
            }
        }
    }

    // 원래 색상 복원
    void ResetOriginalColors(GameObject target)
    {
        if (originalColors.TryGetValue(target, out var colors))
        {
            foreach (var kvp in colors)
            {
                if (kvp.Key != null)
                {
                    SetParticleColor(kvp.Key, kvp.Value);
                }
            }
            originalColors.Remove(target);
        }
    }

    // 파티클 색상 변경
    void ChangeParticleColor(GameObject target, Color color)
    {
        foreach (var ps in target.GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.main.startColor.color != color)
            {
                SetParticleColor(ps, color);
            }
        }
    }

    // 특정 파티클 시스템의 색상을 설정
    void SetParticleColor(ParticleSystem ps, Color color)
    {
        var main = ps.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color);

        // 현재 방출된 모든 파티클 가져오기
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        int count = ps.GetParticles(particles);

        // 모든 파티클의 색상을 변경
        for (int i = 0; i < count; i++)
        {
            particles[i].startColor = color;
        }

        // 변경된 색상을 적용
        ps.SetParticles(particles, count);

        // 기존 파티클을 지우고 새로 방출하여 즉시 반영
        ps.Clear();
        ps.Play();
    }
}
