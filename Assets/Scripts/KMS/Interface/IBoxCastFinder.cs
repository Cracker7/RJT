using UnityEngine;

public interface IBoxCastFinder
{
    /// <summary>
    /// 지정된 카메라와 파라미터를 사용해 BoxCast를 수행하고,
    /// 뷰포트 중앙에 가장 가까운 히트 오브젝트를 반환합니다.
    /// </summary>
    GameObject GetCenterBoxCastHit(Camera cam, Vector3 originOffset, Vector3 halfExtents, float maxDistance, LayerMask layerMask);
}
