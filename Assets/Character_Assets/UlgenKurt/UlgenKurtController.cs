using UnityEngine;

public class UlgenKurtController : MonoBehaviour
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
        // Eðer saldýrý yapýlýyorsa diðer iþlemleri engelle
        if (isAttacking)
            return;

        float currentSpeed = moveSpeed; // Varsayýlan olarak yürüme hýzý

        // Sol fare týklandýðýnda saldýrý animasyonunu baþlat
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            animator.Play("Armature|Armature|Shouting_Angrily|baselayer");
            Invoke("EndAttack", 2f); // Saldýrý animasyonu süresine göre bir gecikme ayarla (örneðin 1.5 saniye)
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
                animator.Play("Armature|Armature|Run_02|baselayer");
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
            animator.Play("Armature|Armature|Alert|baselayer");
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    // Saldýrý bittiðinde çaðrýlýr
    void EndAttack()
    {
        isAttacking = false;
    }
}
