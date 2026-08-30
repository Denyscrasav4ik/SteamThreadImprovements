using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SteamThreadImprovements.CustomComponents;

internal sealed class DeafBaldicator : MonoBehaviour
{
    internal static DeafBaldicator? Instance { get; private set; }

    private RectTransform rect = null!;
    private Image[] images = null!;
    private Coroutine? animation;

    private static readonly Vector2 Start = new(0, -96);
    private const float Speed = 6f, Delay = 2f, ShakeDuration = 0.75f;
    private const float ShakeAmount = 15f;

    private void Awake()
    {
        Instance = this;
        rect = (RectTransform)transform;
        images = GetComponentsInChildren<Image>(true);
        SetSprite();
        rect.anchoredPosition = Start;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void SetSprite()
    {
        if (ImprovementPlugin.deafBaldicator is not { } sprite) return;
        foreach (var image in images) image.sprite = sprite;
    }

    internal static void Create(HudManager hud)
    {
        if (Instance != null) return;

        var clone = Object.Instantiate(
            hud.transform.Find("Baldi").gameObject,
            hud.transform,
            true);

        clone.name = "Deaf Baldi";
        if (clone.TryGetComponent(out Animator animator))
            animator.enabled = false;

        clone.AddComponent<DeafBaldicator>();
    }

    internal void Activate()
    {
        gameObject.SetActive(true);
        if (animation != null) StopCoroutine(animation);
        animation = StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        SetSprite();
        yield return MoveUpWithShake();
        yield return new WaitForSeconds(Delay);
        yield return Move(Vector2.zero, Start);

        gameObject.SetActive(false);
        animation = null;
    }

    private IEnumerator Move(Vector2 from, Vector2 to)
    {
        for (float t = 0; t < 1; t += Speed * Time.deltaTime)
        {
            rect.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }

        rect.anchoredPosition = to;
    }

    private IEnumerator MoveUpWithShake()
    {
        float elapsed = 0f;
        float moveDuration = 1f / Speed;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            Vector2 position = Vector2.Lerp(Start, Vector2.zero, t);

            if (elapsed < ShakeDuration)
                position.x += Random.Range(-ShakeAmount, ShakeAmount);

            rect.anchoredPosition = position;

            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = Vector2.zero;
    }

}
