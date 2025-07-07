using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private Scenes level;

    public void LoadLevel()
    {
        //AudioManager.Instance.PlayUtilityOneshot((int)AudioManager.UtilitySounds.MenuSelect);
		if (GameManager.Instance.menuController != null)
		{
			GameManager.Instance.menuController.gameIsPaused = false;
			GameManager.Instance.menuController.ResetTimeScale();
		}

        if (level == Scenes.Level1) PlayerPrefs.SetInt("ShowTutorial", 1);

        SceneManager.LoadScene((int)level);
    }

}
