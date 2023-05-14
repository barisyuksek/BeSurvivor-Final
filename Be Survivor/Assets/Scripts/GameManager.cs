using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameActive;
    [SerializeField] private GameObject[] zombies;
    [SerializeField] private Transform[] zombieSpawnPoints;
    [SerializeField] private Transform[] projectileSpawnPoints;
    [SerializeField] private AudioSource zombieSource;
    [SerializeField] private GameObject projectile;
    private int score;
    public static GameManager instance;
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

    public void GameQuit()
    {
        Debug.Log("Oyundan cikis yapildi");
        Application.Quit();
    }

    public void GameStart()
    {
        zombieSource.Play();
        gameActive = true;
        UIManager.instance.GamePanel();
        InvokeRepeating(nameof(EnemyCloner), 1, 3); // 3 saniye aralýklarla düþman clonla
        InvokeRepeating(nameof(ProjectileSpawner), 1, 15); // 15 saniye aralýklarla mermi üret
    }
    public void GameOver()
    {
        if(gameActive)
        {
            AudioManager.instance.PlayAudio(AudioManager.AudioCallers.End);
            zombieSource.Stop();
            gameActive = false;
            UIManager.instance.EndPanel(score);
            CancelInvoke(nameof(EnemyCloner));
            CancelInvoke(nameof(ProjectileSpawner));
        }
    }
    private void EnemyCloner()
    {
        int z = Random.RandomRange(0, zombies.Length);
        int s = Random.Range(0, zombieSpawnPoints.Length);
        Instantiate(zombies[z], zombieSpawnPoints[s].position, Quaternion.identity);
    }
    private void ProjectileSpawner()// Spawn noktalarýnda verilen süre boyunca mermi üretip yok eder
    {
        for(int i = 0; i < projectileSpawnPoints.Length; i++)
        {
            GameObject p = Instantiate(projectile, projectileSpawnPoints[i].position + Vector3.up * 0.5f + Vector3.left, Quaternion.identity);
            Destroy(p, 15f);
        }
    }
    public void Score()
    {
        UIManager.instance.ScoreText(++score);
    }
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
}
