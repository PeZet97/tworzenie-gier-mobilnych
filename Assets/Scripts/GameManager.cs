//Śledzi aktualny stan gry: punkty, życia, stan obiektu Gracz (czy żyje, czy nie)
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public float respawnTime = 3.0f;
    public float respawnInvulnerabilityTime = 3.0f;
    public int lives = 3;

    public void PlayerDied() //Obiekt Player informuje Managera, że nie żyje.
    {
        this.lives--;

        if (this.lives <= 0) {
            GameOver();
        } else {
            Invoke (nameof(Respawn), this.respawnTime);    
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
        // TODO
    }

}