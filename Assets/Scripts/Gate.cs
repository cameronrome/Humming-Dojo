using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private HumDial humDial;
    [SerializeField] private List<Material> chakraMaterials = new();
    [SerializeField] private List<Renderer> chakras = new();
    [SerializeField] private List<int> keys = new();
    [SerializeField] private float fadeDur = 2f;

    private List<Color> chakraBaseColors = new();
    private List<Color> chakraEmissionColors = new();
    private Material material;
    private Color baseColor;
    private Color emissionColor;

    public void OnTriggerEnter(Collider collider)
    {
        if (!collider.GetComponent<Player>()) return;

        humDial.SetKeys(keys);
        humDial.Open();
        humDial.OnHumPass += Open;
    }

    public void OnTriggerExit(Collider collider)
    {
        humDial.Close();
    }

    public void Open()
    {
        StartCoroutine(FadeOut());
    }

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        baseColor = material.GetColor("_BaseColor");
        emissionColor = material.GetColor("_EmissionColor");

        for (int i = 0; i < chakras.Count; i++)
        {
            if (i >= 0 || i < keys.Count)
            {
                chakras[i].material = chakraMaterials[keys[i]];
                chakraBaseColors.Add(chakras[i].material.GetColor("_BaseColor"));
                chakraEmissionColors.Add(chakras[i].material.GetColor("_EmissionColor"));
            }
        }
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDur)
        {
            timer += Time.deltaTime;

            float progress = timer / fadeDur;
            float alpha = Mathf.Lerp(1f, 0f, progress);

            Color newBaseColor = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            Color newEmissionColor = emissionColor * alpha;

            material.SetColor("_BaseColor", newBaseColor);
            material.SetColor("_EmissionColor", newEmissionColor);

            for (int i = 0; i < chakras.Count; i++)
            {
                Color newChakraBaseColor = new Color(chakraBaseColors[i].r, chakraBaseColors[i].g, chakraBaseColors[i].b, alpha);
                Color newChakraEmissionColor = chakraEmissionColors[i] * alpha;

                chakras[i].material.SetColor("_BaseColor", newChakraBaseColor);
                chakras[i].material.SetColor("_EmissionColor", newChakraEmissionColor);
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
