using UnityEngine;
using UnityEngine.UI;

// unity returns deltaTime as Max(targetFrameRate,(calcTime + waitTime))
// dont be suprised to see 59 at 60FPS target
public class FPSCounter : MonoBehaviour
{
    public static bool isShown = false;
    public Image background;
    public Text text;

    private static string prefabName = "FPSCounter";
    private static FPSCounter instance;
    private static float worst = 0;
    private static float timer;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (instance != null) return;
        var prefab = Resources.Load<GameObject>(prefabName);
        var obj = Instantiate(prefab);
        obj.name = prefabName;
        instance = obj.GetComponent<FPSCounter>();
        DontDestroyOnLoad(obj);
    }
    private static void Show()
    {
        if (instance == null) return;
        instance.background.gameObject.SetActive(true);
        isShown = true;
    }
    private static void Hide()
    {
        if (instance == null) return;
        instance.background.gameObject.SetActive(false);
        isShown = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (FPSCounter.isShown)
                FPSCounter.Hide();
            else
                FPSCounter.Show();
        }

        var delta = Time.unscaledDeltaTime;
        worst = Mathf.Max(worst,delta);
        timer += delta;
        if (timer <= 1f) return;

        var worstFrames = (int)(1f/worst);

        text.color = worstFrames <= 50 ? Color.red : Color.green;
        text.text = worstFrames.ToString();

        timer = 0;
        worst = 0;
    }
    // private void OnGUI()
    // {
    //     if (timer <= 0) return;
    //     timer -= Time.deltaTime;
    //     GUI.Label(new Rect(10, 10, 300, 300), msg);
    // }
}
