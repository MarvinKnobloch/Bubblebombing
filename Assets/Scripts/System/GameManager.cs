using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MenuController menuController;
    public PlayerUI playerUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;

    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            //AudioManager.Instance.StartMusicFadeOut((int)AudioManager.MusicSongs.Menu, true, 0.1f, 1);
        }
        //if (Player.Instance == null) return;

        //if (SceneManager.GetActiveScene().buildIndex == 1)
        //{
        //    //AudioManager.Instance.SetSong((int)AudioManager.MusicSongs.Tutorial);
        //    AudioManager.Instance.StartMusicFadeOut((int)AudioManager.MusicSongs.Tutorial, true, 0.1f, 1);
        //}
        //else if (SceneManager.GetActiveScene().buildIndex == 2)
        //{
        //    AudioManager.Instance.StartMusicFadeOut((int)AudioManager.MusicSongs.FireArea, true, 0.1f, 1);
        //}
    }
    public void ActivateCursor()
    {
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }
    public void DeactivateCursor()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
}
