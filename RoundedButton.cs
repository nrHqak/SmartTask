using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SmartTaskDistributor;

public class RoundedButton : Button
{
    public Color HoverBackColor { get; set; } = Color.FromArgb(0, 194, 255);
    public Color NormalBackColor { get; set; } = Color.FromArgb(76, 175, 80);

    public RoundedButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        ForeColor = Color.White;
        BackColor = NormalBackColor;
        Cursor = Cursors.Hand;
        MouseEnter += (_, _) => BackColor = HoverBackColor;
        MouseLeave += (_, _) => BackColor = NormalBackColor;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        using GraphicsPath path = new();
        int radius = 12;
        Rectangle rect = ClientRectangle;
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
        path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
        path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
        path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
        path.CloseFigure();
        Region = new Region(path);
    }
}
