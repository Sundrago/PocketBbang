using UnityEngine;
using UnityEngine.Serialization;

public class DdukHit : MonoBehaviour
{
    [SerializeField] private DdukMinigameManager ddukMinigameManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "dduck")
            ddukMinigameManager.GotDduk(collision.gameObject);
        else if (collision.gameObject.tag == "spider") ddukMinigameManager.LostDduck(collision.gameObject);
    }
}