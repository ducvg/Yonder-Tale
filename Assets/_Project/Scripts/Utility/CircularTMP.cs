using UnityEngine;
using TMPro;

/// <summary>
/// An extension of TextMeshPro that causes the text to be displayed in a
/// circular arc.
///
/// Adapted from https://github.com/TonyViT/CurvedTextMeshPro and improved.
/// TonyViT's version has some unnecessary properties and doesn't use the
/// OnPreRenderText event, which allows for fewer mesh updates.
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshProUGUI))]
public class CircularTMP : MonoBehaviour
{
    [SerializeField] private AnimationCurve growthCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);  //Controls how the "pulsing" animation behaves over time
    [SerializeField] private float animationDuration = 1f; //Time it takes for a single pulse.
    [SerializeField] private float characterDelay = 0.05f;  //Delay before each character starts pulsing
    private float elapsedTime;

    private TextMeshProUGUI m_TextComponent;

    [SerializeField] private float m_radius = 10.0f;  //Radius of the circular text arc.

    [Tooltip("The radius of the text circle arc")]
    public float Radius  //The radius is public so you can tweak it dynamically.
    {
        get => m_radius;
        set
        {
            m_radius = value;
            OnCurvePropertyChanged();
        }
    }

    private void Awake()
    {
        m_TextComponent = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        m_TextComponent.OnPreRenderText += UpdateTextCurve; //curve text before rendering
        OnCurvePropertyChanged();
    }

    private void Update()
    {
    #if UNITY_EDITOR
        if (!Application.isPlaying) return;
    #endif
        // m_TextComponent.ForceMeshUpdate();
    }


    private void OnDisable()
    {
        m_TextComponent.OnPreRenderText -= UpdateTextCurve;
    }

    protected void OnCurvePropertyChanged()
    {
        UpdateTextCurve(m_TextComponent.textInfo);
        m_TextComponent.ForceMeshUpdate();
    }

    protected void UpdateTextCurve(TMP_TextInfo textInfo)
    {
        Vector3[] vertices; //used to storing vertices of  at a time
        Matrix4x4 matrix;

        elapsedTime += Time.deltaTime;

        for (int i = 0; i < textInfo.characterInfo.Length; i++) {
            if (!textInfo.characterInfo[i].isVisible) //skip invisible charactersL e.g. spaces
                continue;

            // Get the index of the vertex for the current character
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            // This is <b>bold</b> and <i>italic</i> and <color=red>red</color>
            // each character can have its own material, so we need to get the correct one
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            vertices = textInfo.meshInfo[materialIndex].vertices;

            Vector3 charMidBaselinePos = new Vector2(
                (vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2,
                textInfo.characterInfo[i].baseLine); //use baseline incase of multiple lines

            for (int j = 0; j < 4; j++)
                vertices[vertexIndex + j] -= charMidBaselinePos;

            matrix = ComputeTransformationMatrix(charMidBaselinePos, textInfo, i);

            for (int j = 0; j < 4; j++)
                vertices[vertexIndex + j] = matrix.MultiplyPoint3x4(vertices[vertexIndex + j]);

            // Calculate radial offset
            float charTimeOffset = i * characterDelay;
            float t = Mathf.Repeat((elapsedTime - charTimeOffset) / animationDuration, 1f);
            float offsetAmount = growthCurve.Evaluate(t) * 10f; // adjust '10f' for pulse strength

            // Compute radial direction (from center outward)
            Vector3 radialDir = (Vector3)vertices[vertexIndex + 0]; // or mid point
            radialDir.z = 0;
            radialDir = radialDir.normalized;

            // Offset all 4 vertices outward/inward
            for (int j = 0; j < 4; j++)
                vertices[vertexIndex + j] += radialDir * offsetAmount;
        }
    }

    protected Matrix4x4 ComputeTransformationMatrix(Vector3 charMidBaselinePos, TMP_TextInfo textInfo, int charIdx) {
        // Calculate the radius for the current line
        float radiusForThisLine = m_radius + textInfo.lineInfo[textInfo.characterInfo[charIdx].lineNumber].baseline;

        // Calculate the circumference of the circle for the current line
        float circumference = 2 * radiusForThisLine * Mathf.PI;

        // Calculate the angle in radians based on the character's position and the circumference
        float angle = ((charMidBaselinePos.x / circumference - 0.5f) * 360 + 90) * Mathf.Deg2Rad;

        // Calculate the new x and y coordinates of the character's midpoint using sine and cosine
        float x0 = Mathf.Cos(angle);
        float y0 = Mathf.Sin(angle);

        // Create a new vector representing the new position of the character's midpoint
        Vector2 newMidBaselinePos = new Vector2(x0 * radiusForThisLine, -y0 * radiusForThisLine);

        // Calculate the rotation angle in degrees based on the character's new position
        float rotationAngle = -Mathf.Atan2(y0, x0) * Mathf.Rad2Deg - 90;

        // Create a transformation matrix using the new position, rotation, and scale
        return Matrix4x4.TRS(
            new Vector3(newMidBaselinePos.x, newMidBaselinePos.y, 0),
            Quaternion.AngleAxis(rotationAngle, Vector3.forward),
            Vector3.one
        );
    }
}