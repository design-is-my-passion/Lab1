using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement: MonoBehaviour
{
    public float speed = 10;
    public float maxSpeed = 20;

    public float upSpeed = 10;
    private bool onGroundState = true;

    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public GameObject enemies;

    private Vector3 marioInitalPosition;

    public Canvas normalUI;
    public Canvas gameOver;
    public TextMeshProUGUI gameOverText;

    public TimeController timeController;
    public CameraController gameCamera;

    public GameObject flagText;
    public GameObject normalText;

    public Animator marioAnimator;




    void Start()
    {
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioInitalPosition = marioBody.transform.position;

        marioSprite = GetComponent<SpriteRenderer>();

        marioAnimator.SetBool("onGround", onGroundState);

    }

  void Update()
  {

    if (Input.GetKeyDown("a") && faceRightState)
    {
      faceRightState = false;
      marioSprite.flipX = true;
    }

    if (Input.GetKeyDown("d") && !faceRightState)
    {
      faceRightState = true;
      marioSprite.flipX = false;
    }

    if (marioBody.linearVelocity.x > 0.1f)
        marioAnimator.SetTrigger("onSkid");

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
        }

        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.1f)
                marioAnimator.SetTrigger("onSkid");
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.1f)
                marioAnimator.SetTrigger("onSkid");
        }

        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));

        if (Input.GetKeyDown("space") && onGroundState) {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
        }
    }

  void OnCollisionEnter2D(Collision2D col)
  {
    if (col.gameObject.CompareTag("Flag")) {
      flagText.SetActive(true);
      normalText.SetActive(false);
      GameOver();
    }

    if (col.gameObject.CompareTag("Ground")) onGroundState = true;

    if (col.gameObject.CompareTag("Enemy"))
    {
      Debug.Log("Collided with goomba!");
      GameOver();
    }
  }

  private void GameOver()
  {
    Time.timeScale = 0.0f;
    gameOverText.text = GetComponent<JumpOverGoomba>().scoreText.text;
    normalUI.enabled = false;
    gameOver.enabled = true;
  }

  public void RestartButtonCallback(int input)
  {
      Debug.Log("Restart!");
      // reset everything
      ResetGame();
      // resume time
      Time.timeScale = 1.0f;
      normalUI.enabled = true;
      gameOver.enabled = false;

      flagText.SetActive(false);
      normalText.SetActive(true);

      timeController.ClearRecord();
  }

  private void ResetGame()
  {
    // reset position
    marioBody.transform.position = marioInitalPosition;
    marioBody.linearVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    GetComponent<JumpOverGoomba>().score = 0;
    GetComponent<JumpOverGoomba>().scoreText.text = "Score: 0";

    // reset sprite direction
    faceRightState = true;
    marioSprite.flipX = false;
    // reset Goomba
    foreach (Transform eachChild in enemies.transform)
    {
        eachChild.GetComponent<EnemyMovement>().ResetPos();
    }

    // reset camera position
    gameCamera.ResetCamera();
  }

}
