using UnityEngine;

namespace Items
{
    public class Consumable : MonoBehaviour
    {
        public enum ItemType
        {
            HealthPotion,
            Fireball,
            ScrollOfConfusion
        }

        [SerializeField]
        private ItemType type;

        [SerializeField]
        private int value; // Value property for the consumable

        public ItemType Type
        {
            get { return type; }
        }

        public int Value
        {
            get { return value; }
        }

        private void Start()
        {
            GameManager.Get.AddItem(this);
        }
    }
}
