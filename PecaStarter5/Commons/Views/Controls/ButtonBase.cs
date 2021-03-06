﻿using System.Collections.Generic;
using System.Windows.Controls;

namespace Progressive.PecaStarter.View.Control
{
    delegate bool ClickBehavior();

    class ButtonBase : Button
    {
        private List<ClickBehavior> ClickBehaviors;

        public ButtonBase()
        {
            ClickBehaviors = new List<ClickBehavior> { };
        }

        public void AddBehavior(ClickBehavior clickBehavior)
        {
            ClickBehaviors.Add(clickBehavior);
        }

        protected override void OnClick()
        {
            if (Command != null && !Command.CanExecute(CommandParameter))
            {
                GetBindingExpression(Button.CommandProperty).ValidateWithoutUpdate();
                return;
            }

            foreach (ClickBehavior behavior in ClickBehaviors)
            {
                if (!behavior())
                {
                    return;
                }
            }
            base.OnClick();
        }
    }
}
