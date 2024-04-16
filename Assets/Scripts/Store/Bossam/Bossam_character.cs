using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Bossam_character : MonoBehaviour
{
    [SerializeField] private Sprite open_img, close_img;
    [SerializeField] private Image image, toungue;
    [SerializeField] public Transform center;
    [SerializeField] private AudioSource aahSfx;

    public bool ready;

    [Button]
    public void OpenMouth()
    {
        ready = true;
        image.sprite = open_img;
        toungue.gameObject.SetActive(true);
        aahSfx.Play();
    }

    [Button]
    public void CloseMouth()
    {
        ready = false;
        image.sprite = close_img;
        toungue.gameObject.SetActive(false);
        aahSfx.Stop();
    }
}