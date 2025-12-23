using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerScene : MonoBehaviour
{
    public static ManagerScene instance;

    [Header("Loading UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;

    [SerializeField] private float loadingSmoothSpeed = 5f;
    private float currentProgress = 0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneByID(sceneId));
    }

    private IEnumerator LoadSceneByID(int sceneId)
    {

        //Debug.Log("loading scene started");

        loadingScreen.SetActive(true);

        var operation = SceneManager.LoadSceneAsync(sceneId);
        operation.allowSceneActivation = false;
        var val = 0;
        while (loadingBar.value<.9)
        {
            val++;
            loadingBar.value = Mathf.Lerp(0, val, Time.deltaTime * loadingSmoothSpeed);
            
            //Debug.Log($"progress {operation.progress}");

            //loadingBar.value = operation.progress / 0.9f;

            yield return null;
        }

        //Debug.Log("scene loaded to 90 %");
        
        yield return new WaitForSeconds(4f);

        //Debug.Log("allowing scene activation");
        operation.allowSceneActivation=true;


        while (!operation.isDone)
            yield return null;

        loadingScreen.SetActive(false);
        //Debug.Log("loading screen disabled");      
    }

    public void GoHome()
    {
        SceneManager.LoadSceneAsync(0);
        AudioManager.Instance.PlayMenuMusic();
    }
}
