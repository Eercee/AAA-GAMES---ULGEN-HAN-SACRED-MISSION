using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject humanCharacter; // İnsan karakter
    public GameObject wolfCharacter; // Kurt karakter
    public Camera humanCamera; // İnsan karakter kamerası
    public Camera wolfCamera; // Kurt karakter kamerası

    public VideoPlayer videoPlayer; // Video oynatıcı
    public VideoClip toWolfClip; // İnsan → Kurt dönüşüm videosu
    public VideoClip toHumanClip; // Kurt → İnsan dönüşüm videosu

    private GameObject currentCharacter; // Şu anki aktif karakter

    void Start()
    {
        // Başlangıçta insan karakter aktif
        currentCharacter = humanCharacter;
        ActivateCharacter(humanCharacter, humanCamera);
        DeactivateCharacter(wolfCharacter, wolfCamera);

        // VideoPlayer başlangıçta devre dışı olsun
        videoPlayer.loopPointReached += OnVideoFinished; // Videonun bitiş olayını dinle
        videoPlayer.enabled = false;
    }

    void Update()
    {
        // X tuşuna basılırsa insan → kurt dönüşümü
        if (Input.GetKeyDown(KeyCode.X) && currentCharacter == humanCharacter)
        {
            StartCoroutine(SwitchCharacter(humanCharacter, wolfCharacter, toWolfClip, wolfCamera));
        }

        // C tuşuna basılırsa kurt → insan dönüşümü
        if (Input.GetKeyDown(KeyCode.C) && currentCharacter == wolfCharacter)
        {
            StartCoroutine(SwitchCharacter(wolfCharacter, humanCharacter, toHumanClip, humanCamera));
        }
    }

    // Karakter değişim işlemi
    private System.Collections.IEnumerator SwitchCharacter(GameObject fromCharacter, GameObject toCharacter, VideoClip videoClip, Camera toCamera)
    {
        // Yeni karakterin pozisyonunu eski karaktere eşitle
        toCharacter.transform.position = fromCharacter.transform.position;
        toCharacter.transform.rotation = fromCharacter.transform.rotation;

        // Video oynatıcıyı etkinleştir, ayarla ve oynat
        videoPlayer.enabled = true;
        videoPlayer.clip = videoClip;
        videoPlayer.targetCamera = toCamera;
        videoPlayer.Play();

        // Videonun tamamlanmasını bekle
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        // Aktif karakteri değiştir
        DeactivateCharacter(fromCharacter, fromCharacter.GetComponentInChildren<Camera>());
        ActivateCharacter(toCharacter, toCamera);
        currentCharacter = toCharacter;
    }

    // Bir karakteri aktif hale getir
    private void ActivateCharacter(GameObject character, Camera camera)
    {
        character.SetActive(true);
        camera.gameObject.SetActive(true);
    }

    // Bir karakteri devre dışı bırak
    private void DeactivateCharacter(GameObject character, Camera camera)
    {
        character.SetActive(false);
        camera.gameObject.SetActive(false);
    }

    // Video bittiğinde çağrılır
    private void OnVideoFinished(VideoPlayer vp)
    {
        videoPlayer.targetCamera = null; // Hedef kamerayı temizle
        videoPlayer.enabled = false;    // VideoPlayer'ı devre dışı bırak
    }
}
