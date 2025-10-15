using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

//variabelen aanmaken
int beurt = 1;
int scoreRood = 2;
int scoreBlauw = 2;

int bordX = 100;
int bordY = 180;
int bordLengte = 300;
int bordDimensie = 6;
bool helpIngeschakeld = false;
bool roodHeeftZet = false;
bool blauwHeeftZet = false;
bool spelBezig = true;

int bordVakjeLengte = bordLengte / bordDimensie;
Pen penZwart = new Pen(Color.Black, 3);

//3D array voor het bord maken
int[,,] bordData = new int[bordDimensie, bordDimensie, 5];

//form aanmaken
Form scherm = new Form();
scherm.Text = "Reversi";
scherm.BackColor = Color.FromArgb(100, 125, 65);
scherm.ClientSize = new Size(500, 500);

//fonts en sizes aanmaken
Font statusFont = new Font("Times New Roman", 14, FontStyle.Bold);
Font knopFont = new Font("Times New Roman", 12, FontStyle.Bold);
Size buttonSize = new Size(100, 30);
Size miniButtonSize = new Size(30, 30);
Size textSize = new Size(22, 20);

//knop voor kleur 1 aanpassen aanmaken
Button kleurKnop1 = new Button();
kleurKnop1.Location = new Point(40, 110);
kleurKnop1.Size = miniButtonSize;
scherm.Controls.Add(kleurKnop1);

//knop voor kleur 2 aanpassen aanmaken
Button kleurKnop2 = new Button();
kleurKnop2.Location = new Point(430, 110);
kleurKnop2.Size = miniButtonSize;
scherm.Controls.Add(kleurKnop2);

//lijst met mogelijke kleuren voor de stenen
Color[] kleuren = new[]
{
    Color.Red, Color.DarkOrange, Color.Yellow, Color.YellowGreen, Color.DarkGreen, Color.Cyan, Color.Blue, Color.BlueViolet, Color.Magenta, Color.DimGray, Color.FromArgb(45, 45, 45)
};
int press1 = 0;
int press2 = 0;

//begin kleuren van de stenen zetten
Color rgbKleur1 = kleuren[0];
Color rgbKleur2 = kleuren[6];
kleurKnop1.BackColor = rgbKleur1;
kleurKnop2.BackColor = rgbKleur2;

Color lichteKleur1 = ControlPaint.LightLight(rgbKleur1);
Color donkereKleur1 = ControlPaint.Dark(rgbKleur1);

Color lichteKleur2 = ControlPaint.LightLight(rgbKleur2);
Color donkereKleur2 = ControlPaint.Dark(rgbKleur2);

//nieuwspel knop aanmaken
Button nieuwspel = new Button();
nieuwspel.Location = new Point(75, 40);
nieuwspel.Size = buttonSize;
scherm.Controls.Add(nieuwspel);
nieuwspel.BackColor = lichteKleur1;
nieuwspel.Font = knopFont;
nieuwspel.Text = "Nieuw spel";

//help knop aanmaken
Button help = new Button();
help.Location = new Point(325, 40);
help.Size = buttonSize;
scherm.Controls.Add(help);
help.BackColor = lichteKleur2;
help.Font = knopFont;
help.Text = "Help";

//label voor score van rood aanmaken
Label aantalRood = new Label();
aantalRood.Location = new Point(115, 115);
aantalRood.Size = textSize;
aantalRood.Font = knopFont;
aantalRood.TextAlign = ContentAlignment.MiddleCenter;
scherm.Controls.Add(aantalRood);
aantalRood.ForeColor = rgbKleur1;
aantalRood.Text = $"{scoreRood}";

//label voor score van blauw aanmaken
Label aantalBlauw = new Label();
aantalBlauw.Location = new Point(364, 115);
aantalBlauw.Size = textSize;
aantalBlauw.Font = knopFont;
scherm.Controls.Add(aantalBlauw);
aantalBlauw.TextAlign = ContentAlignment.MiddleCenter;
aantalBlauw.ForeColor = rgbKleur2;
aantalBlauw.Text = $"{scoreBlauw}";

//tekstlabel voor de status van het spel aanmaken
Label status = new Label();
status.Location = new Point(200, 45);
status.Size = new Size(100, 60);
status.Font = statusFont;
scherm.Controls.Add(status);
status.TextAlign = ContentAlignment.MiddleCenter;

//label voor de beurtenteller tekst aanmaken
Label beurtTeller = new Label();
beurtTeller.Location = new Point(200, 15);
beurtTeller.Size = buttonSize;
beurtTeller.Font = knopFont;
beurtTeller.TextAlign = ContentAlignment.MiddleCenter;
scherm.Controls.Add(beurtTeller);
beurtTeller.ForeColor = Color.Black;
beurtTeller.Text = $"Beurt {beurt}";

