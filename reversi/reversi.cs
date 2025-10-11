using System; //oke niet judgen ik heb niet op handigheid gefocust
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "reversi";
scherm.BackColor = Color.Moccasin;
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
int bordVakjeLengte = bordLengte / bordDimensie;
Pen penZwart = new Pen(Color.Black, 3);

int[,,] bordData = new int[bordDimensie, bordDimensie, 3];
void bordDimensieSelectie(object o, EventArgs ea)
{
    if (bordSelectie.SelectedItem == "4x4")//geselecteerde bordgrootte toepassen
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

    bordVakjeLengte = bordLengte / bordDimensie;

    bitgr.FillRectangle(Brushes.WhiteSmoke, bordX, bordY, bordLengte, bordLengte); //bordondergrond tekenen

    //bordData array aanmaken voor gebruik is de eerste waarde het x vak tweede het y vak en derde een waarde van het vakje
        {
            //klassen aanroepen voor steentjes tekenen
            //vgm is dit tzelfde als wat jij hieronder doet maar <deze manier is als t goed is wel meer de bedoeling, kijk maar of je er iets mee doet lol
        }
    }
    //bordData array aanmaken, voor gebruik is de eerste waarde het x vak tweede het y vak en derde een waarde van het vakje
        {
            //klassen aanroepen voor steentjes tekenen
            //vgm is dit tzelfde als wat jij hieronder doet maar <deze manier is als t goed is wel meer de bedoeling, kijk maar of je er iets mee doet lol
        }
    }
    //bordData array aanmaken, voor gebruik is de eerste waarde het x vak tweede het y vak en derde een waarde van het vakje
        {
            //klassen aanroepen voor steentjes tekenen
            //vgm is dit tzelfde als wat jij hieronder doet maar <deze manier is als t goed is wel meer de bedoeling, kijk maar of je er iets mee doet lol
        }
    }
    //bordData array aanmaken, voor gebruik is de eerste waarde het x vak tweede het y vak en derde een waarde van het vakje
    //0 is voor het vakjes x coordinaat, 1 is voor het y coordinaat en 2 is voor de status van het vakje
    bordData = new int[bordDimensie, bordDimensie, 3];

    for (int x = 0; x <= bordDimensie; x += 1) //bordlijnen tekenen en bordData Array x en y cordinaten vakjes geven en de vakjes hun waarde op 0 zetten
    {
        int xPos = (int)(x / (double)(bordDimensie) * bordLengte) + bordX;
        bitgr.DrawLine(penZwart, xPos, bordY, xPos, bordY + bordLengte);

        if (x < bordDimensie)
        {
            for (int y = 0; y < bordDimensie; y += 1)
            {
                bordData[x, y, 2] = 0;
                bordData[x, y, 0] = xPos;
            }
        }
    }
    for (int x = 0; x <= bordDimensie; x += 1)
}

bitgr.FillEllipse(Brushes.Red, bordX, bordX, bordX / 2, bordX / 2); //deze dingen en de stenen zelf maak ik nog wel een keer mooier
bitgr.FillEllipse(Brushes.Blue, 3 * bordX + bordX / 2, bordX, bordX / 2, bordX / 2); //de technieken verbeteren however mag jij doen als je wil

Point hier = new Point();

void teken (object o, PaintEventArgs pea)
{
    Graphics paintgr = pea.Graphics;

    for (int x = 0; x < bordDimensie; x += 1)
    {
        for (int y = 0; y < bordDimensie; y += 1)
        {
            if (bordData[x, y, 2] == 1)
            {
                paintgr.FillEllipse(Brushes.Red, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
            for (int y = 0; y < bordDimensie; y += 1)
        if (t < bordDimensie)
        {
            for (int t2 = 0; t2 < bordDimensie; t2++)
            {
                bordData[t2, t, 1] = yPos;

    //beginstenen op het bordt zetten
    bordData[bordDimensie / 2, bordDimensie / 2, 2] = 1;
    bordData[bordDimensie / 2 - 1, bordDimensie / 2 - 1, 2] = 1;
    bordData[bordDimensie / 2 - 1, bordDimensie / 2, 2] = 2;
    bordData[bordDimensie / 2, bordDimensie / 2 - 1, 2] = 2;

        if (t < bordDimensie)
        {
            for (int t2 = 0; t2 < bordDimensie; t2++)
            {
                bordData[t2, t, 1] = yPos;
            }
            if (bordData[x, y, 2] == 2)
            {
                paintgr.FillEllipse(Brushes.Blue, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
            }
        }
    }
}
void bordGekliked(object o, MouseEventArgs mea)
{
    hier = mea.Location;

    int selectedVakNumHorizontaal = 0;
    while (selectedVakNumHorizontaal * bordVakjeLengte < hier.X - bordX)
    {
        selectedVakNumHorizontaal += 1;
    }
    selectedVakNumHorizontaal -= 1;
    int selectedVakNumVerticaal = 0;
    while (selectedVakNumVerticaal * bordVakjeLengte < hier.Y - bordY)
    {
        selectedVakNumVerticaal += 1;
    }
    selectedVakNumVerticaal -= 1;

    if (selectedVakNumHorizontaal < 0 || selectedVakNumHorizontaal >= bordDimensie) ;
    else
    {
        if (selectedVakNumVerticaal < 0 || selectedVakNumVerticaal >= bordDimensie) ;
        else
        {
            if (beurt % 2 == 1)
            {
                bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 2] = 1;
            }
            else
            {
                bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 2] = 2;
            }
            beurt += 1;
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
    if (o == bordSelectie) //eh outdated maar uiteindelijk moet je ofc wel echt een andere bordgrootte kunnen krijgen
    {

    }
}

afbeelding.MouseClick += bordGekliked;
bordSelectie.SelectedValueChanged += bordDimensieSelectie;
afbeelding.Paint += teken;


bordDimensieSelectie(null, null);

Application.Run(scherm);