using System;
using System.Numerics;
using Microsoft.Extensions.Logging;
using XPlat.Core;
using XPlat.NanoGui;
using XPlat.NanoVg;

public class NanoGuiApp : Screen
{
    private readonly ILogger<NanoGuiApp> logger;
    private readonly ISdlPlatformEvents events;
    private readonly XPlat.NanoGui.Window window;
    private Label label;
    private Label label1;
    private Label label2;
    private Label label3;
    private XPlat.NanoGui.Window window2;
    private Button button;

    public NanoGuiApp(ILogger<NanoGuiApp> logger, ISdlPlatformEvents events, IPlatform info) : base(info, events)
    {
        this.logger = logger;
        this.events = events;

        this.window = new XPlat.NanoGui.Window(this, "Metrics Panel");
        window.Position = new Vector2(15, 15);
        window.Layout = new GroupLayout();

        this.label1 = new Label(window, "", "sans-bold");
        this.label2 = new Label(window, "", "sans-bold");
        this.label3 = new Label(window, "", "sans-bold");

        new Label(window, "Push Buttons");
        button = new Button(window, "Plain Button");
        button.Tooltip = "Short tooltip";
        button = new Button(window, "Styled", (int)Icons.FA_ROCKET);
        button.BackgroundColor = "#0000FF19";
        button.Tooltip = "This button has a fairly long tooltip. It is so long, in fact, that the shown text will span several lines.";
        button.OnPush += (s, a) => button.Tooltip = "Button clicked";

        new Label(window, "Toggle buttons", "sans-bold");
        var b = new Button(window, "Toggle me")
        {
            Flags = ButtonFlags.ToggleButton
        };
        b.OnChange += (s, a) => b.Caption = a ? "On" : "Off";

        new Label(window, "Radio buttons", "sans-bold");
        new Button(window, "Radio button 1") { Flags = ButtonFlags.RadioButton };
        new Button(window, "Radio button 2") { Flags = ButtonFlags.RadioButton };

        new Label(window, "A tool palette", "sans-bold");
        var tools = new Widget(window) { Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 6) };
        new ToolButton(tools, (int)Icons.FA_CLOUD);
        new ToolButton(tools, (int)Icons.FA_FAST_FORWARD);
        new ToolButton(tools, (int)Icons.FA_COMPASS);
        new ToolButton(tools, (int)Icons.FA_UTENSILS);

        new Label(window, "Popup buttons", "sans-bold");
        PopupButton popupBtn = new PopupButton(window, "Popup", (int)Icons.FA_FLASK);
        var popup = popupBtn.Popup;
        popup.Layout = new GroupLayout();
        new Label(popup, "Arbitrary widgets can be placed here");
        new Checkbox(popup, "A check box");

        popupBtn = new PopupButton(popup, "Recursive Popup", (int)Icons.FA_CHART_PIE);
        var popupRight = popupBtn.Popup;
        popupRight.Layout = new GroupLayout();
        new Checkbox(popupRight, "Another checkbox");

        popupBtn = new PopupButton(popup, "Recursive popup", (int)Icons.FA_DNA);
        popupBtn.SetSide(PopupSide.Left);
        var popupLeft = popupBtn.Popup;
        popupLeft.Layout = new GroupLayout();
        new Checkbox(popupLeft, "Another checkbox");

        window = new XPlat.NanoGui.Window(this, "Basic Widgets");
        window.Position = new Vector2(200, 15);
        window.Layout = new GroupLayout();

        new Label(window, "Message dialog", "sans-bold");
        tools = new Widget(window);
        tools.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 6);
        b = new Button(tools, "Info");
        b.OnPush += (s, a) => {
            var dlg = new MessageDialog(this, MessageDialogType.Information, "Title", "This is an infomation message");
        };
        b = new Button(tools, "Warn");
        b.OnPush += (s, a) => new MessageDialog(this, MessageDialogType.Warning, "Title", "This is a wanring message");
        b = new Button(tools, "Ask");
        b.OnPush += (s, a) => new MessageDialog(this, MessageDialogType.Question, "Title", "This is a question message", "Yes", "No", true);

        new Label(window, "Image Panel & scroll panel", "sans-bold");
        var imagePanelBtn = new PopupButton(window, "Image Panel", (int)Icons.FA_IMAGES);
        popup = imagePanelBtn.Popup;
        var vScroll = new VScrollPanel(popup);
        var imgPanel = new ImagePanel(vScroll);
        var icons = Directory.EnumerateFiles("assets/icons")
            .Where(x => Path.GetExtension(x) == ".png")
            .Select(x => nvgContext.CreateImage(x, 0))
            .ToList();
        imgPanel.Images.AddRange(icons);

        // ...
        popup.FixedSize = new Vector2(245, 150);



        UpdateValues();
        PerformLayout();
    }

    public void UpdateValues()
    {
        label1.Caption = "Mouse Position " + MousePos.ToString();
        label2.Caption = "Runtime(s) " + ((int)Time.RunningTime).ToString();
        label3.Caption = "Focused " + button.MouseFocus;
    }


    public override void Update()
    {
        UpdateValues();
        base.Update();
    }

    public override void DrawContents()
    {
        base.DrawContents();
        var vg = nvgContext;
        vg.BeginPath();
        vg.Circle(Platform.MousePosition.X, Platform.MousePosition.Y, 2);
        vg.FillColor("#ff0000");
        vg.Fill();
    }
}

