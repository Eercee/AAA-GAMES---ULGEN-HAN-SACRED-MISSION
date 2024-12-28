using UnityEngine;

public class ReturnToStartOnBoundaryHit : MonoBehaviour
{
    public Vector3 centerPoint = Vector3.zero; // Halkanýn merkezi
    public float innerRadius = 3.0f;          // Ýç sýnýrýn yarýçapý
    public float outerRadius = 5.0f;          // Dýþ sýnýrýn yarýçapý
    private Vector3 startPosition;            // Baþlangýç pozisyonu
    private Quaternion startRotation;         // Baþlangýç rotasyonu

    void Start()
    {
        // Baþlangýç pozisyonu ve rotasyonu kaydet
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Karakterin merkezden uzaklýðýný hesapla
        Vector3 offset = transform.position - centerPoint;
        float distanceFromCenter = offset.magnitude;

        // Eðer iç sýnýrýn altýna girerse veya dýþ sýnýrýn dýþýna çýkarsa
        if (distanceFromCenter < innerRadius || distanceFromCenter > outerRadius)
        {
            // Karakteri baþlangýç pozisyonuna ve rotasyonuna döndür
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
    }
}
