using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
     void Update()
    {

    }

    public void ChangePlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void ChangeOpeningVideoScene()
    {
        SceneManager.LoadScene("OpeningVideoScene");
    }

    public void ChangeGameScene()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    public void ChangeSettingScene()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("SettingScene");
        }
    }

    public void OnClickExit()
    {
        Application.Quit();
        Debug.Log("Get Out This Game");
    }
}
