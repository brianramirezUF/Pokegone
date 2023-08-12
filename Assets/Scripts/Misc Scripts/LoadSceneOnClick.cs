using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public void LoadByIndex(int sceneIndex)
    {
        //SceneManager.LoadScene(sceneIndex);
        StartCoroutine(loadWait(sceneIndex));
    }

    IEnumerator loadWait(int sceneIndex)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneIndex);
    }
}
