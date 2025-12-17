using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

 
public class PlaneMeshColliderUpdater : MonoBehaviour
{
    ARPlaneMeshVisualizer _meshVisualizer;
    MeshFilter _meshFilter;
    MeshCollider _meshCollider;
    ARPlane _plane;

    void Awake()
    {
        _meshVisualizer = GetComponent<ARPlaneMeshVisualizer>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        _plane = GetComponent<ARPlane>();
    }

    void OnEnable()
    {
        _plane.boundaryChanged += OnBoundaryChanged;
        // Also update immediately in case plane already had a mesh
        UpdateCollider();
    }

    void OnDisable()
    {
        _plane.boundaryChanged -= OnBoundaryChanged;
    }

    void OnBoundaryChanged(ARPlaneBoundaryChangedEventArgs args)
    {
        UpdateCollider();
    }

    void UpdateCollider()
    {
        // The ARPlaneMeshVisualizer keeps an internal mesh; get it from MeshFilter.
        Mesh mesh = _meshFilter.sharedMesh;
        if (mesh == null)
            return;

        // Important: assigning sharedMesh directly avoids allocating new Mesh every frame.
        _meshCollider.sharedMesh = null; // clear first to avoid Unity warning on some versions
        _meshCollider.sharedMesh = mesh;
    }
}
