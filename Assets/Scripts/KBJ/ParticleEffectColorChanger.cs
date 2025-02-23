using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectColorChanger : MonoBehaviour
{
    public float SelectRadius = 30f;
    public Color highlightColor = Color.green;
    public LayerMask particleLayer;

    private GameObject closestParticlePrefab = null;
    private Dictionary<ParticleSystem, Color> lastOriginalColors = new Dictionary<ParticleSystem, Color>();

    void Update()
    {
        FindAndHighlightClosestParticle();
    }

    void FindAndHighlightClosestParticle()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, SelectRadius, particleLayer);
        GameObject nearestPrefab = null;
        float minDistance = SelectRadius;

        foreach (var col in colliders)
        {
            GameObject Prefab = col.transform.gameObject;
            float distance = Vector3.Distance(transform.position, Prefab.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPrefab = Prefab;
            }
        }

        if (nearestPrefab != closestParticlePrefab)
        {
            ResetPreviousParticles(); // 원래 색상 복구
            closestParticlePrefab = nearestPrefab;

            if (closestParticlePrefab != null)
            {
                StoreOriginalColors(closestParticlePrefab);
                ChangeParticlesColor(closestParticlePrefab, highlightColor);
            }
        }
    }

    void StoreOriginalColors(GameObject prefab)
    {
        lastOriginalColors.Clear(); // 이전 저장된 원래 색상 초기화
        foreach (var ps in prefab.GetComponentsInChildren<ParticleSystem>())
        {
            lastOriginalColors[ps] = ps.main.startColor.color; // 현재 색상을 저장
        }
    }

    void ResetPreviousParticles()
    {
        if (lastOriginalColors.Count == 0) return; // 원래 색상이 저장되지 않았다면 패스

        foreach (var kvp in lastOriginalColors)
        {
            if (kvp.Key != null) // ParticleSystem이 여전히 존재하는지 확인
            {
                SetParticleColor(kvp.Key, kvp.Value); // 원래 색상 복구
            }
        }
        lastOriginalColors.Clear(); // 복구 후 초기화
    }

    void ChangeParticlesColor(GameObject prefab, Color color)
    {
        foreach (var ps in prefab.GetComponentsInChildren<ParticleSystem>())
        {
            SetParticleColor(ps, color);
        }
    }

    void SetParticleColor(ParticleSystem ps, Color color)
    {
        var main = ps.main;
        main.startColor = color;

        // 기존 파티클들의 색상을 즉시 변경
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            particles[i].startColor = color;
        }

        ps.SetParticles(particles, count); // 변경 사항 적용

        ps.Clear();
        ps.Play();
    }

}
