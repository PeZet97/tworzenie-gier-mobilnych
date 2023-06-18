//Śledzi aktualny stan gry: punkty, życia, stan obiektu Gracz (czy żyje, czy nie)
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f; 
    public float respawnInvulnerabilityTime = 3.0f;
    public int lives = 3;
    public int score = 0;

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if (asteroid.size < 0.6f)
        {
            this.score += 50; // punkty za male asteroidy
        }
        else if (asteroid.size < 1.2f)
        {
            this.score += 25; // punkty za srednie asteroidy
        }
        else
        {
            this.score += 10; // punkty za male asteroidy
        }
    }

    public void PlayerDied() //Obiekt Player informuje Managera, że nie żyje.
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();

        this.lives--;

        if (this.lives <= 0) { //jeśli liczba lives jest równa 0, następuje Game Over.
            GameOver();
        } else {
            Invoke (nameof(Respawn), this.respawnTime);  // jeśli nie jest równa 0, następuje respawn.
        } 
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
       
    }

}
