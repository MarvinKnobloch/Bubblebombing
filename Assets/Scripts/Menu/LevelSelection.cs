using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private SceneEnum.Scenes level;

    public void LoadLevel()
    {
        //AudioManager.Instance.PlayUtilityOneshot((int)AudioManager.UtilitySounds.MenuSelect);
        GameManager.Instance.menuController.gameIsPaused = false;
        GameManager.Instance.menuController.ResetTimeScale();
        SceneManager.LoadScene((int)level);
    }

}
