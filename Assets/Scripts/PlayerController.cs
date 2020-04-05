using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    public float MoveSpeed;
    public float RotationSpeed;
    CharacterController cc;
    private Animator anim;
    protected Vector3 gravidade = Vector3.zero;
    protected Vector3 move = Vector3.zero;
    private bool jump = false;

    [HideInInspector]
    public bool isDead = false;

    private GameManager gm;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        anim.SetTrigger("Parado");
        cc.enableOverlapRecovery = false;
        gm = GameManager.GetInstance();
        StartPlayer();
    }

    void Update()
    {
        if (!isDead)
        {
            Vector3 move = Input.GetAxis("Vertical") * transform.TransformDirection(Vector3.forward) * MoveSpeed;
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime, 0));

            if (!cc.isGrounded)
            {
                gravidade += Physics.gravity * Time.deltaTime;
            }
            else
            {
                gravidade = Vector3.zero;
                if (jump)
                {
                    gravidade.y = 6f;
                    jump = false;
                }
            }
            move += gravidade;
            cc.Move(move * Time.deltaTime);
            Anima();
        }
    }

    void StartPlayer()
    {
        gm.player = gameObject;
        gm.playerController = this;
        gm.playerPos = transform;
    }

    void Anima()
    {
        if (!Input.anyKey)
        {
            anim.SetTrigger("Parado");
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetTrigger("Pula");
                jump = true;
            }
            else if (IsPlayerWalking())
            {
                anim.SetTrigger("Corre");
            }
        }
    }

    bool IsPlayerWalking()
    {
        return Input.GetAxis("Vertical") != 0;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("MortalDanger"))
        {
            gm.DealDamage(100);
        }
    }

}