//combobox voor bordgrootte maken
ComboBox bordSelectie = new ComboBox();
bordSelectie.Location = new Point(200, 120);
bordSelectie.Size = new Size(100, 30);
scherm.Controls.Add(bordSelectie);
bordSelectie.BackColor = Color.FromArgb(255, 255, 255);
bordSelectie.Font = statusFont;
bordSelectie.Text = "6x6";
bordSelectie.Items.Add("4x4");
bordSelectie.Items.Add("6x6");
bordSelectie.Items.Add("8x8");
bordSelectie.Items.Add("10x10");
bordSelectie.Font = statusFont;

//bitmap van het bord maken
Bitmap bitmapBord = new Bitmap(500, 500);
Label bordLabel = new Label();
scherm.Controls.Add(bordLabel);
bordLabel.Location = new Point(0, 0);
bordLabel.Size = new Size(500, 500);
bordLabel.Image = bitmapBord;
Graphics bitgr = Graphics.FromImage(bitmapBord);

void nieuwSpel(object o, EventArgs ea)
{
    if (bordSelectie.SelectedItem == "4x4") //bordgrootte bepalen
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
    beurt = 1; //variabelen resetten
    beurtTeller.Text = $"Beurt {beurt}";
    status.ForeColor = donkereKleur1;
    status.Text = "Speler 1 aan zet";
    scoreRood = 2;
    scoreBlauw = 2;
    aantalRood.Text = $"{scoreRood}";
    aantalBlauw.Text = $"{scoreBlauw}";
    bordLabel.Invalidate();
    spelBezig = true;

    bordVakjeLengte = bordLengte / bordDimensie;

    bitgr.FillRectangle(Brushes.WhiteSmoke, bordX, bordY, bordLengte, bordLengte); //bord wit maken

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

Brush rgbBrush1 = new SolidBrush(rgbKleur1);
Brush rgbBrush2 = new SolidBrush(rgbKleur2);
Brush rgbBrushLight1 = new SolidBrush(lichteKleur1);
Brush rgbBrushLight2 = new SolidBrush(lichteKleur2);
Brush rgbBrushDark1 = new SolidBrush(donkereKleur1);
Brush rgbBrushDark2 = new SolidBrush(donkereKleur2);
Pen penRood = new Pen(rgbBrush1, 10);
Pen penBlauw = new Pen(rgbBrush2, 10);

void kanPlaatsenOp()
{
    blauwHeeftZet = false;
    roodHeeftZet = false;
    for (int x = 0; x < bordDimensie; x++)//ieder vakje langs gaan
    {
        for (int y = 0; y < bordDimensie; y++)
        {
            bordData[x, y, 3] = 0;
            bordData[x, y, 4] = 0;
            if (bordData[x,y,2]==0)
            {
                for (int xOmliggend = -1; xOmliggend <= 1; xOmliggend++) //iedere richting voor dat vakje langs gaan
                {
                    for (int yOmliggend = -1; yOmliggend <= 1; yOmliggend++)
                    {
                        if (xOmliggend != 0 || yOmliggend != 0)//niet de tile zelf checken
                        {
                            //de plaatsbare tiles voor rood in de bordData array zetten
                            if (x + xOmliggend >= 0 && x + xOmliggend < bordDimensie && y + yOmliggend >= 0 && y + yOmliggend < bordDimensie)
                            {
                                if (bordData[x + xOmliggend, y + yOmliggend, 2] == 2)
                                {
                                    int xOffset = xOmliggend;
                                    int yOffset = yOmliggend;

                                    while (bordData[x + xOffset, y + yOffset, 2] == 2)
                                    {
                                        if (x + xOmliggend + xOffset >= 0 && x + xOmliggend + xOffset < bordDimensie && y + yOmliggend + yOffset >= 0 && y + yOmliggend + yOffset < bordDimensie)
                                        {
                                            xOffset += xOmliggend;
                                            yOffset += yOmliggend;
                                        }
                                        else
                                        {
                                            break;
                                        }

                                        if (bordData[x + xOffset, y + yOffset, 2] == 1) //plaatsbaar
                                        {
                                            bordData[x, y, 3] = 1;
                                            roodHeeftZet = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            //de plaatsbare tiles voor blauw in de bordData array zetten
                            if (x + xOmliggend >= 0 && x + xOmliggend < bordDimensie && y + yOmliggend >= 0 && y + yOmliggend < bordDimensie)
                            {
                                if (bordData[x + xOmliggend, y + yOmliggend, 2] == 1)
                                {
                                    int xOffset = xOmliggend;
                                    int yOffset = yOmliggend;

                                    while (bordData[x + xOffset, y + yOffset, 2] == 1)
                                    {
                                        if (x + xOmliggend + xOffset >= 0 && x + xOmliggend + xOffset < bordDimensie && y + yOmliggend + yOffset >= 0 && y + yOmliggend + yOffset < bordDimensie)
                                        {
                                            xOffset += xOmliggend;
                                            yOffset += yOmliggend;
                                        }
                                        else
                                        {
                                            break;
                                        }

                                        if (bordData[x + xOffset, y + yOffset, 2] == 2) //plaatsbaar
                                        {
                                            bordData[x, y, 4] = 1;
                                            blauwHeeftZet = true;
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
    for (int xOmliggend = -1; xOmliggend <= 1; xOmliggend++) //door alle omliggende vakjes loopen
    {
        for (int yOmliggend = -1; yOmliggend <= 1; yOmliggend++)
        {
            if (xOmliggend != 0 || yOmliggend != 0) //niet de tile zelf checken
            {
                //de tile in directie xomliggen en yomliggend updaten
                if (beurt % 2 == 1)
                {
                    try
                    {
                        if (bordData[x + xOmliggend, y + yOmliggend, 2] == 2)
                        {
                            int xOffset = xOmliggend;
                            int yOffset = yOmliggend;
                            while (bordData[x + xOffset, y + yOffset, 2] == 2)
                            {
                                xOffset += xOmliggend;
                                yOffset += yOmliggend;
                                if (bordData[x + xOffset, y + yOffset, 2] == 1)
                                {
                                    xOffset = xOmliggend;
                                    yOffset = yOmliggend;
                                    while (bordData[x + xOffset, y + yOffset, 2] == 2)
                                    {
                                        bordData[x + xOffset, y + yOffset, 2] = 1;
                                        xOffset += xOmliggend;
                                        yOffset += yOmliggend;
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
                else //de omliggende tiles voor blauw updaten in borddata array
                {
                    try
                    {
                        if (bordData[x + xOmliggend, y + yOmliggend, 2] == 1)
                        {
                            int xOffset = xOmliggend;
                            int yOffset = yOmliggend;

                            while (bordData[x + xOffset, y + yOffset, 2] == 1)
                            {
                                xOffset += xOmliggend;
                                yOffset += yOmliggend;
                                if (bordData[x + xOffset, y + yOffset, 2] == 2)
                                {
                                    xOffset = xOmliggend;
                                    yOffset = yOmliggend;

                                    while (bordData[x + xOffset, y + yOffset, 2] == 1)
                                    {
                                        bordData[x + xOffset, y + yOffset, 2] = 2;
                                        xOffset += xOmliggend;
                                        yOffset += yOmliggend;
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
    if (scoreRood > scoreBlauw) //checken wie er gewonnen heeft
    {
        status.ForeColor = donkereKleur1;
        status.Text = "Speler 1 heeft gewonnen!";
        bordLabel.Invalidate();
    }
    else
    {
    if (scoreRood < scoreBlauw)
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
    scoreRood = 0;
    scoreBlauw = 0;

    if (beurt % 2 == 0) //checken of de spelers een zet hebben
    {
        if (blauwHeeftZet == false)
        {
            status.ForeColor = donkereKleur1;
            status.Text = "Speler 1 aan zet";
            beurt++;
        }
    }
    if (beurt % 2 == 1)
    {
        if (roodHeeftZet == false)
        {
            status.ForeColor = donkereKleur2;
            status.Text = "Speler 2 aan zet";
            beurt++;
        }
    }

    for (int x = 0; x < bordDimensie; x++) //stenen tekenen en score berekenen
    {
        for (int y = 0; y < bordDimensie; y++)
        {
            if (bordData[x, y, 2] == 1)
            {
                scoreRood++;
                paintgr.FillEllipse(rgbBrush1, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                paintgr.FillEllipse(rgbBrushDark1, bordData[x, y, 0] + bordVakjeLengte / 6, bordData[x, y, 1] + bordVakjeLengte / 6, bordVakjeLengte / 7 * 5, bordVakjeLengte / 7 * 5);
            }
            if (bordData[x, y, 2] == 2)
            {
                scoreBlauw++;
                paintgr.FillEllipse(rgbBrush2, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                paintgr.FillEllipse(rgbBrushDark2, bordData[x, y, 0] + bordVakjeLengte / 6, bordData[x, y, 1] + bordVakjeLengte / 6, bordVakjeLengte / 7 * 5, bordVakjeLengte / 7 * 5);
            }

            if (beurt % 2 == 0 && bordData[x, y, 4] == 1)
            {
                if (helpIngeschakeld == true)
                {
                    paintgr.FillEllipse(rgbBrushLight2, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                }
            }
            if (beurt % 2 == 1 && bordData[x, y, 3] == 1)
            {
                if (helpIngeschakeld == true)
                { 
                    paintgr.FillEllipse(rgbBrushLight1, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                }
            }
        }
    }

    aantalRood.Text = $"{scoreRood}"; //score tekenen
    aantalBlauw.Text = $"{scoreBlauw}";
    paintgr.DrawEllipse(penRood, bordX, bordX, bordX / 2, bordX / 2);
    paintgr.DrawEllipse(penBlauw, 3 * bordX + bordX / 2, bordX, bordX / 2, bordX / 2);

    if (roodHeeftZet == false && blauwHeeftZet == false) //checken of het spel afgelopen is
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
        while (selectedVakNumHorizontaal * bordVakjeLengte < hier.X - bordX) //selectedVakNumHorizontaal en Verticaal bepalen
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
        if (selectedVakNumHorizontaal < 0 || selectedVakNumHorizontaal >= bordDimensie) ; //checken of er binnen het bord is geklikt
        else
        {
            if (selectedVakNumVerticaal < 0 || selectedVakNumVerticaal >= bordDimensie) ;
            else
            {
                if (bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 2] == 0)
                {
                    //check wiens beurt het is en of diegene op dat vakje mag plaatsen
                    if (beurt % 2 == 1)
                    {
                        if (bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 3] == 1)
                        {
                            //upate tile colors
                            bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 2] = 1;
                            updateOmliggendeStennen(selectedVakNumHorizontaal, selectedVakNumVerticaal, beurt);

                            //update beurt
                            status.ForeColor = donkereKleur2;
                            status.Text = "Speler 2 aan zet";
                            beurt++;
                            bordLabel.Invalidate();
                        }
                    }
                    else
                    {
                        if (bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 4] == 1)
                        {
                            //upate tile colors
                            bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 2] = 2;
                            updateOmliggendeStennen(selectedVakNumHorizontaal, selectedVakNumVerticaal, beurt);

                            //update beurt
                            status.ForeColor = donkereKleur1;
                            status.Text = "Speler 1 aan zet";
                            beurt++;
                            bordLabel.Invalidate();
                        }
                    }
                    beurtTeller.Text = $"Beurt {beurt}";
                }
            }
        }
    }
}

void helpGeklikt(object o, EventArgs ea)
{
    helpIngeschakeld = !helpIngeschakeld;
    bordLabel.Invalidate();
}

void kleurVeranderen1(object o, EventArgs ea)
{
    press1++;
    if (press1 > 10)
    {
        press1 = 0;
    }
    rgbKleur1 = kleuren[press1];
    if (rgbKleur1 == rgbKleur2)
    {
        press1++;
        if (press1 > 10)
        {
            press1 = 0;
        }
        rgbKleur1 = kleuren[press1];
    }
    lichteKleur1 = ControlPaint.LightLight(rgbKleur1);
    donkereKleur1 = ControlPaint.Dark(rgbKleur1);

    rgbBrush1 = new SolidBrush(rgbKleur1);
    rgbBrushLight1 = new SolidBrush(lichteKleur1);
    rgbBrushDark1 = new SolidBrush(donkereKleur1);
    penRood = new Pen(rgbBrush1, 10);

    kleurKnop1.BackColor = rgbKleur1;
    aantalRood.ForeColor = rgbKleur1;
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
    rgbKleur2 = kleuren[press2];

    if (rgbKleur2 == rgbKleur1)
    {
        press2++;
        if (press2 > 10)
        {
            press2 = 0;
        }
        rgbKleur2 = kleuren[press2];
    }
    lichteKleur2 = ControlPaint.LightLight(rgbKleur2);
    donkereKleur2 = ControlPaint.Dark(rgbKleur2);

    rgbBrush2 = new SolidBrush(rgbKleur2);
    rgbBrushLight2 = new SolidBrush(lichteKleur2);
    rgbBrushDark2 = new SolidBrush(donkereKleur2);
    penBlauw = new Pen(rgbBrush2, 10);

    kleurKnop2.BackColor = rgbKleur2;
    aantalBlauw.ForeColor = rgbKleur2;
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
help.Click += helpGeklikt;
kleurKnop1.Click += kleurVeranderen1;
kleurKnop2.Click += kleurVeranderen2;
bordLabel.Paint += teken;

nieuwSpel(null, null);

Application.Run(scherm);