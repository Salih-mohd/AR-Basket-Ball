using UnityEngine;

public class BallHitTesting : MonoBehaviour
{

    public GameObject cubePrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            ScoreManager.Instance.OnMiss();

        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;

        if(cubePrefab!=null)
            Instantiate(cubePrefab, hitPoint, Quaternion.identity);
    }
}
