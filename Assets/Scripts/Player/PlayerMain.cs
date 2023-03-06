using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{
    [Header("Variables")]

    public float moveSpeed = 750f;

    public float camDistance = 10;

    public float camDistanceCap = 35;

    public float camDistanceFloor = 10;

    public float maxHealth = 100f;

    public float health = 15f;

    public float healthSubtractionRate = 2f;

    public float healthSubtractionAmount = 5f;

    public int timeAlive = -1;

    public int highScore = 0;

    public bool isAlive = true;

    public bool hasSpeed = false;

    public bool canRestart = false;

    public bool invincible = false;

    public string highScoreID;

    [Header("References")]

    public Rigidbody rb;

    public AudioClip deathSFX, speedBoostEnd, healthUp, speedUp, invinceStart, invinceEnd;

    public AudioSource audioSource, audioPowerSFX;

    public Material normalMat, invincibleMat;

    public GameObject playerObj;

    Vector3 movement;

    public TMP_Text healthTextUI, timerTextUI, bestTimeTextUI, finalTimeTextUI, highScoreTextUI;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(SubtractHealth());

        StartCoroutine(DeathTimer());

        highScoreID = Application.loadedLevelName + "HighScore";

        highScore = PlayerPrefs.GetInt(highScoreID);

        highScoreTextUI.color = new Color(1f, 1f, 1f, 0f);

        bestTimeTextUI.text = "Best Time: " + highScore;

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.z = Input.GetAxisRaw("Vertical");
            Health();
        }

        healthTextUI.text = "Health: " + health;

        timerTextUI.text = "Time Alive: " + timeAlive;


        if(Input.GetKeyDown(KeyCode.K))
        {
            health = 0;
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;

            SceneManager.LoadScene("MainMenu");
        }

        if (!isAlive && Input.anyKey && canRestart)
        {

            Application.LoadLevel(Application.loadedLevel);
        }

        if (hasSpeed)
        {
            camDistance = Mathf.Lerp(camDistance, camDistanceCap, Time.deltaTime);
        }

        else if (!hasSpeed)
        {
            camDistance = Mathf.Lerp(camDistance, camDistanceFloor, Time.deltaTime);

        }
    }

    void FixedUpdate()
    {
        //rb.AddForce(rb.position + movement * moveSpeed * Time.fixedDeltaTime, ForceMode.Impulse);

        rb.velocity = new Vector3(movement.x, 0, movement.z).normalized * moveSpeed * Time.fixedDeltaTime;

    }

    //Custom Functions

    void Health()
    {

        if (health <= 0 && isAlive)
        {
            DeathHandler();
        }
    }

    public void StartInvincibility(int duration)
    {
        health += 20;

        if(health > 100)
        {
            health = maxHealth;
        }

        CheckHealth();

        audioPowerSFX.PlayOneShot(invinceStart);

        StartCoroutine(Invincibility(duration));
    }

    void DeathHandler()
    {
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;

        isAlive = false;

        audioSource.volume = 0.75f;
        audioSource.PlayOneShot(deathSFX);

        StopCoroutine(DeathTimer());
        StopCoroutine(SubtractHealth());

        movement.x = 0;
        movement.z = 0;

        Debug.Log("You have Died!");

        if(hasSpeed)
        {
            StopSpeed();
        }

        if (timeAlive > highScore)
        {
            PlayerPrefs.SetInt(highScoreID, timeAlive);

            highScore = PlayerPrefs.GetInt(highScoreID);

            highScoreTextUI.color = new Color(1f, 1f, 0f, 0f);

            bestTimeTextUI.text = "Best Time: " + highScore;

        }

        StartCoroutine(TextFadeOut(2,2));

        StartCoroutine(TextFadeIn(2));

        StartCoroutine(RestartDelay());
    }

    public void AddHealth(float amount)
    {
        health += amount;

        audioPowerSFX.volume = 0.75f;
        audioPowerSFX.PlayOneShot(healthUp);

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        
        CheckHealth();
    }

    public void TempSpeed(int duration, int amount)
    {
        hasSpeed = true;

        audioPowerSFX.volume = 0.4f;
        audioPowerSFX.PlayOneShot(speedUp);

        StartCoroutine(SpeedBoost(duration, amount));
    }

    public void StopSpeed()
    {
        hasSpeed = false;

        audioSource.volume = 0.4f;
        audioSource.PlayOneShot(speedBoostEnd);


        moveSpeed = 850;
    }

    public void CheckHealth()
    {
        if (health > 50)
        {
            healthTextUI.color = Color.green;
        }

        else if (health <= 50 && health > 15)
        {
            healthTextUI.color = Color.yellow;
        }

        else
        {
            healthTextUI.color = Color.red;
        }
    }

    IEnumerator SubtractHealth()
    {
        if(isAlive && !invincible)
        {
            health -= healthSubtractionAmount;

            CheckHealth();

            yield return new WaitForSeconds(healthSubtractionRate);

            StartCoroutine(SubtractHealth());
        }

        else
        {
            yield return null;
        }

    }

    IEnumerator SpeedBoost(int duration, int amount)
    {
        moveSpeed = amount;

        yield return new WaitForSeconds(duration);

        StopSpeed();
    }

    IEnumerator Invincibility(int duration)
    {
        invincible = true;

        playerObj.GetComponent<MeshRenderer>().material = invincibleMat;

        yield return new WaitForSeconds(duration);

        playerObj.GetComponent<MeshRenderer>().material = normalMat;

        invincible = false;

        audioPowerSFX.PlayOneShot(invinceEnd);

        StartCoroutine(SubtractHealth());
    }

    IEnumerator DeathTimer()
    {
        timeAlive += 1;

        if(timeAlive > highScore)
        {
            timerTextUI.color = new Color(1, 1, 0, 1);
        }

        yield return new WaitForSeconds(1);

        if(isAlive)
        {
            StartCoroutine(DeathTimer());
        }
        
    }

    IEnumerator TextFadeOut(int outTime, int inTime)
    {
        float textAlpha = healthTextUI.alpha;

        while (healthTextUI.alpha > 0 && !invincible)
        {
            healthTextUI.alpha = textAlpha -= 1 * Time.deltaTime / outTime;

            timerTextUI.alpha = textAlpha -= 1 * Time.deltaTime / outTime;

            bestTimeTextUI.alpha = textAlpha -= 1 * Time.deltaTime / outTime;

            yield return null;
        }
    }

    IEnumerator TextFadeIn(int inTime)  
    {
        yield return new WaitForSeconds(inTime);

        float finalTextAlpha = finalTimeTextUI.alpha;


        if(highScore == 1)
        {
            highScoreTextUI.text = "Longest Time: " + highScore + " Second";
        }

        else
        {
            highScoreTextUI.text = "Longest Time: " + highScore + " Seconds";
        }


        if(timeAlive == 1)
        {
            finalTimeTextUI.text = "You Survived: " + timeAlive + " Second";
        }

        else
        {
            finalTimeTextUI.text = "You Survived: " + timeAlive + " Seconds";
        }
        

        while (finalTimeTextUI.alpha < 255)
        {
            finalTimeTextUI.alpha = finalTextAlpha += 1 * Time.deltaTime / inTime;

            highScoreTextUI.alpha = finalTextAlpha += 1 * Time.deltaTime / inTime;

            yield return null;
        }
    }

    IEnumerator RestartDelay()
    {
        yield return new WaitForSeconds(3);

        canRestart = true;
    }
}
