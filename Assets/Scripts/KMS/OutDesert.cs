using UnityEngine;

public class OutDesert : MonoBehaviour
{
    public delegate void InDesertDelegate();
    public event InDesertDelegate InDesertEvent;

    // 예를 들어, "Target" 태그를 가진 오브젝트가 트리거되었을 때 실행됩니다.
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("사막 입장 트리거 발생");
        // 특정 오브젝트를 태그로 필터링 (필요에 따라 조건을 수정하세요)
        Debug.Log("트리거된 탈것 레이어 : " + other.gameObject.layer);
        Debug.Log("carbody 레이어 : " + LayerMask.NameToLayer("carbody"));
        if (other.gameObject.layer == LayerMask.NameToLayer("carbody"))
        {
            Debug.Log("트리거된 같은 레이어 찾음");
            // "Body"라는 이름의 자식 오브젝트를 찾습니다.
            Transform playerTransform = FindChildRecursive(other.transform, "Remy Ragdoll");
            Transform bodyTransform = FindChildRecursive(other.transform, "Mesh");
            Debug.Log("playerTransform : " + playerTransform);
            Debug.Log("파인드가 실행됨");
            if (playerTransform != null)
            {
            }
            else
            {
                Debug.LogWarning("사막을 나감");
            }
        }
    }

    Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform found = FindChildRecursive(child, childName);
            if (found != null)
                return found;
        }
        return null;
    }
}
