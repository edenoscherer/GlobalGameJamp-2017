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
        Debug.Log("Hello " + collider.gameObject.CompareTag("Enemies"), gameObject);
        if (collider.gameObject.CompareTag("Enemies"))
            this.HitByEnemies(collider);
    }

    void HitByEnemies(Collider2D collider)
    {
        this.isAlive = false;
    }

}
