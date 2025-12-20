using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HoopPlacer : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARAnchorManager anchorManager;
    [SerializeField] private GameObject hoopPrefab;
    [SerializeField] private GameObject anchorPrefab;

    private static readonly List<ARRaycastHit> hits = new();

    public void Place(Vector2 screenPosition)
    {
        if (!raycastManager.Raycast(
                screenPosition,
                hits))
            return;


        



        if (hits[0].trackable is ARPlane)
        {
            Pose hitPose = hits[0].pose;


            // setting direction

            Camera arCamera = Camera.main;

            Vector3 lookDir = arCamera.transform.position - hitPose.position;
            lookDir.y = 0f;

            Quaternion hoopRotation = Quaternion.LookRotation(lookDir);

            // Vertical offset correction
            float verticalOffset = 1.5f; // 2 cm
            hitPose.position -= hitPose.up * verticalOffset;

            var obj=Instantiate(anchorPrefab,hitPose.position, hitPose.rotation);

            Instantiate(hoopPrefab,
                obj.transform.position,
                hoopRotation,
                obj.transform
                );


            Debug.Log("Hoop placed");
            InteractionCoordinator.instance.hoopPlaced = true;
            GameManager.instance.SetState(GameState.Playing);

        }

        //var result = await anchorManager.TryAddAnchorAsync(hitPose);

        //if (!result.status.IsSuccess())
        //{
        //    Debug.Log("Failed to create anchor");
        //    return;
        //}

        //ARAnchor anchor = result.value;

        //Instantiate(
        //    hoopPrefab,
        //    anchor.transform.position,
        //    anchor.transform.rotation,
        //    anchor.transform
        //);

        
    }
}
