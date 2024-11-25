using TMPro;
using UnityEngine;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI 요소들")]
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private TextMeshProUGUI _levelText;

        /// <summary>
        /// HP 텍스트 업데이트
        /// </summary>
        /// <param name="currentHP">현재 HP</param>
        public void UpdateHP(int currentHP)
        {
            if (_hpText != null)
            {
                _hpText.text = $"HP: {currentHP}";
            }
        }

        /// <summary>
        /// 레벨 텍스트 업데이트
        /// </summary>
        /// <param name="currentLevel">현재 레벨</param>
        public void UpdateLevel(int currentLevel)
        {
            if (_levelText != null)
            {
                _levelText.text = $"Level: {currentLevel}";
            }
        }
    }
}