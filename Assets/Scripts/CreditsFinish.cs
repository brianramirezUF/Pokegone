using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsFinish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(creditsWait());
    }

    IEnumerator creditsWait()
    {
        yield return new WaitForSeconds(20);
        SceneManager.LoadScene(0);
    }
}
