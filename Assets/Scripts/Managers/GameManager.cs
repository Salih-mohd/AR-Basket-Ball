using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]public GameState CurrentState {  get; private set; }

    [Header("AR")]
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private HoopPlacer hoopPlacer;

    [Header("input")]
    [SerializeField] private InputActionReference throwAction;

    [Header("Ball")]
    [SerializeField] private GameObject ballObject;


    // events

    public event Action ScanningEvent;
    public event Action PlayingEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    private void Start()
    {
        SetState(GameState.Scanning);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        ApplyState(newState);
    }

    private void ApplyState(GameState state)
    {
        switch (state)
        {
            case GameState.Scanning:

                ScanningEvent?.Invoke();

                Debug.Log("on scanning state");

                planeManager.enabled = true;

                SetPlaneVisuals(true);

                //hoopPlacer.enabled = false;

                DisableThrow();

                ballObject.SetActive(false);


                break;

            case GameState.Placement:

                planeManager.enabled = true;
                SetPlaneVisuals(true);

                hoopPlacer.enabled = true;
                DisableThrow();
                ballObject.SetActive(false);

                break;

            case GameState.Playing:


                PlayingEvent?.Invoke();

                Debug.Log("on playing state");

                planeManager.enabled = false;
                SetPlaneVisuals(false);

                //hoopPlacer.enabled = false;
                EnableThrow();
                ballObject.SetActive(true);


               break;


            case GameState.GameOver:
                DisableThrow();
                ballObject.SetActive(false);
                break;



            default:
               break;
        }
    }

    private void SetPlaneVisuals(bool visible)
    {
        foreach (var plane in planeManager.trackables)
        {

            

            var meshVisualizer = plane.GetComponent<ARPlaneMeshVisualizer>();
            if (meshVisualizer != null)
                meshVisualizer.enabled = visible;

            var meshRenderer = plane.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.enabled = visible;

            plane.gameObject.SetActive(visible);
        }
    }

    private void DisableThrow()
    {

        Debug.Log(" disabling position action");
        if (throwAction.action.enabled)
            throwAction.action.Disable();
    }

    private void EnableThrow()
    {

        Debug.Log("enabling position action");
        StartCoroutine(ActivatePositionAction());
    }

    IEnumerator ActivatePositionAction()
    {
        yield return new WaitForSeconds(3);
        if (!throwAction.action.enabled)
            throwAction.action.Enable();
    }

}


public enum GameState
{
    Scanning,
    Placement,
    Playing,
    GameOver
}