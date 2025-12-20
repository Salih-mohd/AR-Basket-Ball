using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Vector3 ballPosition;

    [Header("Throw Tuning")]
    [SerializeField] private float minSwipePixels = 50f;
    [SerializeField] private float maxSwipeDistance = 600f;
    [SerializeField] private float maxHoldTime = 1.2f;

    [SerializeField] private float forwardPower = 18f;
    [SerializeField] private float extraUpForceMax = 8f;

    [SerializeField] private float sideInfluence = 1f;
    [SerializeField] private float verticalInfluence = 1f;

    // runtime
    private bool isInFlight;
    private Vector3 throwStartPosition;


    private void Awake()
    {
        rb.isKinematic = true;
    }
    public void Throw(Vector2 startPos, Vector2 endPos, float holdTime)
    {
        if (isInFlight) return;

        Vector2 swipe = endPos - startPos;
        float swipeDistance = swipe.magnitude;

        if (swipeDistance < minSwipePixels) return;


        // clambing distance and time
        float distance01 = Mathf.Clamp01(swipeDistance / maxSwipeDistance);
        float time01 = Mathf.Clamp01(holdTime / maxHoldTime);



        // dealing direction
        Vector3 dir =
            cam.forward +
            cam.right * (swipe.x / Screen.height) * sideInfluence +
            cam.up * (swipe.y / Screen.height) * verticalInfluence;

        dir.Normalize();



        // dealing force
        float forwardForce = forwardPower * distance01;
        float upForce = time01 * extraUpForceMax;

        Vector3 finalForce =
            (dir * forwardForce) +
            (Vector3.up * upForce);

        ExecuteThrow(finalForce);
    }

    private void ExecuteThrow(Vector3 force)
    {
        rb.isKinematic = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 impulse = force * rb.mass;
        rb.AddForce(impulse, ForceMode.Impulse);

        rb.angularVelocity = Random.insideUnitSphere * 2f;

        throwStartPosition = rb.position;
        //isInFlight = true;
        Debug.Log("Throwed");



        // calling shot
        ShotManager.instance.StartShot(ballPrefab);
    }

    
    public void Backup()
    {
        rb.isKinematic = true;
        ballPrefab.transform.localPosition = ballPosition;
    }
}
