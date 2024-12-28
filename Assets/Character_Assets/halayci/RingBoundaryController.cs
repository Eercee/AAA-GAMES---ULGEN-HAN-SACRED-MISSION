using UnityEngine;

public class RingBoundaryController : MonoBehaviour
{
    public Vector3 centerPoint = Vector3.zero; // Halkanýn merkezi
    public float innerRadius = 3.0f;          // Ýç sýnýrýn yarýçapý
    public float outerRadius = 5.0f;          // Dýþ sýnýrýn yarýçapý
    public float rotationSpeed = 5.0f;        // Yumuþak dönüþ hýzý
    private Quaternion targetRotation;        // Hedef rotasyon
    private bool shouldRotate = false;        // Dönüþ kontrolü

    void Update()
    {
        // Karakterin merkezden uzaklýðýný hesapla
        Vector3 offset = transform.position - centerPoint;
        float distanceFromCenter = offset.magnitude;

        // Eðer iç sýnýrýn altýna girerse veya dýþ sýnýrýn dýþýna çýkarsa
        if (distanceFromCenter < innerRadius || distanceFromCenter > outerRadius)
        {
            // Y ekseninde 180 derece dönmesi için hedef rotasyonu belirle
            targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
            shouldRotate = true; // Dönüþ sinyalini etkinleþtir

            // Karakteri sýnýrlarýn içinde kalacak þekilde geri çek
            if (distanceFromCenter < innerRadius)
            {
                // Ýç sýnýrda kalacak þekilde ayarla
                transform.position = centerPoint + offset.normalized * innerRadius;
            }
            else if (distanceFromCenter > outerRadius)
            {
                // Dýþ sýnýrda kalacak þekilde ayarla
                transform.position = centerPoint + offset.normalized * outerRadius;
            }
        }

        // Eðer dönüþ yapýlmasý gerekiyorsa
        if (shouldRotate)
        {
            // Karakteri yavaþça hedef rotasyona döndür
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Hedef rotasyona ulaþýldýðýnda dönüþü durdur
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                shouldRotate = false;
            }
        }
    }
}
