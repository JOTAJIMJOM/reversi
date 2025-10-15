using System;
using System.Drawing;
using System.Windows.Forms;

int beurt = 1;
int scoreSpeler1 = 2;
int scoreSpeler2 = 2;

int bordX = 100;
int bordY = 180;
int bordLengte = 300;
int bordDimensie = 6;

bool helpEnabled = false;
bool speler1HeeftZet = false;
bool speler2HeeftZet = false;
bool spelBezig = true;

int bordVakjeLengte = bordLengte / bordDimensie;
int[,,] bordData = new int[bordDimensie, bordDimensie, 5];

Form scherm = new Form();
scherm.ClientSize = new Size(500, 500);
scherm.BackColor = Color.FromArgb(100, 125, 65);
scherm.Text = "Reversi";

Font bigFont = new Font("Times New Roman", 14, FontStyle.Bold);
Font smallFont = new Font("Times New Roman", 12, FontStyle.Bold);
Size bigButtonSize = new Size(100, 30);
Size smallButtonSize = new Size(30, 30);
Size textSize = new Size(22, 20);

Button kleurKnop1 = new Button();
kleurKnop1.Location = new Point(40, 110);
kleurKnop1.Size = smallButtonSize;
scherm.Controls.Add(kleurKnop1);

Button kleurKnop2 = new Button();
kleurKnop2.Location = new Point(430, 110);
kleurKnop2.Size = smallButtonSize;
scherm.Controls.Add(kleurKnop2);

Color[] kleuren = new[]
{
    Color.Red, Color.DarkOrange, Color.Yellow, Color.YellowGreen, Color.DarkGreen, Color.Cyan, Color.Blue, Color.BlueViolet, Color.Magenta, Color.DimGray, Color.FromArgb(45, 45, 45)
};

Color kleur1 = kleuren[0];
Color kleur2 = kleuren[6];
kleurKnop1.BackColor = kleur1;
kleurKnop2.BackColor = kleur2;

Color lichteKleur1 = ControlPaint.LightLight(kleur1);
Color donkereKleur1 = ControlPaint.Dark(kleur1);
Color lichteKleur2 = ControlPaint.LightLight(kleur2);
Color donkereKleur2 = ControlPaint.Dark(kleur2);

Button nieuwspel = new Button();
nieuwspel.Location = new Point(75, 40);
nieuwspel.Size = bigButtonSize;
scherm.Controls.Add(nieuwspel);
nieuwspel.Font = smallFont;
nieuwspel.BackColor = lichteKleur1;
nieuwspel.Text = "Nieuw spel";

Button help = new Button();
help.Location = new Point(325, 40);
help.Size = bigButtonSize;
scherm.Controls.Add(help);
help.Font = smallFont;
help.BackColor = lichteKleur2;
help.Text = "Help";

Label aantalStenenKleur1 = new Label();
aantalStenenKleur1.Location = new Point(115, 115);
aantalStenenKleur1.TextAlign = ContentAlignment.MiddleCenter;
aantalStenenKleur1.Size = textSize;
scherm.Controls.Add(aantalStenenKleur1);
aantalStenenKleur1.Font = smallFont;
aantalStenenKleur1.ForeColor = kleur1;
aantalStenenKleur1.Text = $"{scoreSpeler1}";

Label aantalStenenKleur2 = new Label();
aantalStenenKleur2.Location = new Point(364, 115);
aantalStenenKleur2.TextAlign = ContentAlignment.MiddleCenter;
aantalStenenKleur2.Size = textSize;
scherm.Controls.Add(aantalStenenKleur2);
aantalStenenKleur2.Font = smallFont;
aantalStenenKleur2.ForeColor = kleur2;
aantalStenenKleur2.Text = $"{scoreSpeler2}";

Label status = new Label();
status.Location = new Point(200, 45);
status.TextAlign = ContentAlignment.MiddleCenter;
status.Size = new Size(100, 60);
scherm.Controls.Add(status);
status.Font = bigFont;

Label beurtTeller = new Label();
beurtTeller.Location = new Point(200, 15);
beurtTeller.TextAlign = ContentAlignment.MiddleCenter;
beurtTeller.Size = bigButtonSize;
scherm.Controls.Add(beurtTeller);
beurtTeller.Font = smallFont;
beurtTeller.ForeColor = Color.Black;
beurtTeller.Text = $"Beurt {beurt}";

