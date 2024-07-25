using ArenaHeroes.Common;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaWidget : MonoBehaviour
{
    [Inject]
    private readonly EventDispatcher _eventDispatcher;
    [Inject]
    private readonly UiSafeAreaController _uiSafeAreaController;

    private RectTransform _rect;

    private void Awake()
    {
        _rect = (RectTransform) transform;
    }

    private void OnEnable()
    {
        _eventDispatcher.Subscribe<OnSafeAreaChanged>(OnSafeAreaChanged);
        OnSafeAreaChanged();
    }

    private void OnDisable()
    {
        _eventDispatcher.Unsubscribe<OnSafeAreaChanged>(OnSafeAreaChanged);
    }

    private void OnSafeAreaChanged()
    {
        var safeArea = _uiSafeAreaController.GetSafeArea();

        _rect.anchorMin = Vector2.zero;
        _rect.anchorMax = Vector2.zero;
        _rect.pivot = Vector2.zero;

        _rect.anchoredPosition = safeArea.min;
        _rect.sizeDelta = safeArea.size;;
    }
