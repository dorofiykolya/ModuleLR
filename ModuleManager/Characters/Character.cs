using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManager
{
    public class Character : Module
    {
        public int X;
        public int Y;
        public float XOffset;
        public float YOffset;
        public CharacterAction Action;

        private readonly float[] _properties;
        private readonly List<Modifier> _modifiers; 

        protected Character()
        {
            _properties = new float[Enum.GetValues(typeof(CharacterProperty)).Length];
            _modifiers = new List<Modifier>();
        }

        protected void SetPosition(int x, int y, float xOffset, float yOffset)
        {
            X = x;
            Y = y;
            XOffset = xOffset;
            YOffset = yOffset;
        }

        public override void Initialize()
        {

        }

        public void AddModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public void RemoveModifier(Modifier modifier)
        {
            _modifiers.Remove(modifier);
        }

        public float GetModifiedValue(CharacterProperty property)
        {
            var value = GetValue(property);
            foreach (var modifier in _modifiers)
            {
                value = modifier.GetValue(value);
            }
            return value;
        }

        public float GetValue(CharacterProperty property)
        {
            return _properties[(int)property];
        }

        public Character SetValue(CharacterProperty property, float value)
        {
            _properties[(int)property] = value;
            return this;
        }

        public float this[CharacterProperty property]
        {
            get { return GetValue(property); }
            set { SetValue(property, value); }
        }
    }
}
