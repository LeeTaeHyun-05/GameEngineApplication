using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpPower = 5f;
    public float gravity = -9.81f;
    public CinemachineVirtualCamera virtualCam;
    public float rotationSpeed = 5f;
    private CinemachinePOV pov;
    private CharacterController controller;
    private Vector3 velocity;
    public bool isGrounded;
     CinemachineSwitcher cS;
    public int maxHP = 100;
    private int currentHP;
    public Slider hpSlider;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();
        cS = FindObjectOfType<CinemachineSwitcher>();

        currentHP = maxHP;
        hpSlider.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            pov.m_HorizontalAxis.Value = transform.eulerAngles.y;
            pov.m_VerticalAxis.Value = 0f;
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 camForward = virtualCam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = virtualCam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = (camForward * z + camRight * x).normalized;
        if(!cS.usingFreeLook)
             controller.Move(move * speed * Time.deltaTime);

        float cameraYaw = pov.m_HorizontalAxis.Value;
        Quaternion targetRot = Quaternion.Euler(0f, cameraYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpPower;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10f;
            virtualCam.m_Lens.FieldOfView = 80f;
        }
        else
        {
            speed = 5f;
            virtualCam.m_Lens.FieldOfView = 40f;
        }

    }

    public void TakeDamage (int damage)
    {
        currentHP -= damage;
        hpSlider.value = (float)currentHP / maxHP;
        
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    

    
}
