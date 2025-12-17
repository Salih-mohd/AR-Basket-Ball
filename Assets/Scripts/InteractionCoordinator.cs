using UnityEngine;

public class InteractionCoordinator : MonoBehaviour
{

    public static InteractionCoordinator instance;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private HoopPlacer hoopPlacer;
    [SerializeField] private BallThrower ballThrower;

    public bool hoopPlaced;



    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        inputManager.OnTap += HandleTap;
        inputManager.OnSwipe += HandleSwipe;
    }

    private void OnDisable()
    {
        inputManager.OnTap -= HandleTap;
        inputManager.OnSwipe -= HandleSwipe;
    }

    private void HandleTap(Vector2 screenPos)
    {
        if (hoopPlaced) return;

        hoopPlacer.Place(screenPos);   
        
         
    }

    private void HandleSwipe(Vector2 start, Vector2 end, float duration)
    {
        if (!hoopPlaced) return;   

        ballThrower.Throw(start, end, duration);
        Debug.Log("called throw");
    }
}
