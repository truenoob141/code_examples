using ArenaHeroes.GUI;
using UnityEngine;
using Zenject;

public class UiSafeAreaController : IInitializable, ITickable
{
    [Inject]
    private readonly EventDispatcher _eventDispatcher;
    [Inject]
    private readonly GuiController _guiController;

    private Vector2 _lastCanvasScale;
    private Vector2 _lastSafeZonePosition;
    private Vector2 _lastSafeZoneSize;

    private Rect _safeArea;
    private RectTransform _root;

    public void Initialize()
    {
        _root = _guiController.GetRoot();
    }

    public void Tick()
    {
        Vector2 scale = _root.localScale;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (_lastCanvasScale.x == scale.x
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            && _lastCanvasScale.y == scale.y &&
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            _lastSafeZonePosition == Screen.safeArea.position &&
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            _lastSafeZoneSize == Screen.safeArea.size)
            return;

        var cam = _guiController.RenderCamera;
        if (cam != null)
        {
            var canvasSize = _root.rect.size;
            var screenSize = new Vector2(Screen.width, Screen.height);
            _safeArea = new Rect(_safeArea)
            {
                min = canvasSize * Screen.safeArea.min / screenSize,
                max = canvasSize * Screen.safeArea.max / screenSize,
            };
        }
        else
        {
            _safeArea = new Rect(_safeArea)
            {
                min = Screen.safeArea.min / scale,
                size = Screen.safeArea.size / scale
            };
        }

        _lastCanvasScale = scale;
        _lastSafeZonePosition = Screen.safeArea.position;
        _lastSafeZoneSize = Screen.safeArea.size;

        Debug.Log(
            $"[SAFE_AREA] Safe area = {_safeArea}\n" +
            $"Screen.height {Screen.height}; Screen.width {Screen.width}\n" +
            $"Screen.safe {Screen.safeArea}\n" +
            $"Cutouts {Screen.cutouts.Length}");

        _eventDispatcher.Trigger<OnSafeAreaChanged>();
    }

    public Rect GetSafeArea()
    {
        return _safeArea;
    }
}