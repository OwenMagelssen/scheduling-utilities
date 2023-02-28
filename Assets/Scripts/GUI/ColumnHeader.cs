using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SchedulingUtilities
{
	public class ColumnHeader : MonoBehaviour
	{
		public Button button;
		public TextMeshProUGUI label;
		public RectTransform RectTransform => _rectTransform;
		private RectTransform _rectTransform;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}
	}
}