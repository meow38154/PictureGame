using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;

public static class Utility
{
    private static Camera _cam;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        _cam = Camera.main;
    }

    public static Vector2 GetMouseWorldPosition()
    {
        if (_cam == null) _cam = Camera.main;

        if (_cam == null || Mouse.current == null) return Vector2.zero;

        Vector2 mousePos = GetMouseScreenPosition();

        float distanceFromCamera = -_cam.transform.position.z;

        Vector3 worldPos = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distanceFromCamera));

        return worldPos;
    }

    public static Vector2 GetMouseScreenPosition()
    {
        return Mouse.current == null ? Vector2.zero : Mouse.current.position.ReadValue();
    }

    public static Task<Texture2D> GetCurrentSceneScreenAsync(Rect rect)
    {
        var tcs = new TaskCompletionSource<Texture2D>();

        GameObject runnerObj = new GameObject("[ScreenCaptureRunner]");
        var runner = runnerObj.AddComponent<DummyMonoBehaviour>();

        runner.StartCoroutine(CaptureCurrentScreenRoutine(rect, texture =>
        {
            tcs.TrySetResult(texture);
        }, runnerObj));

        return tcs.Task;
    }

    private static IEnumerator CaptureCurrentScreenRoutine(Rect rect, System.Action<Texture2D> onComplete, GameObject runnerObj)
    {
        yield return new WaitForEndOfFrame();

        int screenW = Screen.width;
        int screenH = Screen.height;

        int startX = Mathf.Clamp(Mathf.FloorToInt(rect.x), 0, screenW);
        int startY = Mathf.Clamp(Mathf.FloorToInt(rect.y), 0, screenH);
        int width = Mathf.Clamp(Mathf.FloorToInt(rect.width), 0, screenW - startX);
        int height = Mathf.Clamp(Mathf.FloorToInt(rect.height), 0, screenH - startY);

        Texture2D texture = null;

        if (width > 0 && height > 0)
        {
            texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
            texture.Apply();
        }

        onComplete?.Invoke(texture);

        Object.Destroy(runnerObj);
    }
    
    public static Task<Texture2D> GetSceneFirstScreenAsync(string sceneName, Rect rect)
    {
        var tcs = new TaskCompletionSource<Texture2D>();

        GameObject runnerObj = new GameObject("[SceneCaptureRunner]");
        Object.DontDestroyOnLoad(runnerObj);
        var runner = runnerObj.AddComponent<DummyMonoBehaviour>();

        runner.StartCoroutine(CaptureSceneRoutine(sceneName, rect, texture =>
        {
            tcs.TrySetResult(texture);
        }, runnerObj));

        return tcs.Task;
    }

    private static IEnumerator CaptureSceneRoutine(string sceneName, Rect rect, System.Action<Texture2D> onComplete, GameObject runnerObj)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return loadOp;

        Scene targetScene = SceneManager.GetSceneByName(sceneName);
        Camera targetCamera = null;
        
        if (targetScene.IsValid())
        {
            GameObject[] rootObjects = targetScene.GetRootGameObjects();
            foreach (var obj in rootObjects)
            {
                Camera[] cameras = obj.GetComponentsInChildren<Camera>(true);
                foreach (var cam in cameras)
                {
                    if (cam != null && cam.CompareTag("MainCamera"))
                    {
                        targetCamera = cam; break;
                    }
                }

                if (targetCamera != null) break;
            }
        }

        Texture2D texture = null;

        if (targetCamera != null)
        {
            int screenW = Screen.width;
            int screenH = Screen.height;
            RenderTexture rt = new RenderTexture(screenW, screenH, 24);

            bool prevCameraEnabled = targetCamera.enabled;
            RenderTexture prevTargetTexture = targetCamera.targetTexture;
            AudioListener listener = targetCamera.GetComponent<AudioListener>();
            bool prevListenerEnabled = false;

            if (listener != null)
            {
                prevListenerEnabled = listener.enabled;
                listener.enabled = false;
            }

            targetCamera.enabled = false;
            targetCamera.targetTexture = rt;
            targetCamera.Render();

            RenderTexture.active = rt;

            int startX = Mathf.Clamp(Mathf.FloorToInt(rect.x), 0, screenW);
            int startY = Mathf.Clamp(Mathf.FloorToInt(rect.y), 0, screenH);
            int width = Mathf.Clamp(Mathf.FloorToInt(rect.width), 0, screenW - startX);
            int height = Mathf.Clamp(Mathf.FloorToInt(rect.height), 0, screenH - startY);

            if (width > 0 && height > 0)
            {
                texture = new Texture2D(width, height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
                texture.Apply();
            }

            targetCamera.targetTexture = prevTargetTexture;
            targetCamera.enabled = prevCameraEnabled;

            if (listener != null)
            {
                listener.enabled = prevListenerEnabled;
            }

            RenderTexture.active = null;
            Object.Destroy(rt);
        }

        if (targetScene.IsValid())
        {
            yield return SceneManager.UnloadSceneAsync(targetScene);
        }

        onComplete?.Invoke(texture);
        Object.Destroy(runnerObj);
    }
    
    private class DummyMonoBehaviour : MonoBehaviour { }
}