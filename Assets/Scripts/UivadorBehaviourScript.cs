using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UivadorBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform tr;
    private Animator an;

    public LayerMask solid;

    public Transform checkGround;
    public Transform checkWall;

    private bool isInWall;
    private bool isInFloor;
    private bool turnedToRight;

    public float speed;
    public float validRadiusFloor;
    public float validRadiusWall;

    void Awake()
    {

        this.rb = GetComponent<Rigidbody2D>();
        this.tr = GetComponent<Transform>();
        this.an = GetComponent<Animator>();
        
        this.turnedToRight = true;

    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void FixedUpdate() {
        this.EnemyMoviments();
        this.Animations();
    }

    private void OnDrawGizmosSelected()    {        Gizmos.color = Color.red;        Gizmos.DrawWireSphere(this.checkGround.position, this.validRadiusFloor);        Gizmos.DrawWireSphere(this.checkWall.position, this.validRadiusWall);    }

    //----------------------------------------------

    void EnemyMoviments()
    {

        this.isInFloor = Physics2D.OverlapCircle(checkGround.position, validRadiusFloor, solid);
        this.isInWall = Physics2D.OverlapCircle(checkWall.position, validRadiusWall, solid);

        if ((!this.isInFloor || this.isInWall) && this.turnedToRight)
            this.Flip();
       else if ((!this.isInFloor || this.isInWall) && !this.turnedToRight)
            this.Flip();

        if (this.isInFloor)
            this.rb.velocity = new Vector2(this.speed, this.rb.velocity.y);
    }

    void Flip()
    {

        this.turnedToRight = !this.turnedToRight;

        this.tr.localScale = new Vector2(-this.tr.localScale.x, this.tr.localScale.y);

        this.speed *= -1;

    }


    void Animations()
    {

        this.an.SetBool("Walkink", this.isInFloor );
       // this.an.SetBool("Jump", !this.isInFloor);
    }

}
