using System;
using Newtonsoft.Json.Bson;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionAsset inputActions;

    [Header("Tuning")]
    [SerializeField] private float tapThresholdPixels = 15f;

    // public events (intent-level)
    public event Action<Vector2> OnTap;
    public event Action<Vector2, Vector2, float> OnSwipe;

    private InputAction pressAction;
    private InputAction positionAction;

    // runtime
    private Vector2 startPosition;
    private float startTime;
    private bool isPressing;

    private void Awake()
    {
        pressAction = inputActions.FindAction("Throw/Press");
        positionAction = inputActions.FindAction("Throw/Position");
    
    
    }

    private void Start()
    {
        // events managing actions

        if (ScoreManager.Instance == null)
        {
            Debug.Log("insance is null");
            return;
        }

        ShotManager.instance.OnShotStarted += DisableSwipeAction;
        ShotManager.instance.OnShotFinished += EnableSwipeAction;

        ShotManager.instance.OnGameOver += DisableThrowMap;
    }

    private void OnEnable()
    {

        EnhancedTouchSupport.Enable();
        pressAction.started += OnPressStarted;
        pressAction.canceled += OnPressCanceled;

    }

    private void OnDisable()
    {
        pressAction.started -= OnPressStarted;
        pressAction.canceled -= OnPressCanceled;


        
    }

    private void OnPressStarted(InputAction.CallbackContext context)
    {
        Debug.Log("touched");
        isPressing = true;
        startPosition = positionAction.ReadValue<Vector2>();
        startTime = Time.time;

        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();

        Debug.Log("Touched at: " + touchPos);

        if (touchPos != null)
        {
            OnTap?.Invoke(touchPos);
        }
        else
            Debug.Log("touch position is null");
        

    }

    private void OnPressCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("touch withdrawn");
        if (!isPressing) return;
        isPressing = false;

        Vector2 endPosition = positionAction.ReadValue<Vector2>();
        float duration = Time.time - startTime;

        float distance = Vector2.Distance(startPosition, endPosition);

        if (distance <= tapThresholdPixels)
        {
            //OnTap?.Invoke(endPosition);
            Debug.Log("on tap event invoked");
        }
        else
        {
            OnSwipe?.Invoke(startPosition, endPosition, duration);
            Debug.Log("swap event invoked");
        }
    }

    public void DisableSwipeAction()
    {
        positionAction.Disable();
        Debug.Log("disabled swipe action");
    }

    public void EnableSwipeAction()
    {
        positionAction.Enable();
        Debug.Log("enabled swipe action");
    }

    public void DisableThrowMap()
    {
        inputActions.FindActionMap("Throw").Disable();
    }

    public void EnableThrowMap()
    {
        inputActions.FindActionMap("Throw").Enable();
    }

    private void OnDestroy()
    {
        // events managing actions
        ShotManager.instance.OnShotStarted -= DisableSwipeAction;
        ShotManager.instance.OnShotFinished -= EnableSwipeAction;

        ShotManager.instance.OnGameOver -= DisableThrowMap;
    }
}
