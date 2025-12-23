using System.Collections;
using UnityEngine;

public class HoopScoreTrigger : MonoBehaviour
{
    public GameObject scoreSpawnPos;

    public Vector3 positionOfBall;
    private void OnTriggerEnter(Collider other)
    {
        //if (!other.CompareTag("Ball")) return;

        Rigidbody ballRb = other.GetComponent<Rigidbody>();
        if (ballRb == null) return;

        // Ensure ball is moving downward (prevents cheating)
        if (ballRb.linearVelocity.y < 0f)
        {

            ShotManager.instance.RegisterScore(scoreSpawnPos);
            
            //Debug.Log("called score methdo");
            StartCoroutine(SettingOff(other.gameObject));

        }
    }


    IEnumerator SettingOff(GameObject obj)
    {
        yield return new WaitForSeconds(.5f);
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.localPosition = positionOfBall;
        
    }
}

