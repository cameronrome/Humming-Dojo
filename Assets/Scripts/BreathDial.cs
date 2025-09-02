using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreathDial : MonoBehaviour
{
    [SerializeField] private Image timer;
    [SerializeField] private Image wave;
    [SerializeField] private Image icon;
    [SerializeField] private Image marker;
    [SerializeField] private Transform markerAnchor;
    [SerializeField] private Sprite inhaleSprite;
    [SerializeField] private Sprite exhaleSprite;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float inhaleDur = 5f;
    [SerializeField] private float exhaleDur = 5f;
    [SerializeField] private int numDots = 72;

    private List<GameObject> dotTrail;
    private bool inhaling = false;

    private void Start()
    {
        timer.fillAmount = 0;
        icon.sprite = inhaleSprite;
        inhaling = true;
        dotTrail = new List<GameObject>();
        wave.transform.localPosition = new Vector3(0, -150, 0);

        for (int i = 0; i < numDots; i++)
        {
            dotTrail.Add(null);
        }
    }

    private void Update()
    {
        if (timer.fillAmount >= 1f)
        {
            if (inhaling)
            {
                inhaling = false;
                timer.fillAmount = 0;
                icon.sprite = exhaleSprite;
                wave.transform.localPosition = new Vector3(0, -150, 0);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (inhaling)
        {
            timer.fillAmount += Time.deltaTime / inhaleDur;
            marker.transform.RotateAround(markerAnchor.position, -Vector3.forward, Time.deltaTime * 360f / inhaleDur);
            wave.transform.localPosition = new Vector3(0, wave.transform.localPosition.y + (150 / inhaleDur) * Time.deltaTime, 0);
        }
        else
        {
            timer.fillAmount += Time.deltaTime / exhaleDur;
            marker.transform.RotateAround(markerAnchor.position, -Vector3.forward, Time.deltaTime * 360f / exhaleDur);
            wave.transform.localPosition = new Vector3(0, wave.transform.localPosition.y + (150 / exhaleDur) * Time.deltaTime, 0);
        }

        for (int i = 0; i < numDots; i++)
        {
            float fillIdx = (float)i / (float)numDots;
            float radius = 58f;
            float angleDeg = i * (360 / numDots);
            float angleOffset = -90f;
            float angleRad = Mathf.Deg2Rad * (angleDeg + angleOffset);
            float offset = 14f;

            if (timer.fillAmount > fillIdx && dotTrail[i] == null)
            {
                dotTrail[i] = Instantiate(dotPrefab, transform);
                dotTrail[i].transform.localPosition = new Vector3(-radius * Mathf.Cos(angleRad), radius * Mathf.Sin(angleRad) - offset);
            }

            if (timer.fillAmount < fillIdx && dotTrail[i] != null)
            {
                Destroy(dotTrail[i]);
                dotTrail[i] = null;
            }

            if (dotTrail[i] != null)
            {
                Color dotColor = Color.white;
                dotColor.a = (float)i / (float)numDots;
                dotTrail[i].GetComponent<Image>().color = dotColor;
            }
        }
        
    }
}
