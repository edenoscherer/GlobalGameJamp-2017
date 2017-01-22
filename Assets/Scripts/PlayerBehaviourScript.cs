using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{

    private Rigidbody2D rb;
    private Transform tr;
    private Animator an;

    public LayerMask solid;

    public Transform checkGround;
    public Transform checkWall;

    public AudioSource footstepsSound;

    private bool isWalking;
    private bool isInFloor;
    private bool isInWall;
    private bool isAlive;
    private bool turnedToRight;
    private bool playingFootstepsSound;

    private float axis;
    public float speed;
    public float leapForce;
    public float validRadiusFloor;
    public float validRadiusWall;

    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        an = GetComponent<Animator>();

        isAlive = true;
        turnedToRight = true;
        this.playingFootstepsSound = false;
    }

    // Update is called once per frame
    void Update()
    {

        isInFloor = Physics2D.OverlapCircle(checkGround.position, validRadiusFloor, solid);
        isInWall = Physics2D.OverlapCircle(checkWall.position, validRadiusWall, solid);

        if (isAlive)
        {

            axis = Input.GetAxisRaw("Horizontal");

            isWalking = Mathf.Abs(axis) > 0;

            if (axis > 0f && !turnedToRight)
                Flip();
            else if (axis < 0f && turnedToRight)
                Flip();

            if (Input.GetButtonDown("Jump") && isInFloor)
                rb.AddForce(tr.up * leapForce);


            Animations();
        }

    }

    void FixedUpdate()
    {

        if (this.isWalking && !this.isInWall)
        {
            if (this.turnedToRight)
                rb.velocity = new Vector2(speed, rb.velocity.y);
            else
                rb.velocity = new Vector2(-speed, rb.velocity.y);
        }

        this.ControlFootstepsSound();
    }

    //----------------------------------------------------------------------------------------------------------------

    void Flip()
    {

        turnedToRight = !turnedToRight;

        tr.localScale = new Vector2(-tr.localScale.x, tr.localScale.y);

    }

    void Animations()
    {

        an.SetBool("Walking", (isInFloor && isWalking));
        an.SetBool("Jump", !isInFloor);
    }

    /**
     * Controla a reprodução do som dos passo
     * 
     * @param walking Flag que indica se o personagem esta ou não andando
     */
    private void ControlFootstepsSound()
    {
        if (this.isWalking && this.isInFloor && !this.playingFootstepsSound)
        {
            this.footstepsSound.Play();
            this.playingFootstepsSound = true;
        }
        else if ((!this.isWalking || !this.isInFloor) && this.playingFootstepsSound)
        {
            this.footstepsSound.Stop();
            this.playingFootstepsSound = false;
        }
    }
}
