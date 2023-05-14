using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel, menuPanel, pausePanel, endPanel;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private TextMeshProUGUI bestScoreText, endScoreText, gameScoreText, projectileText;
    private GameObject currentPanel;
    public static UIManager instance;
    
    private void Awake()
    {
   
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentPanel = menuPanel;
        bestScoreText.text = PlayerPrefs.GetInt("bestScore", 0).ToString(); //Hafýzadan bestScore deðerini alýp texte yazar
    }
    public void GamePanel() //aktif panel kapanýr gamePanel açýlýr
    {
        currentPanel.SetActive(false);
        gamePanel.SetActive(true);
        currentPanel = gamePanel;
    }
    public void MenuPanel() 
    {
        currentPanel.SetActive(false);
        menuPanel.SetActive(true);
        currentPanel = menuPanel;
    }
    public void EndPanel(int score)
    {
        currentPanel.SetActive(false);
        endPanel.SetActive(true);
        currentPanel = endPanel;
        endScoreText.text = "Score: " + score.ToString();
        if(score > PlayerPrefs.GetInt("bestScore", 0)) //Eðer skor hafýzadaki bestScoreden yüksekse yeni bestScoreyi kaydeder ve texte yazar
        {
            PlayerPrefs.SetInt("bestScore", score);
            bestScoreText.text = PlayerPrefs.GetInt("bestScore").ToString();
        }
    }
    public void PausePanelActive() // Zamaný durdurup pausePaneli açar
    {
        Time.timeScale = 0; 
        pausePanel.SetActive(true);
    }
    public void PausePanelInactive()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    public void ScoreText(int score)
    {
        gameScoreText.text = score.ToString();
    }
    int heartIndex=2;
    public IEnumerator Damaged()
    {
        hearts[heartIndex--].SetActive(false); // Can azaldýkça kalpleri sýra sýra kapar
        gamePanel.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gamePanel.GetComponent<Image>().color = new Color(0,0,0,0);
    }
    public void ProjectileText(int projectile)
    {
        projectileText.text = projectile.ToString();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanelActive();
        }
    }
}
