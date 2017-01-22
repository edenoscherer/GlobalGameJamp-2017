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

    private bool isWalking;
    private bool isInFloor;
    private bool isInWall;
    private bool isAlive;
    private bool turnedToRight;

    private float axis;
    public float speed;
    public float leapForce;
    public float validRadiusFloor;
    public float validRadiusWall;

    // Use this for initialization
    void Awake()
    {

        this.rb = GetComponent<Rigidbody2D>();
        this.tr = GetComponent<Transform>();
        this.an = GetComponent<Animator>();

        this.isAlive = true;
        this.turnedToRight = true;
    }

    // Update is called once per frame
    void Update()
    {

        this.isInFloor = Physics2D.OverlapCircle(this.checkGround.position, this.validRadiusFloor, this.solid);
        this.isInWall = Physics2D.OverlapCircle(this.checkWall.position, this.validRadiusWall, this.solid);

        if (this.isAlive)
        {

            this.axis = Input.GetAxisRaw("Horizontal");

            this.isWalking = Mathf.Abs(this.axis) > 0;

            if (this.axis > 0f && !this.turnedToRight)
                this.Flip();
            else if (this.axis < 0f && this.turnedToRight)
                this.Flip();

            if (Input.GetButtonDown("Jump") && this.isInFloor)
                this.rb.AddForce(this.tr.up * this.leapForce);

        }

        this.Animations();

    }

    private void FixedUpdate()
    {

        if (this.isWalking && !this.isInWall)
        {
            if (this.turnedToRight)
                this.rb.velocity = new Vector2(this.speed, this.rb.velocity.y);
            else
                this.rb.velocity = new Vector2(-this.speed, this.rb.velocity.y);
        }

    }

    void Flip()
    {

        this.turnedToRight = !this.turnedToRight;

        this.tr.localScale = new Vector2(-this.tr.localScale.x, this.tr.localScale.y);

    }

    void Animations()
    {

        this.an.SetBool("Walking", (this.isInFloor && this.isWalking));
        this.an.SetBool("Jump", !this.isInFloor);
        this.an.SetBool("Dead", !this.isAlive);
    }

    void OnDrawGizmosSelected()    {        Gizmos.color = Color.red;        Gizmos.DrawWireSphere(this.checkGround.position, this.validRadiusFloor);        Gizmos.DrawWireSphere(this.checkWall.position, this.validRadiusWall);    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Hello Enemies");
        Debug.Log("Hello " + collider.gameObject.CompareTag("Enemies"));
        if (collider.gameObject.CompareTag("Enemies"))
            this.HitByEnemies(collider);
    }

    void HitByEnemies(Collider2D collider)
    {
        this.isAlive = false;
    }

}

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