ComboBox bordSelectie = new ComboBox();
bordSelectie.Location = new Point(200, 120);
bordSelectie.Size = new Size(100, 30);
scherm.Controls.Add(bordSelectie);
bordSelectie.Font = bigFont;
bordSelectie.BackColor = Color.FromArgb(255, 255, 255);
bordSelectie.Text = "6x6";
bordSelectie.Items.Add("4x4");
bordSelectie.Items.Add("6x6");
bordSelectie.Items.Add("8x8");
bordSelectie.Items.Add("10x10");

Bitmap bitmapBord = new Bitmap(500, 500);
Label bordLabel = new Label();
bordLabel.Location = new Point(0, 0);
bordLabel.Size = new Size(500, 500);
scherm.Controls.Add(bordLabel);
bordLabel.Image = bitmapBord;

Graphics bitgr = Graphics.FromImage(bitmapBord);
Pen penZwart = new Pen(Color.Black, 3);

void nieuwSpel(object o, EventArgs ea)
{
    if (bordSelectie.SelectedItem == "4x4")
    {
        bordDimensie = 4;
        penZwart.Width = 4;
    }
    if (bordSelectie.SelectedItem == "6x6")
    {
        bordDimensie = 6;
        penZwart.Width = 3;
    }
    if (bordSelectie.SelectedItem == "8x8")
    {
        bordDimensie = 8;
        penZwart.Width = 2;
    }
    if (bordSelectie.SelectedItem == "10x10")
    {
        bordDimensie = 10;
        penZwart.Width = 2;
    }
    beurt = 1;
    scoreSpeler1 = 2;
    scoreSpeler2 = 2;

    beurtTeller.Text = $"Beurt {beurt}";
    status.ForeColor = donkereKleur1;
    status.Text = "Speler 1 aan zet";
    aantalStenenKleur1.Text = $"{scoreSpeler1}";
    aantalStenenKleur2.Text = $"{scoreSpeler2}";
    bordLabel.Invalidate();
    spelBezig = true;

    bordVakjeLengte = bordLengte / bordDimensie;

    bitgr.FillRectangle(Brushes.WhiteSmoke, bordX, bordY, bordLengte, bordLengte);

    //bordData array aanmaken, eerste waarde = x-coördinaat, tweede waarde = y-coördinaat, derde waarde = status van het vakje
    //0 is voor de x-coördinaat, 1 is voor de y-coördinaat, 2 is voor de status van het vakje, 3 is voor of er op het vakje geplaatst kan worden door rood en 4 is voor of er op het vakje geplaatst kan worden door blauw
    bordData = new int[bordDimensie, bordDimensie, 5];

    for (int x = 0; x <= bordDimensie; x++) //bordlijnen tekenen, bordData array x- en y-vakjes geven en de waarde van de vakjes op 0 zetten
    {
        int xPos = (int)(x / (double)(bordDimensie) * bordLengte) + bordX;
        bitgr.DrawLine(penZwart, xPos, bordY, xPos, bordY + bordLengte);

        if (x < bordDimensie)
        {
            for (int y = 0; y < bordDimensie; y++)
            {
                bordData[x, y, 2] = 0;
                bordData[x, y, 0] = xPos;
            }
        }
    }
    for (int x = 0; x <= bordDimensie; x++)
    {
        int yPos = (int)(x / (double)(bordDimensie) * bordLengte) + bordY;
        bitgr.DrawLine(penZwart, bordX, yPos, bordX + bordLengte, yPos);
        if (x < bordDimensie)
        {
            for (int y = 0; y < bordDimensie; y++)
            {
                bordData[y, x, 1] = yPos;
            }
        }
    }

    //beginstenen op het bord zetten
    bordData[bordDimensie / 2, bordDimensie / 2, 2] = 1;
    bordData[bordDimensie / 2 - 1, bordDimensie / 2 - 1, 2] = 1;
    bordData[bordDimensie / 2 - 1, bordDimensie / 2, 2] = 2;
    bordData[bordDimensie / 2, bordDimensie / 2 - 1, 2] = 2;
}

Brush brush1 = new SolidBrush(kleur1);
Brush brush2 = new SolidBrush(kleur2);

