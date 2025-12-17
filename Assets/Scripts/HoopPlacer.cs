using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HoopPlacer : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARAnchorManager anchorManager;
    [SerializeField] private GameObject hoopPrefab;

    private static readonly List<ARRaycastHit> hits = new();

    public async void Place(Vector2 screenPosition)
    {
        if (!raycastManager.Raycast(
                screenPosition,
                hits,
                TrackableType.PlaneWithinPolygon))
            return;

        Pose hitPose = hits[0].pose;

        var result = await anchorManager.TryAddAnchorAsync(hitPose);

        if (!result.status.IsSuccess())
        {
            Debug.Log("Failed to create anchor");
            return;
        }

        ARAnchor anchor = result.value;

        Instantiate(
            hoopPrefab,
            anchor.transform.position,
            anchor.transform.rotation,
            anchor.transform
        );

        Debug.Log("Hoop placed");
        InteractionCoordinator.instance.hoopPlaced = true;
    }
}
