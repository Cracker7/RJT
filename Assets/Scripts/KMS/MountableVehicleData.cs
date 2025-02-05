using UnityEngine;

[System.Serializable]
public class MountableVehicleData
{
    public GameObject mountedPrefab;     // 탑승 성공시 생성할 프리팹
    public string vehicleType;           // 차량 타입
    public Vector3 mountPointOffset;     // 생성 위치 오프셋
}
