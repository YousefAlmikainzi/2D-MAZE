using System.Collections;
using UnityEngine;
using UnityEngine.UI;


//this script exaplains the character's movement amongst other things
public class PlayerMovement : MonoBehaviour
{
    public AudioClip winSound;
    public AudioSource audioSource; 
    public float moveSpeed; 
    public Rigidbody2D rb; 
    private Vector2 moveDir; 
    public float timerDuration = 60f; 
    private float timer; 

    public Text timerText; 
    public Text gameOverText; 
    public Text victoryText; 
    private bool isSwallowed = false; 
    private bool hasWon = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        gameOverText.gameObject.SetActive(false); 
        victoryText.gameObject.SetActive(false); 
        Time.timeScale = 1f; 

        timer = timerDuration; 
        UpdateTimerText(); 

        InvokeRepeating("CountdownTimer", 1f, 1f); 
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical"); 
        moveDir = new Vector2(moveX, moveY).normalized; 
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("wizard"))
        {
            timer += 15f;  
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("goal"))
        {
            if (!isSwallowed)
            {
                StopTimer(); 
                hasWon = true; 
                Win(); 
                StartCoroutine(SwallowBall()); 
            }
        }
    }

    private IEnumerator SwallowBall()
    {
        isSwallowed = true; 

        float scale = transform.localScale.x; 
        while (scale > 0)
        {
            scale -= Time.deltaTime; 
            transform.localScale = new Vector3(scale, scale, scale); 
            yield return null; 
        }

        Destroy(gameObject); 
    }

    private void CountdownTimer()
    {
        timer--; 
        if (timer <= 0)
        {
            StopTimer(); 
            if (!isSwallowed)
                Lose(); 
        }

        UpdateTimerText(); 
    }

    private void StopTimer()
    {
        CancelInvoke("CountdownTimer"); 
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f); 
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); 

        if (timer <= 15)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.green; 
        }
    }

    private void Win()
    {
        StartCoroutine(SwallowBall()); 
        victoryText.gameObject.SetActive(true); 
        Time.timeScale = 0f; 

        audioSource.clip = winSound; 
        audioSource.Play(); 
    }

    private void Lose()
    {
        gameOverText.gameObject.SetActive(true); 
        Time.timeScale = 0f; 
    }
}
