using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshProUGUI))]
public class CurveTMP : MonoBehaviour
{
    public float radius = 200f;  // Radius of the circle
    private TextMeshProUGUI tmp;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.ForceMeshUpdate();
    }

    void OnValidate()
{
    if (tmp == null)
        tmp = GetComponent<TextMeshProUGUI>();

    tmp.ForceMeshUpdate();
    WarpTextToCircle();
}


    void WarpTextToCircle()
    {
        tmp.ForceMeshUpdate();
        TMP_TextInfo textInfo = tmp.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            // Get the center point of the character (average of top-left and bottom-right vertices)
            Vector3 charMidBaseline = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2;

            // Calculate the angle for this character around the circle (based on x position)
            float normalizedX = charMidBaseline.x / tmp.rectTransform.rect.width;
            float angle = normalizedX * 360f * Mathf.Deg2Rad;  // Convert to radians

            // Calculate position on the circle's circumference
            Vector3 offset = new Vector3(
                Mathf.Sin(angle) * radius,
                Mathf.Cos(angle) * radius,
                0);

            // Calculate rotation so letter faces outward
            Quaternion rotation = Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg + 90f);

            // Offset each vertex and rotate around circle center
            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = vertices[vertexIndex + j];
                Vector3 charRelative = orig - charMidBaseline;      // Position relative to char center
                vertices[vertexIndex + j] = offset + rotation * charRelative;
            }
        }

        // Push the updated vertex data to the mesh
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            tmp.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
