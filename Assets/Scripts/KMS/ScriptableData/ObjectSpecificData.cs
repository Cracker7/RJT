using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSpecificData", menuName = "ScriptableObjects/ObjectSpecificData", order = 1)]
public class ObjectSpecificData : ScriptableObject
{
    public GameObject Prefab;
    public float durability = 30;
    // 기타 필요한 데이터 추가
}
