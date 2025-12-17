using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{


    //[Header("Throw Power Tuning")]  -> this was for on throw finished 2nd*****
    //public float distancePowerMultiplier = 0.01f; // main power scaler (distance)
    //public float minSwipeTime = 0.08f;             // fastest meaningful swipe
    //public float maxSwipeTime = 0.45f;             // slowest meaningful swipe
    //public float maxTimeBoostMultiplier = 1.3f;


    [Header("Throw Tuning")]  // for on throw finished 3rd ********
    [SerializeField] private float maxSwipeDistance = 600f;   // pixels
    [SerializeField] private float maxHoldTime = 1.2f;        // seconds

    [SerializeField] private float forwardPower = 18f;        // base forward force
    [SerializeField] private float maxExtraUpForce = 8f;      // added by hold time

    [SerializeField] private float sideInfluence = 1.0f;      // left/right control
    [SerializeField] private float verticalInfluence = 1.0f;  // swipe up/down control



    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballParent;
    

    [SerializeField] private Vector3 ballPosition;


    [SerializeField] private float rotationAmount=10f;
    [SerializeField] private float impulseMultiplier = 1f;
    [SerializeField] private float extraUpForceAmount = 0.5f;

    public Rigidbody rb;


    // runtime
    private Vector2 startPosition;
    private float startTime;
    private bool isAiming;


    public float minSwipePixels=50f;
    public float speedMultiplier=.02f;
    public float maxThrowSpeed=25f;




    [Header("input actions")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction positionAction;
    private InputAction pressAction;

    private void Awake()
    {
        rb=ballPrefab.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        positionAction = inputActions.FindAction("Throw/Position");
        pressAction = inputActions.FindAction("Throw/Press");       
    }

    private void OnEnable()
    {
        pressAction.started += OnThrowActionStarted;
        pressAction.canceled += OnThrowActionFinished;
    }

    private void OnDisable()
    {
        pressAction.started -= OnThrowActionStarted;
        pressAction.canceled -= OnThrowActionFinished;
    }




    public void Rotate(int sighn)
    {
        cam.transform.Rotate(0, rotationAmount * sighn, 0);
    }

    public void Backup()
    {
        rb.isKinematic = true;
        ballPrefab.transform.localPosition = ballPosition;
    }

    public void OnThrowActionStarted(InputAction.CallbackContext context)
    {
        Debug.Log("started swiping");
        startPosition = positionAction.ReadValue<Vector2>(); ;
        startTime = Time.time;
        isAiming = true;
    }




    //public void OnThrowActionFinished(InputAction.CallbackContext context)
    //{

    //    Debug.Log("ended swiping");
    //    if (!isAiming) return;
    //    isAiming = false;

    //    // 1) get release data
    //    Vector2 releasePos = positionAction.ReadValue<Vector2>(); ;
    //    float dt = Mathf.Max(0.001f, Time.time - startTime);

    //    // 2) swipe vector & ignore tiny taps
    //    Vector2 swipe = releasePos - startPosition;
    //    if (swipe.magnitude < minSwipePixels) return;


    //    // 3) basic direction: mostly camera forward,
    //    //    but nudged by horizontal/vertical swipe
    //    Vector3 dir = cam.transform.forward
    //                  + cam.transform.right * (swipe.x / Screen.height)
    //                  + cam.transform.up * (swipe.y / Screen.height)
    //                  + cam.transform.up * extraUpForceAmount;  // NEW
    //    dir.Normalize();

    //    // 4) speed = (pixels / second) * multiplier, clamped
    //    float speed = (swipe.magnitude / dt) * speedMultiplier;
    //    speed = Mathf.Clamp(speed, 0f, maxThrowSpeed);


    //    // 5) throw
    //    DoThrow(dir * speed);
    //}


    // On throw action 2nd*****************

    //public void OnThrowActionFinished(InputAction.CallbackContext context)
    //{
    //    Debug.Log("ended swiping");
    //    if (!isAiming) return;
    //    isAiming = false;

    //    // 1) get release data
    //    Vector2 releasePos = positionAction.ReadValue<Vector2>();
    //    float dt = Time.time - startTime;

    //    // 2) swipe vector & ignore tiny taps
    //    Vector2 swipe = releasePos - startPosition;
    //    if (swipe.magnitude < minSwipePixels) return;

    //    // 3) direction (UNCHANGED – already solid)
    //    Vector3 dir = cam.transform.forward
    //                  + cam.transform.right * (swipe.x / Screen.height)
    //                  + cam.transform.up * (swipe.y / Screen.height)
    //                  + cam.transform.up * extraUpForceAmount;
    //    dir.Normalize();

    //    // =================================================
    //    // 4) POWER CALCULATION (REMADE – IMPORTANT PART)
    //    // =================================================

    //    // --- BASE POWER: distance dominates ---
    //    float basePower = swipe.magnitude * distancePowerMultiplier;
    //    // ↑ CHANGED: distance is the main source of power

    //    // --- TIME AS A BOOSTER (NOT DIVISION) ---
    //    // Clamp time to avoid extremes (fast flick abuse / very slow drags)
    //    float clampedTime = Mathf.Clamp(dt, minSwipeTime, maxSwipeTime);
    //    // ↑ NEW: makes time safe

    //    // Convert time into a 0–1 "speed feel"
    //    // Shorter time → closer to 1
    //    // Longer time  → closer to 0
    //    float speed01 = 1f - Mathf.InverseLerp(minSwipeTime, maxSwipeTime, clampedTime);
    //    // ↑ NEW: normalized speed factor

    //    // Convert speed into a small boost
    //    float timeBoost = Mathf.Lerp(1f, maxTimeBoostMultiplier, speed01);
    //    // ↑ NEW: bounded booster (ex: 1.0 → 1.3)

    //    // --- FINAL SPEED ---
    //    float speed = basePower + timeBoost;
    //    // ↑ CHANGED: distance × time boost

    //    speed = Mathf.Clamp(speed, 0f, maxThrowSpeed);

    //    // 5) throw
    //    DoThrow(dir * speed);
    //}



    // on throw action finished 3rd ********************



    public void OnThrowActionFinished(InputAction.CallbackContext context)
    {
        if (!isAiming) return;
        isAiming = false;

        Vector2 releasePos = positionAction.ReadValue<Vector2>();

        // --- 1️⃣ Raw inputs ---
        Vector2 swipe = releasePos - startPosition;
        float holdTime = Mathf.Clamp(Time.time - startTime, 0f, maxHoldTime);

        float swipeDistance = swipe.magnitude;
        if (swipeDistance < minSwipePixels) return;

        // --- 2️⃣ Normalize ---
        float distance01 = Mathf.Clamp01(swipeDistance / maxSwipeDistance);
        float time01 = Mathf.Clamp01(holdTime / maxHoldTime);

        // --- 3️⃣ Direction (aim) ---
        Vector3 dir =
            cam.transform.forward +
            cam.transform.right * (swipe.x / Screen.height) * sideInfluence +
            cam.transform.up * (swipe.y / Screen.height) * verticalInfluence;

        dir.Normalize();

        // --- 4️⃣ Power separation ---
        float forwardForce = forwardPower * distance01;
        float upForce = extraUpForceAmount + (time01 * maxExtraUpForce);

        // --- 5️⃣ Final force ---
        Vector3 finalForce =
            (dir * forwardForce) +
            (Vector3.up * upForce);

        DoThrow(finalForce);
    }





    // throw with velocity



    //private void DoThrow(Vector3 velocity)
    //{
    //    if (rb == null ) return;

    //    // detach (optional)
    //    //if (detachOnThrow) ballTransform.SetParent(null, true);

    //    // enable physics and set velocity directly
    //    rb.isKinematic = false;
    //    rb.linearVelocity = velocity;

    //    // small spin for feel
    //    rb.angularVelocity = Random.insideUnitSphere * 2f;
    //}


    // throw with force


    //private void DoThrow(Vector3 velocity)
    //{
    //    if (rb == null) return;

    //    // detach if needed
    //    // if (detachOnThrow) ballTransform.SetParent(null, true);

    //    // enable physics
    //    rb.isKinematic = false;

    //    // clear previous motion so force is consistent
    //    rb.linearVelocity = Vector3.zero;
    //    rb.angularVelocity = Vector3.zero;

    //    // apply immediate velocity change (ignores mass)
    //    rb.AddForce(velocity, ForceMode.VelocityChange);

    //    // small spin
    //    rb.angularVelocity = Random.insideUnitSphere * 2f;
    //}


    // thrwo with impulse

    private void DoThrow(Vector3 velocity)
    {
        if (rb == null) return;

        rb.isKinematic = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Option 1: Apply exact impulse to reach 'velocity' (mass-aware)
        Vector3 impulse = velocity * rb.mass; // impulse = deltaV * mass
        rb.AddForce(impulse, ForceMode.Impulse);

        // Option 2 (alternate): use a multiplier (tunable)
        // rb.AddForce(velocity * impulseMultiplier, ForceMode.Impulse);

        rb.angularVelocity = Random.insideUnitSphere * 2f;
    }

}
