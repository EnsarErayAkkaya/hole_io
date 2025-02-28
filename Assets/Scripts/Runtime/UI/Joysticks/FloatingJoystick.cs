using UnityEngine;

namespace EEA.Game
{
    public class FloatingJoystick : Joystick
    {
        [SerializeField] private Vector2 originalPosition;
        protected override void Start()
        {
            base.Start();

            background.anchoredPosition = ScreenPointToAnchoredPosition(originalPosition);
        }

        public override void OnHold(Vector3 position)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(position);
            background.gameObject.SetActive(true);

            base.OnHold(position);
        }

        public override void OnFingerUp()
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(originalPosition);
            //background.gameObject.SetActive(false);
            base.OnFingerUp();
        }
    }
}