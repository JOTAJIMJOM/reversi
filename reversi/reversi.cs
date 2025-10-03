using System;
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "reversi";
scherm.BackColor = Color.LightGray;
scherm.ClientSize = new Size(500, 500);

Font font = new Font("Times New Roman", 10, FontStyle.Bold);

Button nieuwspel = new Button();
nieuwspel.Location = new Point(75, 40);
scherm.Controls.Add(nieuwspel);
nieuwspel.BackColor = Color.FromArgb(255, 180, 180);
nieuwspel.Font = font;
nieuwspel.Text = "Nieuw spel";

Button help = new Button();
help.Location = new Point(350, 40);
scherm.Controls.Add(help);
help.BackColor = Color.FromArgb(180, 180, 255);
help.Font = font;
help.Text = "Help";

Label text = new Label();
text.Location = new Point(210, 45);
text.Font = new Font("Times New Roman", 12, FontStyle.Bold);
scherm.Controls.Add(text);

//if ()
//{
    text.ForeColor = Color.DarkRed;
    text.Text = "Rood aan zet";
//}

/*if ()
{
    text.ForeColor = Color.DarkBlue;
    text.Text = "Blauw aan zet";
}*/

void teken(object o, PaintEventArgs pea)
{
    Graphics gr = pea.Graphics;
    int x = 100;
    int y = 75;
    int t = 0;
    Pen pen = new Pen(Color.Black, 3);
    Point[] lijntjes = new Point[13];
    foreach (Point p in lijntjes)
    {
        if (t < 7)
        {
            gr.DrawLine(pen, x + x / 2 * t, y + x, x + x / 2 * t, y + x * 4);
            t = t + 1;
        }
        if (t >= 7)
        {
            gr.DrawLine(pen, x, y + x + x / 2 * (t - 7), x * 4, y + x + x / 2 * (t - 7));
            t = t + 1;
        }
    }
}

Point hier = new Point();

void klik(object o, MouseEventArgs mea)
{
    hier = mea.Location;
    scherm.Invalidate();
}

scherm.MouseClick += klik;
scherm.Paint += teken;
Application.Run(scherm);