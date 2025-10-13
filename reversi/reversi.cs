using System;
using System.Drawing;
using System.Windows.Forms;

int beurt = 1;
int scoreRood = 2;
int scoreBlauw = 2;

int bordX = 100;
int bordY = 180;
int bordLengte = 300;
int bordDimensie = 6;
bool helpEnabled = false;
bool roodHeeftZet = false;
bool blauwHeeftZet = false;
bool spelBezig = true;

int bordVakjeLengte = bordLengte / bordDimensie;
Pen penZwart = new Pen(Color.Black, 3);

int[,,] bordData = new int[bordDimensie, bordDimensie, 5];

Form scherm = new Form();
scherm.Text = "Reversi";
scherm.BackColor = Color.FromArgb(100, 125, 65);
scherm.ClientSize = new Size(500, 500);

Font font = new Font("Times New Roman", 14, FontStyle.Bold);
Font stenenFont = new Font("Times New Roman", 12, FontStyle.Bold);
Size buttonSize = new Size(100, 30);
Size miniButtonSize = new Size(30, 30);
Size textSize = new Size(22, 20);

Button kleurKnop1 = new Button();
kleurKnop1.Location = new Point(40, 110);
kleurKnop1.Size = miniButtonSize;
scherm.Controls.Add(kleurKnop1);

Button kleurKnop2 = new Button();
kleurKnop2.Location = new Point(430, 110);
kleurKnop2.Size = miniButtonSize;
scherm.Controls.Add(kleurKnop2);

Color[] kleuren = new[]
{
    Color.FromArgb(255, 0, 0), Color.FromArgb(255, 255, 0), Color.FromArgb(0, 255, 0), Color.FromArgb(0, 255, 255), Color.FromArgb(0, 0, 255), Color.FromArgb(255, 0, 255)
};
int press1 = 0;
int press2 = 0;

Color rgbKleur1 = kleuren[0];
Color rgbKleur2 = kleuren[4];
kleurKnop1.BackColor = rgbKleur1;
kleurKnop2.BackColor = rgbKleur2;

Color lichteKleur1 = ControlPaint.LightLight(rgbKleur1);
Color donkereKleur1 = ControlPaint.Dark(rgbKleur1);

Color lichteKleur2 = ControlPaint.LightLight(rgbKleur2);
Color donkereKleur2 = ControlPaint.Dark(rgbKleur2);

Button nieuwspel = new Button();
nieuwspel.Location = new Point(75, 40);
nieuwspel.Size = buttonSize;
scherm.Controls.Add(nieuwspel);
nieuwspel.BackColor = lichteKleur1;
nieuwspel.Font = stenenFont;
nieuwspel.Text = "Nieuw spel";

Button help = new Button();
help.Location = new Point(325, 40);
help.Size = buttonSize;
scherm.Controls.Add(help);
help.BackColor = lichteKleur2;
help.Font = stenenFont;
help.Text = "Help";

Label aantalRood = new Label();
aantalRood.Location = new Point(115, 115);
aantalRood.Size = textSize;
aantalRood.Font = stenenFont;
aantalRood.TextAlign = ContentAlignment.MiddleCenter;
scherm.Controls.Add(aantalRood);
aantalRood.ForeColor = rgbKleur1;
aantalRood.Text = $"{scoreRood}";

Label aantalBlauw = new Label();
aantalBlauw.Location = new Point(364, 115);
aantalBlauw.Size = textSize;
aantalBlauw.Font = stenenFont;
scherm.Controls.Add(aantalBlauw);
aantalBlauw.TextAlign = ContentAlignment.MiddleCenter;
aantalBlauw.ForeColor = rgbKleur2;
aantalBlauw.Text = $"{scoreBlauw}";

Label status = new Label();
status.Location = new Point(200, 45);
status.Size = new Size(100, 60);
status.Font = font;
scherm.Controls.Add(status);
status.TextAlign = ContentAlignment.MiddleCenter;

ComboBox bordSelectie = new ComboBox();
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

Bitmap bitmapBord = new Bitmap(500, 500);
Label bordLabel = new Label();
scherm.Controls.Add(bordLabel);
bordLabel.Location = new Point(0, 0);
bordLabel.Size = new Size(500, 500);
bordLabel.Image = bitmapBord;
Graphics bitgr = Graphics.FromImage(bitmapBord);

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
    status.ForeColor = donkereKleur1;
    status.Text = "Speler 1 aan zet";
    scoreRood = 2;
    scoreBlauw = 2;
    aantalRood.Text = $"{scoreRood}";
    aantalBlauw.Text = $"{scoreBlauw}";
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
                                            bordData[x, y, 3] = 1;
                                            roodHeeftZet = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            catch { }
                            //de plaatsbare tiles voor blauw in de bordData array zetten
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
                                            bordData[x, y, 4] = 1;
                                            blauwHeeftZet = true;
                                            break;
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
                //de omliggende tiles voor rood updaten in borddata array
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
                else //de omliggende tiles voor blauw updaten in borddata array
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
    if (scoreRood > scoreBlauw)
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

    if (beurt % 2 == 0)
    {
        if (blauwHeeftZet == false)
        {
            status.ForeColor = donkereKleur1;
            status.Text = "Speler 1 aan zet";
            beurt++;
            bordLabel.Invalidate();
        }
    }
    if (beurt % 2 == 1)
    {
        if (roodHeeftZet == false)
        {
            status.ForeColor = donkereKleur2;
            status.Text = "Speler 2 aan zet";
            beurt++;
            bordLabel.Invalidate();
        }
    }

    for (int x = 0; x < bordDimensie; x++)
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
                if (helpEnabled == true)
                {
                    paintgr.FillEllipse(rgbBrushLight2, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                }
            }
            if (beurt % 2 == 1 && bordData[x, y, 3] == 1)
            {
                if (helpEnabled == true)
                { 
                    paintgr.FillEllipse(rgbBrushLight1, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                }
            }
        }
    }

    aantalRood.Text = $"{scoreRood}";
    aantalBlauw.Text = $"{scoreBlauw}";
    paintgr.DrawEllipse(penRood, bordX, bordX, bordX / 2, bordX / 2);
    paintgr.DrawEllipse(penBlauw, 3 * bordX + bordX / 2, bordX, bordX / 2, bordX / 2);

    if (roodHeeftZet == false && blauwHeeftZet == false)
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
                        //upate tile colors
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
            }
        }
    }
}

void helpFunctie(object o, EventArgs ea)
{
    helpEnabled = !helpEnabled;
    bordLabel.Invalidate();
}

void kleurVeranderen1(object o, EventArgs ea)
{
    press1++;
    if (press1 > 5)
    {
        press1 = 0;
    }
    rgbKleur1 = kleuren[press1];
    if (rgbKleur1 == rgbKleur2)
    {
        press1++;
        if (press1 > 5)
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
        bordLabel.Invalidate();
        scherm.Invalidate();
    }
}
void kleurVeranderen2(object o, EventArgs ea)
{
    press2++;
    if (press2 > 5)
    {
        press2 = 0;
    }
    rgbKleur2 = kleuren[press2];

    if (rgbKleur2 == rgbKleur1)
    {
        press2++;
        if (press2 > 5)
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
        bordLabel.Invalidate();
        scherm.Invalidate();
    }
}

bordLabel.MouseClick += bordGeklikt;
nieuwspel.Click += nieuwSpel;
help.Click += helpFunctie;
kleurKnop1.Click += kleurVeranderen1;
kleurKnop2.Click += kleurVeranderen2;
bordLabel.Paint += teken;

nieuwSpel(null, null);

Application.Run(scherm);