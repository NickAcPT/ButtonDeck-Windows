using System;
using System.Collections;
using System.ComponentModel;

namespace NickAc.Backend.Utils.Misc
{
    public class IFTTTRequestPropertyCollection : CollectionBase, ICustomTypeDescriptor
    {
        public void Add(IFTTTRequestProperty prop)
        {
            base.List.Add(prop);
        }

        public void Remove(string propertyName)
        {
            foreach (IFTTTRequestProperty prop in base.List) {
                if (prop.Name == propertyName) {
                    base.List.Remove(prop);
                    return;
                }
            }
        }

        public IFTTTRequestProperty this[int index] {
            get {
                return (IFTTTRequestProperty)base.List[index];
            }
            set {
                base.List[index] = value;
            }
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptor[] newProps = new PropertyDescriptor[this.Count];
            for (int i = 0; i < this.Count; i++) {
                IFTTTRequestProperty prop = (IFTTTRequestProperty)this[i];
                newProps[i] = new IFTTTRequestPropertyDescriptor(ref prop, attributes);
            }

            return new PropertyDescriptorCollection(newProps);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
    }

    public class IFTTTRequestPropertyDescriptor : PropertyDescriptor
    {
        public IFTTTRequestProperty BaseProperty { get; set; }

        public IFTTTRequestPropertyDescriptor(ref IFTTTRequestProperty prop, Attribute[] attrs) : base(prop.Name, attrs)
        {
            BaseProperty = prop;
        }

        public override Type ComponentType => null;

        public override bool IsReadOnly => false;

        public override Type PropertyType => BaseProperty.Value.GetType();

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return BaseProperty.Value;
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            BaseProperty.Value = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}