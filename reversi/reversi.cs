using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "reversi";
scherm.BackColor = Color.Moccasin;
scherm.ClientSize = new Size(500, 500);

Font font = new Font("Times New Roman", 11, FontStyle.Bold);
Font stenenFont = new Font("Times New Roman", 12, FontStyle.Bold);
Size buttonSize = new Size(100, 30);
Size textSize = new Size(22, 20);

Button nieuwspel = new Button();
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

int beurt = 1;
int scoreRood = 2;
int scoreBlauw = 2;

Label aantalRood = new Label();
aantalRood.Location = new Point(115, 115);
aantalRood.Size = textSize;
aantalRood.Font = stenenFont;
scherm.Controls.Add(aantalRood);
aantalRood.ForeColor = Color.Red;
aantalRood.Text = $"{scoreRood}";

Label aantalBlauw = new Label();
aantalBlauw.Location = new Point(365, 115);
aantalBlauw.Size = textSize;
aantalBlauw.Font = stenenFont;
scherm.Controls.Add(aantalBlauw);
aantalBlauw.ForeColor = Color.Blue;
aantalBlauw.Text = $"{scoreBlauw}";

Label status = new Label();
status.Location = new Point(203, 45);
status.Size = new Size(100, 60);
status.Font = font;
scherm.Controls.Add(status);

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
void niewSpel(object o, EventArgs ea)
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
    status.Location = new Point(203, 45);
    status.ForeColor = Color.DarkRed;
    status.Text = "Rood aan zet";
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

Pen penRood = new Pen(Brushes.Red, 10);
Pen penBlauw = new Pen(Brushes.Blue, 10);
bitgr.DrawEllipse(penRood, bordX, bordX, bordX / 2, bordX / 2);
bitgr.DrawEllipse(penBlauw, 3 * bordX + bordX / 2, bordX, bordX / 2, bordX / 2);

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
                for (int xSurounding = -1; xSurounding <= 1; xSurounding++)
                {
                    for (int ySurounding = -1; ySurounding <= 1; ySurounding++)
                    {
                        if (xSurounding != 0 || ySurounding != 0)
                        {
                            //de plaatsbare tiles voor rood in de bordData array zetten
                            try
                            {
                                if (bordData[x + xSurounding, y + ySurounding, 2] == 2)
                                {
                                    int xOfset = xSurounding;
                                    int yOfset = ySurounding;

                                    while (bordData[x + xOfset, y + yOfset, 2] == 2)
                                    {
                                        xOfset += xSurounding;
                                        yOfset += ySurounding;
                                        if (bordData[x + xOfset, y + yOfset, 2] == 1)
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
                                if (bordData[x + xSurounding, y + ySurounding, 2] == 1)
                                {
                                    int xOfset = xSurounding;
                                    int yOfset = ySurounding;

                                    while (bordData[x + xOfset, y + yOfset, 2] == 1)
                                    {
                                        xOfset += xSurounding;
                                        yOfset += ySurounding;
                                        if (bordData[x + xOfset, y + yOfset, 2] == 2)
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
    for (int xSurounding = -1; xSurounding <= 1; xSurounding++)
    {
        for (int ySurounding = -1; ySurounding <= 1; ySurounding++)
        {
            if (xSurounding != 0 || ySurounding != 0)
            {
                //de omliggende tiles voor rood updaten in borddata array
                if (beurt % 2 == 1)
                {
                    try
                    {
                        if (bordData[x + xSurounding, y + ySurounding, 2] == 2)
                        {
                            int xOfset = xSurounding;
                            int yOfset = ySurounding;
                            while (bordData[x + xOfset, y + yOfset, 2] == 2)
                            {
                                xOfset += xSurounding;
                                yOfset += ySurounding;
                                if (bordData[x + xOfset, y + yOfset, 2] == 1)
                                {
                                    xOfset = xSurounding;
                                    yOfset = ySurounding;
                                    while (bordData[x + xOfset, y + yOfset, 2] == 2)
                                    {
                                        bordData[x + xOfset, y + yOfset, 2] = 1;
                                        xOfset += xSurounding;
                                        yOfset += ySurounding;
                                        if (bordData[x + xOfset, y + yOfset, 2] == 1)
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
                        if (bordData[x + xSurounding, y + ySurounding, 2] == 1)
                        {
                            int xOfset = xSurounding;
                            int yOfset = ySurounding;

                            while (bordData[x + xOfset, y + yOfset, 2] == 1)
                            {
                                xOfset += xSurounding;
                                yOfset += ySurounding;
                                if (bordData[x + xOfset, y + yOfset, 2] == 2)
                                {
                                    xOfset = xSurounding;
                                    yOfset = ySurounding;

                                    while (bordData[x + xOfset, y + yOfset, 2] == 1)
                                    {
                                        bordData[x + xOfset, y + yOfset, 2] = 2;
                                        xOfset += xSurounding;
                                        yOfset += ySurounding;
                                        if (bordData[x + xOfset, y + yOfset, 2] == 2)
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
        status.ForeColor = Color.DarkRed;
        status.Text = "Rood heeft gewonnen!";
        bordLabel.Invalidate();
    }
    else
    {
    if (scoreRood < scoreBlauw)
        {
            status.ForeColor = Color.DarkBlue;
            status.Text = "Blauw heeft gewonnen!";
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
            status.ForeColor = Color.DarkRed;
            status.Text = "Rood aan zet";
            beurt++;
            bordLabel.Invalidate();
        }
    }
    if (beurt % 2 == 1)
    {
        if (roodHeeftZet == false)
        {
            status.ForeColor = Color.DarkBlue;
            status.Text = "Blauw aan zet";
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
                paintgr.FillEllipse(Brushes.Red, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
            }
            if (bordData[x, y, 2] == 2)
            {
                scoreBlauw++;
                paintgr.FillEllipse(Brushes.Blue, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
            }

            if (beurt % 2 == 0 && bordData[x, y, 4] == 1)
            {
                if (helpEnabled == true)
                {
                    paintgr.FillEllipse(Brushes.LightBlue, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                }
            }
            if (beurt % 2 == 1 && bordData[x, y, 3] == 1)
            {
                if (helpEnabled == true)
                { 
                    paintgr.FillEllipse(Brushes.LightSalmon, bordData[x, y, 0], bordData[x, y, 1], bordVakjeLengte, bordVakjeLengte);
                }
            }
        }
    }

    aantalRood.Text = $"{scoreRood}";
    aantalBlauw.Text = $"{scoreBlauw}";

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
                        status.ForeColor = Color.DarkBlue;
                        status.Text = "Blauw aan zet";
                        beurt++;
                        bordLabel.Invalidate();
                    }
                    if (beurt % 2 != 1 && bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 4] == 1)
                    {
                        //upate tile colors
                        bordData[selectedVakNumHorizontaal, selectedVakNumVerticaal, 2] = 2;
                        updateOmliggendeStennen(selectedVakNumHorizontaal, selectedVakNumVerticaal, beurt);

                        //update beurt
                        status.ForeColor = Color.DarkRed;
                        status.Text = "Rood aan zet";
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


bordLabel.MouseClick += bordGeklikt;
nieuwspel.Click += niewSpel;
help.Click += helpFunctie;
bordLabel.Paint += teken;

niewSpel(null, null);

Application.Run(scherm);