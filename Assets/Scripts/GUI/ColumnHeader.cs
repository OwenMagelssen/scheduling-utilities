using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SchedulingUtilities
{
	public class ColumnHeader : MonoBehaviour
	{
		[SerializeField] private Button button;
		public Button Button => button;
		[SerializeField] private TextMeshProUGUI label;
		public TextMeshProUGUI Label => label;
		[SerializeField] private SortIndicator sortIndicator;
		public SortIndicator SortIndicator => sortIndicator;
		
		public RectTransform RectTransform => _rectTransform;
		private RectTransform _rectTransform;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}
	}
}