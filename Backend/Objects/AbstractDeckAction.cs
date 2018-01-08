using NickAc.Backend.Objects.Implementation.DeckActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NickAc.Backend.Objects
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ActionPropertyIncludeAttribute : Attribute
    { }


    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ActionPropertyHelperAttribute : Attribute
    {
        readonly Action targetMethod;
        public ActionPropertyHelperAttribute(Action targetMethod)
        {
            this.targetMethod = targetMethod;
        }
    }

    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ActionPropertyDescriptionAttribute : Attribute
    {
        readonly string description;

        public string Description {
            get {
                return description;
            }
        }

        public ActionPropertyDescriptionAttribute(string description)
        {
            this.description = description;
        }
    }

    [XmlInclude(typeof(ExecutableRunAction))]
    [XmlInclude(typeof(KeyPressAction))]
    public abstract class AbstractDeckAction
    {
        public static Type FindType(string fullName)
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.FullName.Equals(fullName));
        }

        public enum DeckActionCategory
        {
            General,
            AutoHotKey,
            OBS
        }
        public abstract DeckActionCategory GetActionCategory();
        public abstract string GetActionName();
        public abstract bool OnButtonClick(DeckDevice deckDevice);
        public abstract void OnButtonDown(DeckDevice deckDevice);
        public abstract void OnButtonUp(DeckDevice deckDevice);
        public abstract AbstractDeckAction CloneAction();
    }
}
