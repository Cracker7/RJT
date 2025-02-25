using UnityEngine;

[CreateAssetMenu(fileName = "RidingObjectData", menuName = "ScriptableObjects/RidingObjectData", order = 2)]
public class RidingObjectData : ScriptableObject
{
    public GameObject Prefab;
    public float durability = 30;
    // 기타 필요한 데이터 추가
}
