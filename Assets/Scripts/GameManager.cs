using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// O'yinning asosiy boshqaruvchisi: ball, jon, saqlash, UI
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // boshqa skriptlar GameManager.Instance orqali murojaat qiladi

    [Header("Sozlamalar")]
    public int maxHealth = 5;

    [Header("UI")]
    public Text scoreText;
    public Text bestText;
    public Text healthText;
    public Text enemyStateText;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public Button restartButton;

    [Header("Havolalar")]
    public EnemyCar enemy;
    public PlayerCar player;

    public PlayerData data;   // JSON dan yuklangan ma'lumot
    public bool isGameOver;

    int score;
    int health;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        data = SaveSystem.Load();      // o'yin boshlanganda saqlangan ma'lumotni yuklaymiz
        health = maxHealth;
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(Restart);
        UpdateUI();
    }

    void Update()
    {
        // Dushmanning hozirgi holatini ekranda ko'rsatamiz
        if (enemy != null)
            enemyStateText.text = "Dushman: " + enemy.GetStateName();
    }

    public void AddScore(int amount)
    {
        score += amount;
        data.totalCoins += amount;

        if (score > data.bestScore)
            data.bestScore = score; // yangi rekord

        SaveSystem.Save(data);
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver) return;

        health -= damage;
        player.ShowHit();

        if (health <= 0)
        {
            health = 0;
            GameOver();
        }
        UpdateUI();
    }

    // Reklama ko'rilgandan keyin beriladigan bonus
    public void GiveAdReward(int bonusCoins)
    {
        if (isGameOver)
        {
            // O'yin tugagan bo'lsa - davom ettirish imkoniyati
            isGameOver = false;
            gameOverPanel.SetActive(false);
        }

        health = maxHealth;   // jon to'ldiriladi
        AddScore(bonusCoins); // bonus ball qo'shiladi
        Debug.Log("Reklama bonusi berildi: +" + bonusCoins + " ball, jon to'ldirildi");
    }

    void GameOver()
    {
        isGameOver = true;
        data.gamesPlayed++;
        SaveSystem.Save(data);

        gameOverPanel.SetActive(true);
        gameOverText.text = "O'YIN TUGADI!\n\nBall: " + score + "\nRekord: " + data.bestScore +
                            "\n\nReklama ko'rib davom eting yoki qayta boshlang";
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateUI()
    {
        scoreText.text = "Ball: " + score;
        bestText.text = "Rekord: " + data.bestScore + "   |   Jami tanga: " + data.totalCoins +
                        "   |   O'yinlar: " + data.gamesPlayed;
        healthText.text = "Jon: " + health + " / " + maxHealth;
    }

    void OnApplicationQuit()
    {
        SaveSystem.Save(data);
    }
}
