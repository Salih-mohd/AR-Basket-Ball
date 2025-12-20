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
         
        Mesh mesh = _meshFilter.sharedMesh;
        if (mesh == null)
            return;

         
        _meshCollider.sharedMesh = null;  
        _meshCollider.sharedMesh = mesh;
    }
}
