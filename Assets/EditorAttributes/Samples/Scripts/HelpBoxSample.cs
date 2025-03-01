using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/helpbox.html")]
	public class HelpBoxSample : MonoBehaviour
	{
		[Header("HelpBox Attribute:")]
		[HelpBox("This is a help box", MessageMode.None)]
		[SerializeField] private int helpBox;

		[HelpBox("This is a log box", MessageMode.Log)]
		[SerializeField] private int logBox;

		[HelpBox("This is a warning box", MessageMode.Warning)]
		[SerializeField] private int warningBox;

		[HelpBox("This is an error box", MessageMode.Error)]
		[SerializeField] private int errorBox;
	}
}
