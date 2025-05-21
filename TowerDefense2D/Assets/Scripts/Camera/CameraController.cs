using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        Camera.main.orthographic = true;

        Map map = FindObjectOfType<Map>();

        if (map != null)
        {
            // Simpelweg center op 0,0 als de map daar rondom geplaatst is
            Camera.main.transform.position = new Vector3(0f, 0f, -10f);

            // Pas dit eventueel aan afhankelijk van de verhouding van je scherm
            Camera.main.orthographicSize = map.Height / 2f + 1f;
        }
        else
        {
            Debug.LogError("Geen Map gevonden in de scene!");
        }
    }
}
