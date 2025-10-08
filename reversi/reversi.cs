using System;
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "reversi";
scherm.BackColor = Color.LightGray;
scherm.ClientSize = new Size(500, 500); //misschien dit als variabele nemen voor de rest? nu is de rest complicated en ugly

Font font = new Font("Times New Roman", 11, FontStyle.Bold); //klasse voor de fonts zou chill zijn
Font stenenFont = new Font("Times New Roman", 12, FontStyle.Bold);
Size buttonSize = new Size(100, 30);
Size textSize = new Size(22, 20);

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

int intRood = 2; //moet updaten bij een refreshed aantal stenen dat al geplaatst is op het bord
int intBlauw = 2;

Label text2 = new Label();//deze stuff moet sowieso een klasse holy moly wat ugly
text2.Location = new Point(115, 115);
text2.Size = textSize;
text2.Font = stenenFont;
scherm.Controls.Add(text2);
text2.ForeColor = Color.Red;
text2.Text = $"{intRood}";

Label text3 = new Label();
text3.Location = new Point(365, 115);
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

/*if (intRood > intBlauw) bij einde spel
{
    text.ForeColor = Color.DarkRed;
    text.Text = "Rood heeft gewonnen!";

    alle troep van de interface afgooien

}

if (intRood < intBlauw) bij einde spel
{
    text.ForeColor = Color.DarkBlue;
    text.Text = "Blauw heeft gewonnen!";

    alle troep van de interface afgooien

}*/

Bitmap bitmapBord = new Bitmap(500, 500); //voor alles dat niet verandert bij het spelen van stenen
Label afbeelding = new Label();
scherm.Controls.Add(afbeelding);
afbeelding.Location = new Point(0, 0);
afbeelding.Size = new Size(500, 500);
afbeelding.Image = bitmapBord;
Graphics bitgr = Graphics.FromImage(bitmapBord);

int bordX = 100;
int bordY = 180;
int bordLengte = 300;
int bordDimensie = 6;
Pen penZwart = new Pen(Color.Black, 3);

void bordDimensieSelectie(object o, EventArgs ea)
{
    if (bordSelectie.SelectedItem == "4x4")
    {
        bordDimensie = 4;
        penZwart.Width = 4;
        afbeelding.Invalidate();
    }
    if (bordSelectie.SelectedItem == "6x6")
    {
        bordDimensie = 6;
        penZwart.Width = 3;
        afbeelding.Invalidate();
    }
    if (bordSelectie.SelectedItem == "8x8")
    {
        bordDimensie = 8;
        penZwart.Width = 2;
        afbeelding.Invalidate();
    }
    if (bordSelectie.SelectedItem == "10x10")
    {
        bordDimensie = 10;
        penZwart.Width = 2;
        afbeelding.Invalidate();
    }

    int[,] bordStatus = new int[5, 5];
    bordStatus[2, 2] = 2; bordStatus[3, 2] = 1; bordStatus[2, 3] = 2; bordStatus[3, 3] = 1;
    foreach (int i in bordStatus)
    {
        if (i == 1)
        {
            //klassen aanroepen voor steentjes tekenen
            //vgm is dit tzelfde als wat jij hieronder doet maar <deze manier is als t goed is wel meer de bedoeling, kijk maar of je er iets mee doet lol
        }
    }
    //bordData array aanmaken, voor gebruik is de eerste waarde het x vak tweede het y vak en derde een waarde van het vakje
    //0 is voor het vakjes x coordinaat, 1 is voor het y coordinaat en 2 is voor de status van het vakje
    int[,,] bordData = new int[bordDimensie, bordDimensie, 3];


    bitgr.FillRectangle(Brushes.DarkGray, bordX, bordY, bordLengte, bordLengte); //bordondergrond
    for (int t = 0; t <= bordDimensie; t++) //bordlijnen tekenen en bordData Array x en y cordinaten vakjes geven en de vakjes hun waarde op 0 zetten
    {
        int xPos = (int)(t / (double)(bordDimensie) * bordLengte) + bordX;
        bitgr.DrawLine(penZwart, xPos, bordY, xPos, bordY + bordLengte);

        if (t < bordDimensie)
        {
            for (int t2 = 0; t2 < bordDimensie; t2++)
            {
                bordData[t, t2, 2] = 0;
                bordData[t, t2, 0] = xPos;
            }
        }
    }
    for (int t = 0; t <= bordDimensie; t++)
    {
        int yPos = (int)(t / (double)(bordDimensie) * bordLengte) + bordY;
        bitgr.DrawLine(penZwart, bordX, yPos, bordX + bordLengte, yPos);

        if (t < bordDimensie)
        {
            for (int t2 = 0; t2 < bordDimensie; t2++)
            {
                bordData[t2, t, 1] = yPos;
            }
        }
    }
}

Pen penRood = new Pen(Brushes.Red, 10);
Pen penBlauw = new Pen(Brushes.Blue, 10);
bitgr.DrawEllipse(penRood, bordX, bordX, bordX / 2, bordX / 2);
bitgr.DrawEllipse(penBlauw, 3 * bordX + bordX / 2, bordX, bordX / 2, bordX / 2);

int beurt = 0;
text.ForeColor = Color.DarkRed;
text.Text = "Rood aan zet";

void muisklik(object o, MouseEventArgs mea)
{
    Point hier = mea.Location;
    if (bordX < hier.X && hier.X < bordX + bordLengte && bordY < hier.Y && hier.Y < bordY + bordLengte)
    {
        if (beurt % 2 == 0)
        {
            bitgr.FillEllipse(Brushes.Red, hier.X - 24, hier.Y - 24, 48, 48);
            bitgr.FillEllipse(Brushes.White, hier.X + 2, hier.Y - 19, 12, 12);
            text.ForeColor = Color.DarkBlue;
            text.Text = "Blauw aan zet";
            beurt++;
            intRood++;
            text2.Text = $"{intRood}";
            afbeelding.Invalidate();
        }
        else
        {
            bitgr.FillEllipse(Brushes.Blue, hier.X - 24, hier.Y - 24, 48, 48);
            bitgr.FillEllipse(Brushes.White, hier.X + 2, hier.Y - 19, 12, 12);
            text.ForeColor = Color.DarkRed;
            text.Text = "Rood aan zet";
            beurt++;
            intBlauw++;
            text3.Text = $"{intBlauw}";
            afbeelding.Invalidate();
        }
    }
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
}

afbeelding.MouseClick += muisklik; //hoe werkt dit
bordSelectie.SelectedValueChanged += bordDimensieSelectie;
//nieuwspel.Click += klik(); HOE WERKT DIT


bordDimensieSelectie(null, null);

Application.Run(scherm);