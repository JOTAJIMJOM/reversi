using System; //oke niet judgen ik heb niet op handigheid gefocust
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "reversi";
scherm.BackColor = Color.LightGray;
scherm.ClientSize = new Size(500, 500); //misschien dit als variabele nemen voor de rest? nu is de rest complicated en ugly

Font font = new Font("Times New Roman", 12, FontStyle.Bold); //klasse voor de fonts zou chill zijn
Font stenenFont = new Font("Times New Roman", 16, FontStyle.Bold);
Size buttonSize = new Size(100, 30);
Size textSize = new Size(15, 20);

Button nieuwspel = new Button(); //klasse maken voor buttons lijkt me ook wel handig..
nieuwspel.Location = new Point(75, 40);
nieuwspel.Size = buttonSize;
scherm.Controls.Add(nieuwspel);
nieuwspel.BackColor = Color.FromArgb(255, 180, 180);
nieuwspel.Font = font;
nieuwspel.Text = "Nieuw spel";

Button help = new Button();
help.Location = new Point(325, 40);
help.Size = buttonSize;
scherm.Controls.Add(help);
help.BackColor = Color.FromArgb(180, 180, 255);
help.Font = font;
help.Text = "Help";

ComboBox bord = new ComboBox(); //dropdown menu opties koppelen aan verschillende bordgroottes
bord.Location = new Point(200, 120);
bord.Size = new Size(100, 30);
scherm.Controls.Add(bord);
bord.BackColor = Color.FromArgb(255, 255, 255);
bord.Font = font;
bord.Text = "6x6";
bord.Items.Add("4x4"); //idk of dit korter kan
bord.Items.Add("6x6");
bord.Items.Add("8x8");
bord.Items.Add("10x10");
bord.Font = font;

int intRood = 0; //moet updaten bij een refreshed aantal stenen dat al geplaatst is op het bord
int intBlauw = 0;

Label text2 = new Label();//deze stuff moet sowieso een klasse holy moly wat ugly
text2.Location = new Point(150, 115);
text2.Size = textSize;
text2.Font = stenenFont;
scherm.Controls.Add(text2);
text2.ForeColor = Color.Red;
text2.Text = $"{intRood}";

Label text3 = new Label();
text3.Location = new Point(333, 115);
text3.Size = textSize;
text3.Font = stenenFont;
scherm.Controls.Add(text3);
text3.ForeColor = Color.Blue;
text3.Text = $"{intBlauw}";

Label text = new Label();
text.Location = new Point(203, 45); //bro waar slaat dit op
text.Font = font;
scherm.Controls.Add(text);

int beurt = 1;

if (beurt % 2 != 0)
{
    text.ForeColor = Color.DarkRed;
    text.Text = "Rood aan zet";
}

if (beurt % 2 == 0) //hoi lucas!! fijn datie werkt (^:
{
    text.ForeColor = Color.DarkBlue;
    text.Text = "Blauw aan zet";
}

/*if ()
{
    text.ForeColor = Color.DarkRed;
    text.Text = "Rood heeft gewonnen!";

    alle troep van de interface afgooien

}

if ()
{
    text.ForeColor = Color.DarkBlue;
    text.Text = "Blauw heeft gewonnen!";

    alle troep van de interface afgooien

}*/

Bitmap bitmap = new Bitmap(500, 500); //voor alles dat niet verandert bij het spelen van stenen
Label afbeelding = new Label();
scherm.Controls.Add(afbeelding);
afbeelding.Location = new Point(0, 0);
afbeelding.Size = new Size(500, 500);
afbeelding.Image = bitmap;
Graphics bitgr = Graphics.FromImage(bitmap);

int x = 100;
int y = 75;
int t = 0;

bitgr.FillRectangle(Brushes.DarkGray, x, x + y, 3 * x, 3 * x); //bordondergrond gewoon voor de mooi

Pen pen = new Pen(Color.Black, 3);
Point[] lijntjes = new Point[13];
foreach (Point p in lijntjes) //succes hiermee lol
{
    if (t < 7)
    {
        bitgr.DrawLine(pen, x + x / 2 * t, x + y, x + x / 2 * t, x * 4 + y);
        t = t + 1;
    }
    if (t >= 7)
    {
        bitgr.DrawLine(pen, x, x + x / 2 * (t - 7) + y, x * 4, x + x / 2 * (t - 7) + y);
        t = t + 1;
    }
}

bitgr.FillEllipse(Brushes.Red, x, x, x / 2, x / 2); //deze dingen en de stenen zelf maak ik nog wel een keer mooier
bitgr.FillEllipse(Brushes.Blue, 3 * x + x / 2, x, x / 2, x / 2); //de technieken verbeteren however mag jij doen als je wil

Point hier = new Point();
void teken(object o, PaintEventArgs pea)
{
    Graphics paintgr = pea.Graphics;
    paintgr.FillEllipse(Brushes.Red, hier.X - 25, hier.Y - 25, 50, 50); //ik snap niet hoe je dit werkend krijgt
    //stenen tekenen
}
void muisklik(object o, MouseEventArgs mea)
{
    hier = mea.Location;
    scherm.Invalidate();
    //if (hier = ...)
    //locatie waar steentje komt
}

void klik(object o, EventArgs ea)
{
    if (o == nieuwspel)
    {
        scherm.Invalidate(); //idk hoe dit werkt either
    }
    if (o == help)
    {
        //kleine witte cirkeltjes op beschikbare plekken tekenen
        scherm.Invalidate();
    }
    if (o == bord) //eh outdated maar uiteindelijk moet je ofc wel echt een andere bordgrootte kunnen krijgen
    {

    }
}

scherm.MouseClick += muisklik; //hoe werkt dit
//nieuwspel.Click += klik(); HOE WERKT DIT

scherm.Paint += teken;
Application.Run(scherm); //i am silly :3