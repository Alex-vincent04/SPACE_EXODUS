using UnityEngine;
using System.Collections.Generic;

public class SegmentedHealthBar : MonoBehaviour
{
    public Transform healthBarContainer; // Parent object containing all health segments
    public PlayerHealth playerHealth;    // Reference to PlayerHealth script
    private List<GameObject> healthSegments = new List<GameObject>();

    //public Transform shieldBarContainer;  // Parent object containing all shield segments
    //private List<GameObject> shieldSegments = new List<GameObject>();

    //private int lastDisabledSegmentIndex = -1; // Keeps track of the last disabled segment

    void Start()
    {
        // Populate the list with all child objects inside the container
        foreach (Transform child in healthBarContainer)
        {
            healthSegments.Add(child.gameObject);
        }
    }

    void Update()
    {
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        // Calculate how many segments should be active
        int activeSegments = Mathf.CeilToInt(playerHealth.currentHealth / 3f);

        // Loop through all segments
        for (int i = 0; i < healthSegments.Count; i++)
        {
            if (i < activeSegments)
                healthSegments[i].SetActive(true);
            else
                healthSegments[i].SetActive(false);
        }
    }
}
