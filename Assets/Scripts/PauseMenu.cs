using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject camera;
    public GameManager gameManager;
    public AudioSource music;
    public AudioSource bossmusic;
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && camera.active == true)
        {
            menu.SetActive(true);
            music.Pause();
            if(bossmusic != null)
            {
                bossmusic.Pause();
            }
            gameManager.DisablePlayerMovement(true);
        }
    }

    public void Resume()
    {
        menu.SetActive(false);
        music.UnPause();
        if (bossmusic != null)
        {
            bossmusic.UnPause();
        }
        gameManager.DisablePlayerMovement(false);
    }

    public void Quit()
    {
        StartCoroutine(loadWait(0));
    }

    IEnumerator loadWait(int sceneIndex)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneIndex);
    }
}
