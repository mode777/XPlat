using System;

namespace XPlat.NanoGui
{
    public enum MessageDialogType {
        Information,
        Question, 
        Warning
    }

    public class MessageDialog : Window
    {
        public MessageDialog(Widget parent, 
            MessageDialogType type, 
            string title = "Untitled", 
            string message = "Message", 
            string buttonText = "OK", 
            string altButtonText = 
            "Cancel", 
            bool altButton = false) : base(parent, title)
        {
            Layout = new BoxLayout(Orientation.Vertical, Alignment.Middle, 10, 10);
            Modal = true;
            
            var panel1 = new Widget(this);
            panel1.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 10, 15);

            int icon = 0;
            switch (type)
            {
                case MessageDialogType.Information: icon = Theme.InformationIcon; break;
                case MessageDialogType.Question: icon = Theme.QuestionIcon; break;
                case MessageDialogType.Warning: icon = Theme.WarningIcon; break;
            }
            var iconLabel = new Label(panel1, char.ConvertFromUtf32(icon), "icons");
            iconLabel.FontSize = 50;
            this.MessageLabel = new Label(panel1, message);
            MessageLabel.FixedWidth = 200;
            var panel2 = new Widget(this);
            panel2.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 15);
            panel2.Id = "test2";

            if(altButton){
                var buttonAlt = new Button(panel2, altButtonText, Theme.MessageAltButtonIcon);
                buttonAlt.OnPush += (s, a) =>
                {
                    OnResult?.Invoke(buttonAlt, true);
                    Dispose();
                };
            } 
            var button = new Button(panel2, buttonText, Theme.MessagePrimaryButtonIcon);
            button.OnPush += (s, a) =>
            {
                OnResult?.Invoke(button, false);
                Dispose();
            };
            button.Id = "test";

            Center();
            RequestFocus();
        }
        public Label MessageLabel { get; }
        public event EventHandler<bool> OnResult;
    }
}