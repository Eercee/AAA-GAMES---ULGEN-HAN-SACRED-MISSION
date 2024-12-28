using UnityEngine;

public class MoveBackwardOnAnimation : MonoBehaviour
{
    public Animator animator; // Karakterin Animator bileþeni
    public float moveSpeed = 2.0f; // Geri hareket hýzý

    private void Update()
    {
        // Animator'ýn þu anki animasyon durumunu kontrol ediyoruz
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Eðer "BackLeft_run" animasyonu oynatýlýyorsa
        if (stateInfo.IsName("Armature|Armature|BackLeft_run|baselayer"))
        {
            // Karakteri geri hareket ettir
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }
}
