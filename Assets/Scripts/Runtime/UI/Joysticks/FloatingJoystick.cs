using UnityEngine;

namespace EEA.Game
{
    public class FloatingJoystick : Joystick
    {
        //[SerializeField] private Vector2 originalPosition;
        protected override void Start()
        {
            base.Start();

            background.pivot = new Vector2(0.5f, 0.5f);
            background.anchoredPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.25f);
        }

        public override void OnHold(Vector3 position)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(position);
            background.gameObject.SetActive(true);

            base.OnHold(position);
        }

        public override void OnFingerUp()
        {
            background.pivot = new Vector2(0.5f, 0.5f);
            background.anchoredPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.25f);

            base.OnFingerUp();
        }
    }
}