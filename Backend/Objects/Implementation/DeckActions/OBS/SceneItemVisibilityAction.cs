using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NickAc.Backend.Objects.Implementation.DeckActions.OBS
{
    public class SceneItemVisibilityAction : AbstractDeckAction
    {
        public enum ItemVisibility
        {
            Visible,
            Gone
        }


        public string SceneName { get; set; } = "";

        [ActionPropertyInclude]
        [ActionPropertyDescription("Scene Item")]
        public String SceneItem { get; set; } = "";

        [ActionPropertyInclude]
        [ActionPropertyDescription("Visibility")]
        public ItemVisibility ItemVisibilityStatus { get; set; }

        public void SceneItemHelper()
        {
            var oldSceneName = new String(SceneName.ToCharArray());
            var oldScene = new String(SceneItem.ToCharArray());
            dynamic form = Activator.CreateInstance(FindType("ButtonDeck.Forms.ActionHelperForms.OBS.OBSSceneItemVisibilityHelper")) as Form;
            var execAction = CloneAction() as SceneItemVisibilityAction;
            execAction.SceneName = SceneName;
            execAction.SceneItem = SceneItem;
            form.ModifiableAction = execAction;

            if (form.ShowDialog() == DialogResult.OK) {
                SceneName = form.ModifiableAction.SceneName;
                SceneItem = form.ModifiableAction.SceneItem;
            } else {
                SceneName = oldSceneName;
                SceneItem = oldScene;
            }
        }

        public override AbstractDeckAction CloneAction()
        {
            return new SceneItemVisibilityAction();
        }

        public override DeckActionCategory GetActionCategory()
        {
            return DeckActionCategory.OBS;
        }

        public override string GetActionName()
        {
            return "Scene Item Visibility";
        }

        public override void OnButtonDown(DeckDevice deckDevice)
        {
        }

        public override void OnButtonUp(DeckDevice deckDevice)
        {
            if (string.IsNullOrEmpty(SceneItem) || string.IsNullOrEmpty(SceneName)) return;
            OBSUtils.SetSceneItemVisibility(SceneItem, SceneName, ItemVisibilityStatus == ItemVisibility.Visible);
        }
    }
}
