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
    public abstract class AbstractDeckAction
    {
        public enum DeckActionCategory
        {
            General,
            AutoHotKey,
            OBS
        }
        public abstract DeckActionCategory GetActionCategory();
        public abstract string GetActionName();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deckDevice"></param>
        /// <returns>True if the button down and button up must be suppressed</returns>
        public abstract bool OnButtonClick(DeckDevice deckDevice);
        public abstract void OnButtonDown(DeckDevice deckDevice);
        public abstract void OnButtonUp(DeckDevice deckDevice);
    }
}
