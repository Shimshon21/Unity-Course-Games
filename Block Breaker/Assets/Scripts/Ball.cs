using UnityEngine;

public class Ball : MonoBehaviour
{
    // Config params
    [SerializeField] Paddle paddle1;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;
    [SerializeField] AudioClip[]ballSounds;
    [SerializeField] float randomFactor = 0.5f;


    // Cached component reference
    AudioSource m_audioSource;
    Rigidbody2D m_rigidbody;

    // state
    Vector2 paddleToBallVector;

    bool hasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;

        m_audioSource = GetComponent<AudioSource>();

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
            LockTheBallOnPaddle();

        LaunchOnMouseClick();
    }


    // Launch the ball on mouse left click
    private void LaunchOnMouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(xPush, yPush);

            hasStarted = true;
        }
        
    }

    // Attach the ball above the paddle
    private void LockTheBallOnPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);

        transform.position = (paddlePos + paddleToBallVector);
    }


    // On each collison play sound.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2
            (UnityEngine.Random.Range(0f, randomFactor),
            UnityEngine.Random.Range(0f, randomFactor));


        if (hasStarted)
        {
            AudioClip audio = ballSounds[Random.Range(0,ballSounds.Length)];
            m_audioSource.PlayOneShot(audio);
            m_rigidbody.velocity += velocityTweak;
        }

    }
}
