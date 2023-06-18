//Śledzi aktualny stan gry: punkty, życia, stan obiektu Gracz (czy żyje, czy nie)
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        SetScore(0);
        SetLives(3);
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if (asteroid.size < 0.6f)
        {
            SetScore(score += 50); // punkty za male asteroidy
        }
        else if (asteroid.size < 1.2f)
        {
            SetScore(score += 25);  // punkty za srednie asteroidy
        }
        else
        {
            SetScore(score += 10); // punkty za male asteroidy
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
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }

}