using UnityEngine;

public class ThirdPersonCameraWithMouse : MonoBehaviour
{
    public Transform target; // Karakterin Transform'u
    public float distance = 4.0f; // Kameranýn hedefe olan uzaklýðý
    public float height = 2.0f;   // Kameranýn hedefe olan yüksekliði
    public float rotationSpeed = 5.0f; // Fare hareketine duyarlýlýk
    public float minHeight = 1.0f;     // Kameranýn yere yaklaþabileceði minimum yükseklik
    public float maxHeight = 5.0f;     // Kameranýn çýkabileceði maksimum yükseklik

    private float currentYaw = 0.0f;   // Yatay döndürme açýsý
    private float currentPitch = 0.0f; // Dikey döndürme açýsý

    void LateUpdate()
    {
        // Fare hareketine göre açýyý güncelle
        currentYaw += Input.GetAxis("Mouse X") * rotationSpeed;
        currentPitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // Dikey açýyý sýnýrla (yukarý ve aþaðý dönmeyi kontrol eder)
        currentPitch = Mathf.Clamp(currentPitch, -30f, 60f); // Dikey açý sýnýrlarý (örnek deðerler)

        // Kameranýn hedef pozisyonunu hesapla
        Vector3 offset = Quaternion.Euler(currentPitch, currentYaw, 0) * Vector3.back * distance;
        Vector3 desiredPosition = target.position + offset;

        // Kameranýn yüksekliðini sýnýrla
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, target.position.y + minHeight, target.position.y + maxHeight);

        // Kamerayý hedef pozisyona taþý
        transform.position = desiredPosition;

        // Kamerayý hedefe doðru çevir
        transform.LookAt(target);
    }
}
