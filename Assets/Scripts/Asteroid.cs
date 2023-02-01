using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite [] sprites;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 50.0f;
    public float maxLifetime = 30.0f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;

        _rigidbody.mass = this.size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.speed);

        Destroy(this.gameObject, this.maxLifetime);
    }
    //Niszczenie Asteroid - rozbijanie je na dwie mniejsze połówki.
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "Bullet") //Sprawdza kolizję z obiektem Bullet, jeśli występuje, Asteroida rozłamuje się na pół.
       {
            if((this.size * 0.5f) >= this.minSize) //Asteroida przełamuje się tylko jeśli jej wielkość podzielona na pół mieści się w dozwolonej wiekości.
            {
                CreateSplit();
                CreateSplit();
            }

            Destroy(this.gameObject); //Model Asteroidy przed rozłamem zostaje usunięty.
       }
    } 

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f; // Asteroidy pojawiają się w miejscu rozłamu na pół, z lekką różnicą, aby wydawało się to bardziej naturalne.

        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f; 
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed); // Po przedzieleniu na pół, asteroidy lecą w inną stronę.
    }
}
