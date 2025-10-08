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
text.Location = new Point(203, 45);
text.Font = font;
scherm.Controls.Add(text);

ComboBox bordSelectie = new ComboBox(); //dropdown menu opties koppelen aan verschillende bordgroottes
bordSelectie.Location = new Point(200, 120);
bordSelectie.Size = new Size(100, 30);
scherm.Controls.Add(bordSelectie);
bordSelectie.BackColor = Color.FromArgb(255, 255, 255);
bordSelectie.Font = font;
bordSelectie.Text = "6x6";
bordSelectie.Items.Add("4x4");
bordSelectie.Items.Add("6x6");
bordSelectie.Items.Add("8x8");
bordSelectie.Items.Add("10x10");
bordSelectie.Font = font;

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

int bordX = 100;
int bordY = 180;
int bordLengte = 300;
int bordDimensie = 6;

void bordDimensieSelectie(object o, EventArgs ea)
{
    if (bordSelectie.SelectedItem == "4x4")
    {
        bordDimensie = 4;
    }
    if (bordSelectie.SelectedItem == "6x6")
    {
        bordDimensie = 6;
    }
    if (bordSelectie.SelectedItem == "8x8")
    {
        bordDimensie = 8;
    }
    if (bordSelectie.SelectedItem == "10x10")
    {
        bordDimensie = 10;
    }
}

Pen penZwart = new Pen(Color.Black, 3);


//bordData array aanmaken voor gebruik is de eerste waarde het x vak tweede het y vak en derde een waarde van het vakje
//0 is voor het vakjes x coordinaat, 1 is voor het y coordinaat en 2 is voor de status van het vakje
int[,,] bordData = new int[bordDimensie,bordDimensie,3];


bitgr.FillRectangle(Brushes.DarkGray, bordX, bordY, bordLengte, bordLengte); //bordondergrond
for (int t = 0; t <= bordDimensie; t += 1) //bordlijnen tekenen en bordData Array x en y cordinaten vakjes geven en de vakjes hun waarde op 0 zetten
{
    int xPos = (int)(t / (double)(bordDimensie) * bordLengte) + bordX;
    bitgr.DrawLine(penZwart, xPos, bordY, xPos, bordY + bordLengte);

    if (t < bordDimensie)
    {
        for (int t2 = 0; t2 < bordDimensie; t2 += 1)
        {
            bordData[t, t2, 2] = 0;
            bordData[t, t2, 0] = xPos;
        }
    }
}
for (int t = 0; t <= bordDimensie; t += 1)
{
    int yPos = (int)(t / (double)(bordDimensie) * bordLengte) + bordY;
    bitgr.DrawLine(penZwart, bordX, yPos, bordX + bordLengte, yPos);

    if (t < bordDimensie)
    {
        for (int t2 = 0; t2 < bordDimensie; t2 += 1)
        { 
            bordData[t2, t, 1] = yPos;
        }
    }
}

bitgr.FillEllipse(Brushes.Red, bordX, bordX, bordX / 2, bordX / 2); //deze dingen en de stenen zelf maak ik nog wel een keer mooier
bitgr.FillEllipse(Brushes.Blue, 3 * bordX + bordX / 2, bordX, bordX / 2, bordX / 2); //de technieken verbeteren however mag jij doen als je wil

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
    if (o == bordSelectie) //eh outdated maar uiteindelijk moet je ofc wel echt een andere bordgrootte kunnen krijgen
    {

    }
}

scherm.MouseClick += muisklik; //hoe werkt dit
bordSelectie.SelectedValueChanged += bordDimensieSelectie;
//nieuwspel.Click += klik(); HOE WERKT DIT

Application.Run(scherm);