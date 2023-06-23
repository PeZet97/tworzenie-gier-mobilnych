//Śledzi aktualny stan gry: punkty, życia, stan obiektu Gracz (czy żyje, czy nie)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f;
    public float respawnInvulnerabilityTime = 3.0f;
    public int lives = 3;
    public int score = 0;
    public GameObject gameOverUI;

    public Text scoreText;
    public Text livesText;

    private int currentLevel = 1;
    private int [] levelScoreRequirements = { 1000, 2000 };

    private void Start()
    {
        SetScore(0);
        SetLives(3);
    }

    public void AsteroidDestroyed(Asteroid asteroid) //niszczenie asteroid
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if (asteroid.size < 0.6f)
        {
            SetScore(score += 100); // punkty za małe asteroidy
        }
        else if (asteroid.size < 1.2f)
        {
            SetScore(score += 75);  // punkty za srednie asteroidy
        }
        else
        {
            SetScore(score += 50); // punkty za duże asteroidy
        }

    }


    public void PlayerDied() //Obiekt Player informuje Managera, że nie żyje.
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();

        SetLives(lives - 1);

        if (this.lives <= 0)
        { //jeśli liczba lives jest równa 0, następuje Game Over.
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), this.respawnTime);  // jeśli nie jest równa 0, następuje respawn.
        }
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();

        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
        }

        gameOverUI.SetActive(false);

        SetScore(0);
        SetLives(3);
        Respawn();
    }

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero; //Obiekt Player respawnuje się w centrum sceny.
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions"); //Po respawnie Playera, zmienia się jego layer
        this.player.gameObject.SetActive(true); //Obiek Player respawnuje się, status gameObject zmienia się na true.
        Invoke(nameof(TurnOnCollisions), this.respawnInvulnerabilityTime);
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();

        if (currentLevel <= levelScoreRequirements.Length && score >= levelScoreRequirements[currentLevel - 1])

        {
            LoadNextLevel();
        }

        if (currentLevel <= levelScoreRequirements.Length)
        {
            if (score >= levelScoreRequirements[currentLevel - 1])
            {
                if (currentLevel ==2 && score >= 2000)
                {
                    LoadNextLevel();
                }
            }
        }
        
    }

    private void LoadNextLevel()
    {
        currentLevel ++;

        SetScore(0);
        SetLives(3);

        string sceneName = "";
        switch (currentLevel)
        {
            case 2:
                sceneName = "Trash";
                break;
            case 3:
                sceneName = "Flowers";
                break;
        
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }

}