Brush brushLight1 = new SolidBrush(lichteKleur1);
Brush brushLight2 = new SolidBrush(lichteKleur2);
Brush brushDark1 = new SolidBrush(donkereKleur1);
Brush brushDark2 = new SolidBrush(donkereKleur2);

Pen pen1 = new Pen(brush1, 10);
Pen pen2 = new Pen(brush2, 10);

void kanPlaatsenOp()
{
    speler2HeeftZet = false;
    speler1HeeftZet = false;
    for (int x = 0; x < bordDimensie; x++)
    {
        for (int y = 0; y < bordDimensie; y++)
        {
            bordData[x, y, 3] = 0;
            bordData[x, y, 4] = 0;
            if (bordData[x,y,2]==0)
            {
                for (int xSurrounding = -1; xSurrounding <= 1; xSurrounding++)
                {
                    for (int ySurrounding = -1; ySurrounding <= 1; ySurrounding++)
                    {
                        if (xSurrounding != 0 || ySurrounding != 0)
                        {
                            //de plaatsbare tiles voor rood in de bordData array zetten
                            if (x + xSurrounding >= 0 && x + xSurrounding < bordDimensie && y + ySurrounding >= 0 && y + ySurrounding < bordDimensie)
                            {
                                if (bordData[x + xSurrounding, y + ySurrounding, 2] == 2)
                                {
                                    int xOffset = xSurrounding;
                                    int yOffset = ySurrounding;

                                    while (bordData[x + xOffset, y + yOffset, 2] == 2)
                                    {
                                        if (x + xSurrounding + xOffset >= 0 && x + xSurrounding + xOffset < bordDimensie && y + ySurrounding + yOffset >= 0 && y + ySurrounding + yOffset < bordDimensie)
                                        {
                                            xOffset += xSurrounding;
                                            yOffset += ySurrounding;
                                        }
                                        else
                                        {
                                            break;
                                        }

                                        if (bordData[x + xOffset, y + yOffset, 2] == 1) //plaatsbaar
                                        {
                                            bordData[x, y, 3] = 1;
                                            speler1HeeftZet = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            //de plaatsbare tiles voor blauw in de bordData array zetten
                            if (x + xSurrounding >= 0 && x + xSurrounding < bordDimensie && y + ySurrounding >= 0 && y + ySurrounding < bordDimensie)
                            {
                                if (bordData[x + xSurrounding, y + ySurrounding, 2] == 1)
                                {
                                    int xOffset = xSurrounding;
                                    int yOffset = ySurrounding;

                                    while (bordData[x + xOffset, y + yOffset, 2] == 1)
                                    {
                                        if (x + xSurrounding + xOffset >= 0 && x + xSurrounding + xOffset < bordDimensie && y + ySurrounding + yOffset >= 0 && y + ySurrounding + yOffset < bordDimensie)
                                        {
                                            xOffset += xSurrounding;
                                            yOffset += ySurrounding;
                                        }
                                        else
                                        {
                                            break;
                                        }

                                        if (bordData[x + xOffset, y + yOffset, 2] == 2) //plaatsbaar
                                        {
                                            bordData[x, y, 4] = 1;
                                            speler2HeeftZet = true;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }

        }
    }

}

void updateOmliggendeStennen(int x, int y,int beurt)
{
    for (int xSurrounding = -1; xSurrounding <= 1; xSurrounding++)
    {
        for (int ySurrounding = -1; ySurrounding <= 1; ySurrounding++)
        {
            if (xSurrounding != 0 || ySurrounding != 0)
            {
                //de omliggende stenen voor kleur 1 updaten in bordData array
                if (beurt % 2 == 1)
                {
                    try
                    {
                        if (bordData[x + xSurrounding, y + ySurrounding, 2] == 2)
                        {
                            int xOffset = xSurrounding;
                            int yOffset = ySurrounding;
                            while (bordData[x + xOffset, y + yOffset, 2] == 2)
                            {
                                xOffset += xSurrounding;
                                yOffset += ySurrounding;
                                if (bordData[x + xOffset, y + yOffset, 2] == 1)
                                {
                                    xOffset = xSurrounding;
                                    yOffset = ySurrounding;
                                    while (bordData[x + xOffset, y + yOffset, 2] == 2)
                                    {
                                        bordData[x + xOffset, y + yOffset, 2] = 1;
                                        xOffset += xSurrounding;
                                        yOffset += ySurrounding;
                                        if (bordData[x + xOffset, y + yOffset, 2] == 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
                else //de omliggende stenen voor kleur 2 updaten in bordData array
                {
                    try
                    {
                        if (bordData[x + xSurrounding, y + ySurrounding, 2] == 1)
                        {
                            int xOffset = xSurrounding;
                            int yOffset = ySurrounding;

                            while (bordData[x + xOffset, y + yOffset, 2] == 1)
                            {
                                xOffset += xSurrounding;
                                yOffset += ySurrounding;
                                if (bordData[x + xOffset, y + yOffset, 2] == 2)
                                {
                                    xOffset = xSurrounding;
                                    yOffset = ySurrounding;

                                    while (bordData[x + xOffset, y + yOffset, 2] == 1)
                                    {
                                        bordData[x + xOffset, y + yOffset, 2] = 2;
                                        xOffset += xSurrounding;
                                        yOffset += ySurrounding;
                                        if (bordData[x + xOffset, y + yOffset, 2] == 2)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
        }
    }
}

void eindeSpel()
{
    spelBezig = false;
    if (scoreSpeler1 > scoreSpeler2)
    {
        status.ForeColor = donkereKleur1;
        status.Text = "Speler 1 heeft gewonnen!";
        bordLabel.Invalidate();
    }
    else
    {
        if (scoreSpeler1 < scoreSpeler2)
        {
            status.ForeColor = donkereKleur2;
            status.Text = "Speler 2 heeft gewonnen!";
            bordLabel.Invalidate();
        }
        else
        {
            status.ForeColor = Color.Black;
            status.Text = "Remise";
            bordLabel.Invalidate();
        }
    }
}

void teken(object o, PaintEventArgs pea)
{
    Graphics paintgr = pea.Graphics;
    kanPlaatsenOp();

    scoreSpeler1 = 0;
    scoreSpeler2 = 0;

    if (beurt % 2 == 0)
    {
        if (speler2HeeftZet == false)
        {
            status.ForeColor = donkereKleur1;
            status.Text = "Speler 1 aan zet";
            beurt++;
        }
    }
    if (beurt % 2 == 1)
    {
        if (speler1HeeftZet == false)
        {
            status.ForeColor = donkereKleur2;
            status.Text = "Speler 2 aan zet";
            beurt++;
        }
    }

    for (int x = 0; x < bordDimensie; x++)
    {
        for (int y = 0; y < bordDimensie; y++)
        {
            if (bordData[x, y, 2] == 1)
            {
                scoreSpeler1++;
                paintgr.FillEllipse(brush1, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                paintgr.FillEllipse(brushDark1, bordData[x, y, 0] + bordVakjeLengte / 6, bordData[x, y, 1] + bordVakjeLengte / 6, bordVakjeLengte / 7 * 5, bordVakjeLengte / 7 * 5);
            }
            if (bordData[x, y, 2] == 2)
            {
                scoreSpeler2++;
                paintgr.FillEllipse(brush2, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                paintgr.FillEllipse(brushDark2, bordData[x, y, 0] + bordVakjeLengte / 6, bordData[x, y, 1] + bordVakjeLengte / 6, bordVakjeLengte / 7 * 5, bordVakjeLengte / 7 * 5);
            }

            if (beurt % 2 == 0 && bordData[x, y, 4] == 1)
            {
                if (helpEnabled == true)
                {
                    paintgr.FillEllipse(brushLight2, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                }
            }
            if (beurt % 2 == 1 && bordData[x, y, 3] == 1)
            {
                if (helpEnabled == true)
                { 
                    paintgr.FillEllipse(brushLight1, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                }
            }
        }
    }

    aantalStenenKleur1.Text = $"{scoreSpeler1}";
    aantalStenenKleur2.Text = $"{scoreSpeler2}";

    paintgr.DrawEllipse(pen1, bordX, bordX, bordX / 2, bordX / 2);
    paintgr.DrawEllipse(pen2, 3 * bordX + bordX / 2, bordX, bordX / 2, bordX / 2);

    if (speler1HeeftZet == false && speler2HeeftZet == false)
    {
        eindeSpel();
    }
}

void bordGeklikt(object o, MouseEventArgs mea)
{
    if (spelBezig == true)
    {
        Point hier = mea.Location;
        int selectedVakNumHorizontaal = 0;
        while (selectedVakNumHorizontaal * bordVakjeLengte < hier.X - bordX)
        {
            selectedVakNumHorizontaal++;
        }
        selectedVakNumHorizontaal -= 1;
        int selectedVakNumVerticaal = 0;
        while (selectedVakNumVerticaal * bordVakjeLengte < hier.Y - bordY)
        {
            selectedVakNumVerticaal++;
        }
        selectedVakNumVerticaal -= 1;
        if (selectedVakNumHorizontaal < 0 || selectedVakNumHorizontaal >= bordDimensie) ;
        else
        {
            if (selectedVakNumVerticaal < 0 || selectedVakNumVerticaal >= bordDimensie) ;
            else
            {
                if (bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 2] == 0)
                {
                    if (beurt % 2 == 1 && bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 3] == 1)
                    {
                        //update steenkleuren
                        bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 2] = 1;
                        updateOmliggendeStennen(selectedVakNumHorizontaal, selectedVakNumVerticaal, beurt);

                        //update beurt
                        status.ForeColor = donkereKleur2;
                        status.Text = "Speler 2 aan zet";
                        beurt++;
                        bordLabel.Invalidate();
                    }
                    if (beurt % 2 != 1 && bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 4] == 1)
                    {
                        //update steenkleuren
                        bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 2] = 2;
                        updateOmliggendeStennen(selectedVakNumHorizontaal, selectedVakNumVerticaal, beurt);

                        //update beurt
                        status.ForeColor = donkereKleur1;
                        status.Text = "Speler 1 aan zet";
                        beurt++;
                        bordLabel.Invalidate();
                    }
                    beurtTeller.Text = $"Beurt {beurt}";
                }
            }
        }
    }
}

void helpFunctie(object o, EventArgs ea)
{
    helpEnabled = !helpEnabled;
    bordLabel.Invalidate();
}

int press1 = 0;
int press2 = 0;
void kleurVeranderen1(object o, EventArgs ea)
{
    press1++;
    if (press1 > 10)
    {
        press1 = 0;
    }
    kleur1 = kleuren[press1];
    if (kleur1 == kleur2)
    {
        press1++;
        if (press1 > 10)
        {
            press1 = 0;
        }
        kleur1 = kleuren[press1];
    }
    lichteKleur1 = ControlPaint.LightLight(kleur1);
    donkereKleur1 = ControlPaint.Dark(kleur1);

    brush1 = new SolidBrush(kleur1);
    brushLight1 = new SolidBrush(lichteKleur1);
    brushDark1 = new SolidBrush(donkereKleur1);
    pen1 = new Pen(brush1, 10);

    kleurKnop1.BackColor = kleur1;
    aantalStenenKleur1.ForeColor = kleur1;
    nieuwspel.BackColor = lichteKleur1;
    if (beurt % 2 == 1)
    {
        status.ForeColor = donkereKleur1;
    }
    bordLabel.Invalidate();
    scherm.Invalidate();
}
void kleurVeranderen2(object o, EventArgs ea)
{
    press2++;
    if (press2 > 10)
    {
        press2 = 0;
    }
    kleur2 = kleuren[press2];
    if (kleur2 == kleur1)
    {
        press2++;
        if (press2 > 10)
        {
            press2 = 0;
        }
        kleur2 = kleuren[press2];
    }
    lichteKleur2 = ControlPaint.LightLight(kleur2);
    donkereKleur2 = ControlPaint.Dark(kleur2);

    brush2 = new SolidBrush(kleur2);
    brushLight2 = new SolidBrush(lichteKleur2);
    brushDark2 = new SolidBrush(donkereKleur2);
    pen2 = new Pen(brush2, 10);

    kleurKnop2.BackColor = kleur2;
    aantalStenenKleur2.ForeColor = kleur2;
    help.BackColor = lichteKleur2;
    if (beurt % 2 != 1)
    {
        status.ForeColor = donkereKleur2;
    }
    bordLabel.Invalidate();
    scherm.Invalidate();
}

bordLabel.MouseClick += bordGeklikt;
nieuwspel.Click += nieuwSpel;
help.Click += helpFunctie;
kleurKnop1.Click += kleurVeranderen1;
kleurKnop2.Click += kleurVeranderen2;
bordLabel.Paint += teken;

nieuwSpel(null, null);

Application.Run(scherm);