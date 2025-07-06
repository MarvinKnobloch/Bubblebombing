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
        SceneManager.LoadScene((int)level);
    }

}
