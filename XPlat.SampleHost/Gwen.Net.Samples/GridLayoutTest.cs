using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Layout", Order = 404)]
    public class GridLayoutTest : GUnit
    {
        public GridLayoutTest(ControlBase parent)
            : base(parent)
        {
            GridLayout grid = CreateGrid(this);
            grid.Dock = Net.Dock.Fill;
        }

        private GridLayout CreateGrid(ControlBase parent)
        {
            GridLayout grid = new GridLayout(parent);

            grid.SetColumnWidths(0.2f, GridLayout.AutoSize, 140.0f, 0.8f);
            grid.SetRowHeights(0.2f, GridLayout.AutoSize, 140.0f, 0.8f);

            CreateControl(grid, "C: 20%, R: 20%");
            CreateControl(grid, "C: Auto R: 20%");
            CreateControl(grid, "C: 140, R: 20%");
            CreateControl(grid, "C: 80%, R: 20%");

            CreateControl(grid, "C: 20%, R: Auto");
            CreateControl(grid, "C: Auto R: Auto");
            CreateControl(grid, "C: 140, R: Auto");
            CreateControl(grid, "C: 80%, R: Auto");

            CreateControl(grid, "C: 20%, R: 140");
            CreateControl(grid, "C: Auto R: 140");
            CreateControl(grid, "C: 140, R: 140");
            CreateControl(grid, "C: 80%, R: 140");

            CreateControl(grid, "C: 20%, R: 80%");
            CreateControl(grid, "C: Auto R: 80%");
            CreateControl(grid, "C: 140, R: 80%");
            CreateControl(grid, "C: 80%, R: 80%");

            return grid;
        }

        private void CreateControl(ControlBase parent, string text)
        {
            Button button = new Button(parent);
            button.Text = text;
        }
    }
}