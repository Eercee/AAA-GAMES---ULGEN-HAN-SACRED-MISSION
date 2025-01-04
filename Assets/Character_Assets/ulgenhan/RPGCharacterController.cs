using UnityEngine;

public class RPGCharacterController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Yürüme hýzý
    public float runSpeed = 10.0f; // Koþma hýzý
    public float rotationSpeed = 10.0f; // Karakterin dönüþ hýzý
    public Transform cameraTransform; // Kameranýn Transform'u
    private Animator animator;
    private Rigidbody rb;

    private bool isAttacking = false; // Saldýrý durumunu takip eder

    void Start()
    {
        // Animator ve Rigidbody bileþenlerini al
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Fare imlecini gizle ve kilitle
        Cursor.lockState = CursorLockMode.Locked; // Ýmleci ekranýn ortasýna kilitle
        Cursor.visible = false; // Ýmleci görünmez yap
    }

    void Update()
    {
        // Eðer saldýrý yapýlýyorsa veya UI açýk durumdaysa diðer iþlemleri engelle
        if (isAttacking || IsDialogueActive())
            return;

        float currentSpeed = moveSpeed; // Varsayýlan olarak yürüme hýzý

        // Sol fare týklandýðýnda saldýrý animasyonunu baþlat
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            animator.Play("Armature|Armature|Left_Slash|baselayer");
            Invoke("EndAttack", 1.5f); // Saldýrý animasyonu süresine göre bir gecikme ayarla
            return;
        }

        // W tuþuna basýlýysa
        if (Input.GetKey(KeyCode.W))
        {
            // Eðer Shift tuþuna da basýlýysa koþma moduna geç
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;

                // Koþma animasyonunu baþlat
                animator.Play("Armature|Armature|Run_03|baselayer");
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
            else
            {
                // Yürüme animasyonunu baþlat
                animator.Play("Armature|Armature|walking_man|baselayer");
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }

            // Kameranýn yönüne göre hareket
            Vector3 forward = cameraTransform.forward;
            forward.y = 0; // Y eksenini sýfýrla (düz zemin hareketi)
            forward.Normalize(); // Yönü normalize et

            // Karakterin yönünü kameranýn yönüne doðru döndür
            Quaternion targetRotation = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Karakteri ileri doðru hareket ettir
            Vector3 movement = forward * currentSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
        else
        {
            // Boþta kalma animasyonuna geç
            animator.Play("Armature|Armature|Combat_Stance|baselayer");
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    // Saldýrý bittiðinde çaðrýlýr
    void EndAttack()
    {
        isAttacking = false;
    }

    // UI Paneli açýk mý kontrol eder
    bool IsDialogueActive()
    {
        GameObject dialoguePanel = GameObject.Find("Panel"); // Panel adýný kontrol edin
        return dialoguePanel != null && dialoguePanel.activeSelf;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shahmeran"))
        {
            // Animasyonu idle'a geçir
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.Play("Armature|Armature|Combat_Stance|baselayer"); // Idle animasyonu
        }
    }

}
