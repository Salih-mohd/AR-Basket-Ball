using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARTrackables : MonoBehaviour
{

    private ARPlaneManager _PManager;


    private void Start()
    {
        _PManager = Object.FindAnyObjectByType<ARPlaneManager>();
    }

    private void OnEnable()
    {
        SubscribeToPlanesChanged();
    }


    public void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARPlane> changes)
    {
        foreach (var plane in changes.added)
        {
            // handle added planes
            Debug.Log("added planes");
        }

        foreach (var plane in changes.updated)
        {
            // handle updated planes
            Debug.Log("updated planes");
        }

        foreach (var plane in changes.removed)
        {
            // handle removed planes
            Debug.Log("removed planes");
        }
    }

    void SubscribeToPlanesChanged()
    {
        //_PManager.trackablesChanged.AddListener(OnTrackablesChanged);
    }

}
