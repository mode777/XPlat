using Gwen.Net.CommonDialog;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Xml", Order = 602)]
    public class CommonDialogsTest : GUnit
    {
        public CommonDialogsTest(ControlBase parent)
            : base(parent)
        {
            GridLayout grid = new GridLayout(this);
            grid.Dock = Net.Dock.Fill;
            grid.SetColumnWidths(GridLayout.AutoSize, GridLayout.Fill);

            Button button;

            {
                Label openFile = null;

                button = new Button(grid);
                button.Margin = Net.Margin.Five;
                button.Text = "OpenFileDialog";
                button.Clicked += (sender, args) =>
                {
                    openFile.Text = "";

                    OpenFileDialog dialog = Gwen.Net.Xml.Component.Create<OpenFileDialog>(this);
                    dialog.InitialFolder = "C:\\";
                    dialog.Filters = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    dialog.Callback = (path) => openFile.Text = path != null ? path : "Cancelled";
                };

                openFile = new Label(grid);
                openFile.TextPadding = new Net.Padding(3, 0, 0, 0);
                openFile.Alignment = Net.Alignment.Left | Net.Alignment.CenterV;
            }

            {
                Label saveFile = null;

                button = new Button(grid);
                button.Margin = Net.Margin.Five;
                button.Text = "SaveFileDialog";
                button.Clicked += (sender, args) =>
                {
                    saveFile.Text = "";
                    SaveFileDialog dialog = Gwen.Net.Xml.Component.Create<SaveFileDialog>(this);
                    dialog.Callback = (path) => saveFile.Text = path != null ? path : "Cancelled";
                };

                saveFile = new Label(grid);
                saveFile.TextPadding = new Net.Padding(3, 0, 0, 0);
                saveFile.Alignment = Net.Alignment.Left | Net.Alignment.CenterV;
            }

            {
                Label createFile = null;

                button = new Button(grid);
                button.Margin = Net.Margin.Five;
                button.Text = "SaveFileDialog (create)";
                button.Clicked += (sender, args) =>
                {
                    createFile.Text = "";
                    SaveFileDialog dialog = Gwen.Net.Xml.Component.Create<SaveFileDialog>(this);
                    dialog.Title = "Create File";
                    dialog.OkButtonText = "Create";
                    dialog.Callback = (path) => createFile.Text = path != null ? path : "Cancelled";
                };

                createFile = new Label(grid);
                createFile.TextPadding = new Net.Padding(3, 0, 0, 0);
                createFile.Alignment = Net.Alignment.Left | Net.Alignment.CenterV;
            }

            {
                Label selectFolder = null;

                button = new Button(grid);
                button.Margin = Net.Margin.Five;
                button.Text = "FolderBrowserDialog";
                button.Clicked += (sender, args) =>
                {
                    selectFolder.Text = "";
                    FolderBrowserDialog dialog = Gwen.Net.Xml.Component.Create<FolderBrowserDialog>(this);
                    dialog.InitialFolder = "C:\\";
                    dialog.Callback = (path) => selectFolder.Text = path != null ? path : "Cancelled";
                };

                selectFolder = new Label(grid);
                selectFolder.TextPadding = new Net.Padding(3, 0, 0, 0);
                selectFolder.Alignment = Net.Alignment.Left | Net.Alignment.CenterV;
            }
        }
    }
}