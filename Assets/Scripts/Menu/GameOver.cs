using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Image gameOverField;
    private Color backgroundColor;
    [SerializeField] private GameObject resetButton;
    private float timer;
    [SerializeField] private float timeUntilGameover;

    [SerializeField] private TextMeshProUGUI gameOvertext;
    //[TextArea][SerializeField] private string gameOverMessage;
    private Color gameOverTextColor;

    private void Awake()
    {
        backgroundColor = Color.black;
        backgroundColor.a = 0;
        gameOverField.color = backgroundColor;

        gameOverTextColor = Color.white;
        gameOverTextColor.a = 0 + 0.25f;
        gameOvertext.faceColor = gameOverTextColor;
        //gameOvertext.text = gameOverMessage;

        resetButton.SetActive(false);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        float time = timer / timeUntilGameover;
        backgroundColor.a = time;
        gameOverField.color = backgroundColor;

        gameOverTextColor.a = time + 0.25f;
        gameOvertext.faceColor = gameOverTextColor;

        if (backgroundColor.a > 0.98)
        {
            resetButton.SetActive(true);
            GameManager.Instance.menuController.TimeScaleToZero();
        }
    }
    public void RestartGame()
    {
        GameManager.Instance.menuController.ResetTimeScale();
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
