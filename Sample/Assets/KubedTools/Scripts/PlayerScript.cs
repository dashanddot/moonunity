using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 pushVelocity = Vector3.zero;
    [HideInInspector]
    public Vector3 moveDir;
    protected int JumpDir;

    public bool thirdPerson = false;

    public bool grounded = true;

    public bool controlLeft = false, controlRight = false, controlForward = false, controlBackward = false;
    public bool controlJump = false, controlDown = false;

    public PlayerConfig playerConfig;
    protected float nextJump;
    protected float nextControl;

    public float stamina;
    public float runSpeed = 5f;
    public float sprintSpeed = 10f;

    [System.NonSerialized]
    public float jumpSpeed = 6f;

    [SerializeField]
    protected float _JUMP_STAMINA_COST = 0.1f;

    public float maxStamina = 10;

    float forwardRun = 0;
    float sideRun = 0;
    protected float _staminaTime = 0f;
    protected Vector3 _camPos;
    public float walkSpeed = 2f;
    public Camera cameraComp;
    protected bool _sprint = false;
    protected Vector2 analogMove;

    [System.NonSerialized]
    public float rotationY = 0F, rotationX = 0f;

    [System.NonSerialized]
    public float sensitivityX = 15F;
    [System.NonSerialized]
    public float sensitivityY = 15F;

    public float minimumY = -90F;
    public float maximumY = 90F;

    public float gravity = -9;

    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = controller.isGrounded;

        controlJump = false;
        controlDown = false;

        controlLeft = false;
        controlRight = false;
        controlForward = false;
        controlLeft = false;


        PlayerInput();

        PlayerReact();

    }

    private void FixedUpdate()
    {
        PhyUpdate();
    }

    void PlayerInput()
    {
        rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        if (!thirdPerson)
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        }
        else
            rotationY = 0;

        transform.localEulerAngles = new Vector3(0f, rotationX, 0f);

        if (!thirdPerson)
            cameraComp.transform.localEulerAngles = new Vector3(-rotationY, 0f, 0f);

        analogMove.x = Input.GetAxis("Horizontal");
        if (this.analogMove.x < -0.02f) { controlLeft = true; controlRight = false; }
        else if (this.analogMove.x > 0.02f) { controlLeft = false; controlRight = true; }
        else { controlLeft = false; controlRight = false; }
        analogMove.y = Input.GetAxis("Vertical");
        if (analogMove.y < -0.02f) { controlBackward = true; controlForward = false; }
        else if (analogMove.y > 0.02f) { controlBackward = false; controlForward = true; }
        else { controlBackward = false; controlForward = false; }

        controlJump = Input.GetButton("Jump");
        controlDown = false;
    }

    void PlayerReact()
    {
        float sideRun = 0;
        float forwardRun = 0;

        if (stamina > 0)
        {
            if (controlForward && _sprint)
            {
                stamina -= Time.deltaTime * 2f;
                _staminaTime = Time.time;
            }

        }
        else
        {
            _sprint = false;
        }

        if (Time.time - _staminaTime > 5f)
        {
            stamina += Time.deltaTime * 0.8f;
        }

        {
            if (controlForward)
            {
                if (_sprint)
                    forwardRun = Mathf.Lerp(forwardRun, sprintSpeed, Time.time * 20f);//animation[animRun].speed=1.3f; if(view3face) animation.CrossFade(animRun);
                else
                    forwardRun = Mathf.Lerp(forwardRun, analogMove.y * runSpeed, Time.time * 20f);//animation[animRun].speed=1.3f; if(view3face) animation.CrossFade(animRun);
            }
            else if (controlBackward)
            {
                forwardRun = Mathf.Lerp(forwardRun, (analogMove.y * runSpeed), Time.time * 20f);//animation[animRun].speed=-1.3f; 
                                                                                                //if(view3face) animation.CrossFade(animRun);
            }
            else
            {
                forwardRun = Mathf.Lerp(forwardRun, 0f, Time.time * 20f); //if( animIdle.Length!=0 ) 
                                                                          //if(view3face) animation.CrossFade(animIdle);
            }

            if (controlLeft)
                sideRun = -playerConfig.strafeSpeed;
            else if (controlRight)
                sideRun = playerConfig.strafeSpeed;


            if ((nextJump < Time.time) && controlJump)
            {

                if (grounded && (stamina > 1f))
                {
                    moveDir.y += jumpSpeed;

                    JumpDir = 1;
                    nextJump = Time.time + 1f;

                    //anim.SetInteger("JumpDir", JumpDir);
                    //anim.SetTrigger("IsJump");

                    stamina -= _JUMP_STAMINA_COST;
                    _staminaTime = Time.time;
                }
            }
            else
            {
                if (nextJump < Time.time)
                {
                    JumpDir = 0;


                }

                //anim.SetInteger("JumpDir", JumpDir);
            }

            Vector3 runDir = new Vector3(sideRun, 0, forwardRun);

            float magitudeMax = Mathf.Max(Mathf.Abs(sideRun), Mathf.Abs(forwardRun));

            runDir = Vector3.ClampMagnitude(runDir, magitudeMax);

            if (controlDown)
                runDir = runDir.normalized * walkSpeed;


            moveDir = new Vector3(runDir.x, moveDir.y, runDir.z);


            Vector3 camPos = _camPos;

            

            //cameraComp.transform.parent.localPosition = camPos;
        }
    }

    public void JumperPush(Vector3 dir, float force)
    {
        pushVelocity += dir * force;
        moveDir.y += jumpSpeed;
        nextJump = Time.time + 1f;
    }

    void PhyUpdate()
    {
        float pushspeed = pushVelocity.magnitude;
        
        if (pushspeed > 0)
        {
            float drop;

            if (grounded)
                drop = playerConfig.GROUND_FRICTION * pushspeed * Time.deltaTime;
            else
                drop = playerConfig.AIR_FRICTION * pushspeed * Time.deltaTime;//HACK - gravity isdelta time

            float newspeed = pushspeed - drop;

            if (newspeed < 0)
            {
                newspeed = 0;
            }

            pushVelocity = pushVelocity.normalized * newspeed;

            if (pushVelocity.y < 0)
                pushVelocity.y = 0;

            moveDir += transform.InverseTransformDirection(pushVelocity) * Time.deltaTime;

            if (controlForward && !grounded)
                moveDir += Vector3.forward * playerConfig.airAccelerate * Time.deltaTime;

            Debug.LogFormat("PV: {0}", newspeed);
        }

        var deltaPos = transform.TransformDirection(moveDir) * Time.deltaTime;
        CollisionFlags flags = controller.Move(deltaPos);
        grounded = controller.isGrounded || (flags & CollisionFlags.Below) != 0;

        if (grounded)
            moveDir.y = 0f;

        moveDir.y += gravity * Time.deltaTime;
    }

}
