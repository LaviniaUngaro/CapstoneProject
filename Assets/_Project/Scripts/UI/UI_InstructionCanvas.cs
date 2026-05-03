using System.Collections;
using UnityEngine;

public class UI_InstructionCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = 1 - (elapsed / _fadeDuration);
            yield return null;
        }

        _canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
}
