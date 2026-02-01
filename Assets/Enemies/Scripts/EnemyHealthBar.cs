using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetHealth(float percent)
    {
        Vector3 scale = fillImage.transform.localScale;
        scale.x = percent;
        fillImage.transform.localScale = scale;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }
}
