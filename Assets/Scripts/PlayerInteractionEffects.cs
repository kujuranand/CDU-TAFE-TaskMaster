using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionEffects : MonoBehaviour
{
    [Header("Highlight Settings")]
    public Color highlightColor = Color.yellow;
    public float highlightMaxIntensity = 2.0f;
    public float highlightMinIntensity = 0.5f;
    public float pulseSpeed = 1.0f;

    [Header("Tick and Cross Mark Settings")]
    public GameObject tickCrossCanvasPrefab;
    public Transform markSpawnPoint;  // Use this for tick/cross mark spawn
    public Vector3 markSize = new Vector3(1f, 1f, 1f);
    public Camera mainCamera;

    private Renderer[] childRenderers;
    private Material[] originalMaterials;
    private Color[] originalEmissionColors;
    private Coroutine highlightCoroutine;
    private GameObject tickCrossCanvasInstance;
    private Image tickImage;
    private Image crossImage;

    private void Start()
    {
        // Find all child GameObjects with Renderer components
        childRenderers = GetComponentsInChildren<Renderer>();

        if (childRenderers.Length == 0)
        {
            Debug.LogError("No Renderer components found on this GameObject or its children!", gameObject);
        }
        else
        {
            // Store original materials and emission colors for all children
            originalMaterials = new Material[childRenderers.Length];
            originalEmissionColors = new Color[childRenderers.Length];

            for (int i = 0; i < childRenderers.Length; i++)
            {
                originalMaterials[i] = childRenderers[i].material;
                originalEmissionColors[i] = originalMaterials[i].GetColor("_EmissionColor");
            }
        }
    }

    private void Update()
    {
        if (tickCrossCanvasInstance != null && mainCamera != null)
        {
            tickCrossCanvasInstance.transform.LookAt(mainCamera.transform);
        }
    }

    public void StartHighlight()
    {
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
        }

        highlightCoroutine = StartCoroutine(PulseHighlight());
    }

    public void StopHighlight()
    {
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
            highlightCoroutine = null;
        }

        // Restore the original emission color for all child GameObjects
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].material.SetColor("_EmissionColor", originalEmissionColors[i]);
            childRenderers[i].material.DisableKeyword("_EMISSION");
        }
    }

    public void ShowTickMark()
    {
        if (tickCrossCanvasPrefab != null && markSpawnPoint != null)
        {
            SetupTickCrossCanvas();
            tickImage.enabled = true;
            crossImage.enabled = false;
        }
        else
        {
            Debug.LogError("TickCrossCanvas prefab or mark spawn point is not assigned!", gameObject);
        }
    }

    public void ShowCrossMark()
    {
        if (tickCrossCanvasPrefab != null && markSpawnPoint != null)
        {
            SetupTickCrossCanvas();
            tickImage.enabled = false;
            crossImage.enabled = true;
        }
        else
        {
            Debug.LogError("TickCrossCanvas prefab or mark spawn point is not assigned!", gameObject);
        }
    }

    private void SetupTickCrossCanvas()
    {
        if (tickCrossCanvasInstance == null)
        {
            tickCrossCanvasInstance = Instantiate(tickCrossCanvasPrefab, markSpawnPoint.position, Quaternion.identity);
            tickCrossCanvasInstance.transform.localScale = markSize;

            tickImage = tickCrossCanvasInstance.transform.Find("TickImage").GetComponent<Image>();
            crossImage = tickCrossCanvasInstance.transform.Find("CrossImage").GetComponent<Image>();
        }
    }

    private IEnumerator PulseHighlight()
    {
        bool increasing = true;
        float currentIntensity = highlightMinIntensity;

        while (true)
        {
            float elapsedTime = 0f;

            while (elapsedTime < 1.0f / pulseSpeed)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / (1.0f / pulseSpeed);

                currentIntensity = increasing ? Mathf.Lerp(highlightMinIntensity, highlightMaxIntensity, t) :
                                                Mathf.Lerp(highlightMaxIntensity, highlightMinIntensity, t);

                // Apply emission color to all child renderers
                foreach (Renderer childRenderer in childRenderers)
                {
                    childRenderer.material.SetColor("_EmissionColor", highlightColor * currentIntensity);
                    childRenderer.material.EnableKeyword("_EMISSION");
                }

                yield return null;
            }

            increasing = !increasing;
        }
    }
}
