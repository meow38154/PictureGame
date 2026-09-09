
using Agents.Players;
using DevLib.ModuleSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PhotoSystem.Capture
{
    public class CaptureController : ModuleOwner
    {
        [SerializeField] private PlayerInputSo playerInputSo;
        [SerializeField] private SpriteRenderer playerSpriteRenderer;
        
        private ICaptureSelection _selection;
        private IScreenCapture _screenCapture;

        protected override void Awake()
        {
            base.Awake();

            _selection = GetModule<ICaptureSelection>();
            _screenCapture = GetModule<IScreenCapture>();

            playerInputSo.OnMouseLeftButtonPressed += HandleCaptureStart;
            playerInputSo.OnMouseLeftButtonReleased += HandleCaptureEnd;
        }

        private void Update()
        {
            if (!_selection.IsSelecting) return;

            _selection.UpdateSelection(Mouse.current.position.ReadValue());
        }

        private void HandleCaptureStart()
        {
            _selection.BeginSelection(Mouse.current.position.ReadValue());
        }

        private void HandleCaptureEnd()
        {
            Rect screenRect = GetEndScreenRect();

            StartCoroutine(_screenCapture.Capture(screenRect, HandleCaptured));
        }

        private void HandleCaptured(Texture2D texture)
        {
            Rect worldRect = ScreenToWorldRect(GetEndScreenRect());
            
            float pixelsPerUnit = texture.width / worldRect.width;

            Sprite sprite = Sprite.Create(
                texture, new Rect(0, 0, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f), pixelsPerUnit);
            
            playerSpriteRenderer.sprite = sprite;
        }
        
        private Rect ScreenToWorldRect(Rect screenRect)
        {
            Camera cam = Camera.main;

            float distance = -cam.transform.position.z;

            Vector3 min = cam.ScreenToWorldPoint(new Vector3(screenRect.xMin, screenRect.yMin, distance));

            Vector3 max = cam.ScreenToWorldPoint(new Vector3(screenRect.xMax, screenRect.yMax, distance));

            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private Rect GetEndScreenRect()
        {
            return _selection.EndSelection(Mouse.current.position.ReadValue());
        }

        private void OnDestroy()
        {
            playerInputSo.OnMouseLeftButtonPressed -= HandleCaptureStart;
            playerInputSo.OnMouseLeftButtonReleased -= HandleCaptureEnd;
        }
    }
}