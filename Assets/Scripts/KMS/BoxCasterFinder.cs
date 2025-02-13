using UnityEngine;

public class BoxCastFinder : IBoxCastFinder
{
    public GameObject GetCenterBoxCastHit(Camera cam, Vector3 originOffset, Vector3 halfExtents, float maxDistance, LayerMask layerMask)
    {
        if (cam == null)
        {
            Debug.LogError("Camera is null");
            return null;
        }

        Vector3 origin = cam.transform.position + originOffset;
        Vector3 direction = cam.transform.forward;
        Quaternion orientation = cam.transform.rotation;

        RaycastHit[] hits = Physics.BoxCastAll(origin, halfExtents, direction, orientation, maxDistance, layerMask);
        if (hits.Length == 0)
            return null;

        GameObject centerHitObject = null;
        float minViewportDistance = float.MaxValue;
        Vector2 viewportCenter = new Vector2(0.5f, 0.5f);

        foreach (RaycastHit hit in hits)
        {
            Vector3 viewportPoint = cam.WorldToViewportPoint(hit.point);
            Vector2 hitViewportPos = new Vector2(viewportPoint.x, viewportPoint.y);
            float distanceFromCenter = Vector2.Distance(hitViewportPos, viewportCenter);
            if (distanceFromCenter < minViewportDistance)
            {
                minViewportDistance = distanceFromCenter;
                centerHitObject = hit.collider.gameObject;
            }
        }

        return centerHitObject;
    }
}
