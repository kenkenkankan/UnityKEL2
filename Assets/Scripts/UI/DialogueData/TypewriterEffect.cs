using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typewriterSpeed = 50f;
    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        return StartCoroutine(TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;

        

        float t = 0;
        float emulateDelta = 0.33f;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            // t += Time.deltaTime * typewriterSpeed;
            t += emulateDelta * typewriterSpeed; // Not affected by pause
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType[..charIndex]; // equal to Substring(0, charIndex)

            // yield return new WaitWhile(() => charIndex < textToType.Length);
            yield return null;
        }

        textLabel.text = textToType;

    }
}
