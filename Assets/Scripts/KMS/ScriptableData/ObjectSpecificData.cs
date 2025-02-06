using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSpecificData", menuName = "ScriptableObjects/ObjectSpecificData", order = 1)]
public class ObjectSpecificData : ScriptableObject
{
    public GameObject winPrefab;
    public GameObject passPrefab;
    public float speed;
    public float maxDistance;
    public float jumpHeight;
    public float damage;
    public float durability;
    // 기타 필요한 데이터 추가
}
