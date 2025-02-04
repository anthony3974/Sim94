using Fs94Values;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Timer = System.Windows.Forms.Timer;

namespace Sim
{
    public enum Visible { Show, Hide, }
    public partial class Sim94 : Form
    {
        #region controls
        readonly string[] outC = { "w = move up", "a = move left", "s = move down", "d = move right", "\u2191 = gas", "\u2190 = turn left", "\u2193 = reverse", "\u2192 = turn right", "c = show controls", "g = open garage", "i = interact", "f = fertilize", "h = harvest", "b = buy", "v = sell", "j = plant wheat", "k = plant barley", "l = plant oats", "1 = tractor", "2 = van", "3 = harvester", "i = save" };
        #endregion
        #region class imports
        readonly Dictionary<string, ImgMover> dictBg = new Dictionary<string, ImgMover>();
        readonly List<string> dictBgVKey = new List<string>(); // <^ these are for the fields and icons
        readonly Dictionary<string, ImgMover> dictText = new Dictionary<string, ImgMover>();
        readonly List<string> dictTextVKey = new List<string>(); // <^ these are for the text
        readonly Dictionary<string, FieldMetadata> dictFieldsData = new Dictionary<string, FieldMetadata>();
        readonly List<string> dictFieldsDataVKey = new List<string>(); // <^ these are for the fields data
        readonly Dictionary<string, Timer> dictTimer = new Dictionary<string, Timer>();
        readonly List<string> dictTimerVKey = new List<string>(); // <^ these are for the field timer events
        readonly Dictionary<string, BuildingMetadata> dictBuildingData = new Dictionary<string, BuildingMetadata>();
        readonly List<string> dictBuildingDataVKey = new List<string>(); // <^ these are for the building data
        #endregion
        #region this is the main player values
        readonly ImgMover[] Player = new ImgMover[3];
        readonly Image[,,] VAkal = new Image[3, 4, 3];
        int ttruck = 0;
        readonly int[] tcolor = new int[] { 0, 0, 0 };
        int tspeed = 6;
        const int TRUCK_SPEED = 8, VAN_SPEED = 10, HARVESTER_SPEED = 6;
        #endregion
        #region global vars
        Point s = new Point(-800, -535); // the starting point of the map for refrence
        string address = "H", isIn = null, waitForSecondKey = null, importantMess = null;
        bool start = false, showControls = false, garageClosed = false;
        int angle = 180;
        readonly Dictionary<string, int> resources = new Dictionary<string, int>();
        readonly string[] OthersNames = new string[] { "Benjamin", "Bernd", "Daniel", "Florian", "Jonas", "Leonie", "Lucas", "Maik", "Paul", "Reinhold", "Robert", "Sophie", };
        readonly Random rn = new Random(DateTime.Now.Millisecond);
        Panel painting;
        readonly Dictionary<string, RadioButton> rad1 = new Dictionary<string, RadioButton>();
        PictureBox picColor;
        Panel dealing;
        readonly Dictionary<string, RadioButton> rad2 = new Dictionary<string, RadioButton>();
        PictureBox picColor1;
        bool[] carOwned = new bool[] { true, false, false };
        bool loaded = false;
        #endregion
        #region main constructor and some vars
        public Sim94()
        {
            InitializeComponent();
            // set resources 
            resources.Add("Dollars", 5000); resources.Add("Diesel", 100); resources.Add("Seeds", 2000); resources.Add("Wheat", 1000); resources.Add("Barley", 1000); resources.Add("Oats", 1000); resources.Add("Compost", 1000);
            #region load classes
            // set string values for acending order then set Bg dict then set values in the list of values class
            {
                string name = "bg";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover($"{Directory.GetCurrentDirectory()}/backround.png", x: s.X, y: s.Y, widthPer: 0.55f, heightPer: 0.55f));
            }
            {
                string name = "garage";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover($"{Directory.GetCurrentDirectory()}/garage.png", 218, 210, 89, 1));
            }
            {
                string name = "address";
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, address, font[0], new Rectangle(83, 190, 18, 23)));
            }
            {
                string name = "19";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new RectangleF(10, -214, 607, 292), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "19", font[1], new RectangleF(320, -90, 18 * name.Length, 23)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, "You", 3.35f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Dahl Ranch";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new RectangleF(9, -408, 872, 84), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Dahl Ranch", font[1], new RectangleF(370, -380, 18 * name.Length, 23)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 1.410f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Wheat Selling Station";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(Color.Transparent, new RectangleF(925, -410, 188, 144), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Wheat \nSelling \nStation", font[0], new RectangleF(944, -343, 18 * name.Length, 20 * 3)));
                dictBuildingDataVKey.Add(name);
                dictBuildingData.Add(name, new BuildingMetadata(name, "Selling", "Wheat"));
            }
            {
                string name = "Barley Selling Station";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(Color.Transparent, new RectangleF(920, 890, 192, 130), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Barley \nSelling \nStation", font[0], new RectangleF(936, 939, 18 * name.Length, 20 * 3)));
                dictBuildingDataVKey.Add(name);
                dictBuildingData.Add(name, new BuildingMetadata(name, "Selling", "Barley"));
            }
            {
                string name = "Sids Seed Shop";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(Color.Transparent, new RectangleF(919, 690, 98, 160), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Sids \nSeed \nShop", font[0], new RectangleF(936, 708, 18 * name.Length, 20 * 3)));
                dictBuildingDataVKey.Add(name);
                dictBuildingData.Add(name, new BuildingMetadata(name, "Buying", "Seeds"));
            }
            {
                string name = "Alpine Farming";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new RectangleF(685, -43, 333, 307), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Alpine \nFarming", font[1], new RectangleF(790, 90, 18 * name.Length, 25 * 2)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 1.92f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "John Deer";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new RectangleF(7, 475, 556, 254), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "John Deer", font[1], new RectangleF(200, 590, 18 * name.Length, 25)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 2.68f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Erlengrat";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new RectangleF(622, 476, 385, 184), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Erlengrat", font[1], new RectangleF(745, 540, 18 * name.Length, 25)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 1.34f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Welker Farms";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new PointF[] { new Point(0, 0), new Point(0, 309), new Point(272, 333), new Point(272, 0) }, new RectangleF(622, 694, 272, 333), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Welker Farms", font[1], new RectangleF(665, 825, 18 * name.Length, 25)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 1.65f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Goldcrest Valley";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new PointF[] { new Point(4, -2), new Point(553, 157), new Point(553, 0) }, new RectangleF(10, 762, 553, 157), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Goldcrest Valley", font[1], new RectangleF(285, 780, 18 * name.Length, 25)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 0.80f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Lone Oak";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new PointF[] { new Point(0, 0), new Point(0, 158), new Point(540, 158) }, new RectangleF(6, 938, 550, 158), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Lone Oak", font[1], new RectangleF(65, 1029, 18 * name.Length, 25)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 0.80f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Estancia Lapacho";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new RectangleF(-352, 793, 167, 292), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Estancia \nLapacho", font[1], new RectangleF(-336, 895, 18 * name.Length, 25 * 2)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 0.92f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Felsbrunn";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new RectangleF(-764, 901, 371, 184), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Felsbrunn", font[1], new RectangleF(-640, 970, 18 * name.Length, 25)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 1.30f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Minibrunn";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new PointF[] { new Point(0, 0), new Point(0, 334), new Point(175, 334), new Point(175, 69) }, new RectangleF(-777, 235, 175, 334), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Minibrunn", font[1], new RectangleF(-760, 400, 18 * name.Length, 25)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 1.00f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Oat Selling Station";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(Color.Transparent, new RectangleF(-776, 595, 190, 130), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Oat \nSelling \nStation", font[0], new RectangleF(-760, 620, 18 * name.Length, 20 * 3)));
                dictBuildingDataVKey.Add(name);
                dictBuildingData.Add(name, new BuildingMetadata(name, "Selling", "Oats"));
            }
            {
                string name = "No Man's Land";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new PointF[] { new Point(0, 0), new Point(0, 305), new Point(140, 509), new Point(510, 0) }, new RectangleF(-673, -306, 140, 512), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "No \nMan's \nLand", font[1], new RectangleF(-653, -170, 18 * name.Length, 25 * 3)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 1.07f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Ravenport";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(CropColors.Dirt, new RectangleF(-502, -406, 326, 692), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Ravenport", font[1], new RectangleF(-415, -100, 18 * name.Length, 25)));
                dictFieldsDataVKey.Add(name);
                dictFieldsData.Add(name, new FieldMetadata(name, NewName(), 4.27f, CropType.None, CropStatus.Empty, 0));
                dictTimerVKey.Add(name);
                dictTimer.Add(name, new Timer());
            }
            {
                string name = "Franks Fert Shop";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(Color.Transparent, new RectangleF(-291, 334, 120, 171), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Franks \nFert \nShop", font[0], new RectangleF(-275, 355, 18 * name.Length, 20 * 3)));
                dictBuildingDataVKey.Add(name);
                dictBuildingData.Add(name, new BuildingMetadata(name, "Buying", "Compost"));
            }
            {
                string name = "Joes Paint Shop";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(Color.Transparent, new RectangleF(701, -184, 320, 117), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Joes \nPaint \nShop", font[0], new RectangleF(720, -145, 18 * name.Length, 20 * 3)));
                dictBuildingDataVKey.Add(name);
                dictBuildingData.Add(name, new BuildingMetadata(name, "Painting"));
            }
            {
                string name = "Case Dealer";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(Color.Transparent, new RectangleF(-776, -505, 254, 172), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Case Dealer", font[0], new RectangleF(-750, -485, 18 * name.Length, 20)));
                dictBuildingDataVKey.Add(name);
                dictBuildingData.Add(name, new BuildingMetadata(name, "Dealing"));
            }
            {
                string name = "Jerrys Gas Station";
                dictBgVKey.Add(name);
                dictBg.Add(name, new ImgMover(Color.Transparent, new RectangleF(-520, 510, 220, 200), fill: true));
                dictTextVKey.Add(name);
                dictText.Add(name, new ImgMover(Color.Black, "Jerrys \nGas \nStation", font[0], new RectangleF(-355, 610, 18 * name.Length, 20 * 3)));
                dictBuildingDataVKey.Add(name);
                dictBuildingData.Add(name, new BuildingMetadata(name, "Gas"));
            }
            {
                //new PointF[] { new Point(0, 0), new Point(0, 200), new Point(400, 200), new Point(400, 600), new Point(800, 600), new Point(800, 0) },
            }
            #endregion
            // load player and others
            Player[0] = new ImgMover($"{Directory.GetCurrentDirectory()}/t/Redtractor.png", x: 220, y: 215, width: 84, height: 84);
            Player[0].RotatePic(angle);
            Player[1] = new ImgMover($"{Directory.GetCurrentDirectory()}/v/Redvan.png", x: -660, y: -420, width: 84, height: 84);
            Player[2] = new ImgMover($"{Directory.GetCurrentDirectory()}/h/Redharvester.png", x: -600, y: -420, width: 84, height: 84);

            #region load images

            // truck 
            VAkal[0, 0, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/Redtractor.png");
            VAkal[0, 0, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/RedtractorL.png");
            VAkal[0, 0, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/RedtractorR.png");
            VAkal[0, 1, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/Yellowtractor.png");
            VAkal[0, 1, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/YellowtractorL.png");
            VAkal[0, 1, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/YellowtractorR.png");
            VAkal[0, 2, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/Bluetractor.png");
            VAkal[0, 2, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/BluetractorL.png");
            VAkal[0, 2, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/BluetractorR.png");
            VAkal[0, 3, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/Greentractor.png");
            VAkal[0, 3, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/GreentractorL.png");
            VAkal[0, 3, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/t/GreentractorR.png");
            // van
            VAkal[1, 0, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/Redvan.png");
            VAkal[1, 0, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/RedvanL.png");
            VAkal[1, 0, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/RedvanR.png");
            VAkal[1, 1, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/Yellowvan.png");
            VAkal[1, 1, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/YellowvanL.png");
            VAkal[1, 1, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/YellowvanR.png");
            VAkal[1, 2, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/Bluevan.png");
            VAkal[1, 2, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/BluevanL.png");
            VAkal[1, 2, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/BluevanR.png");
            VAkal[1, 3, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/Greenvan.png");
            VAkal[1, 3, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/GreenvanL.png");
            VAkal[1, 3, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/v/GreenvanR.png");
            // harvester
            VAkal[2, 0, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/Redharvester.png");
            VAkal[2, 0, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/RedharvesterL.png");
            VAkal[2, 0, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/RedharvesterR.png");
            VAkal[2, 1, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/Yellowharvester.png");
            VAkal[2, 1, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/YellowharvesterL.png");
            VAkal[2, 1, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/YellowharvesterR.png");
            VAkal[2, 2, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/Blueharvester.png");
            VAkal[2, 2, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/BlueharvesterL.png");
            VAkal[2, 2, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/BlueharvesterR.png");
            VAkal[2, 3, 0] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/Greenharvester.png");
            VAkal[2, 3, 1] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/GreenharvesterL.png");
            VAkal[2, 3, 2] = Image.FromFile($"{Directory.GetCurrentDirectory()}/h/GreenharvesterR.png");
            #endregion
        }

        // paint vars
        /// <summary>
        /// Frist index is 12, second is 18, third is 12 not bold
        /// </summary>
        readonly Font[] font = { new Font("OCR A Extended", 12, FontStyle.Bold), new Font("OCR A Extended", 18, FontStyle.Bold), new Font("OCR A Extended", 12) };
        /// <summary>
        /// Frist index is red, second is black
        /// </summary>
        readonly SolidBrush[] brush = { new SolidBrush(Color.Red), new SolidBrush(Color.Black) };
        #endregion
        #region ONPAINT
        protected override void OnPaint(PaintEventArgs e)
        {
            if (start)
            {
                void message(string s, Font font, Brush brush, float x, float y, float width, float height, bool bg = true)
                {
                    if (bg) e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, 100, 210, 50)), x, y, width, height);
                    e.Graphics.DrawString(s, font, brush, x, y);
                }

                // backround
                for (int x = 0; x < dictBg.Count; x++)
                {
                    if (x != 1) e.Graphics.DrawImage(dictBg[dictBgVKey[x]].Image, dictBg[dictBgVKey[x]].X, dictBg[dictBgVKey[x]].Y, dictBg[dictBgVKey[x]].Width, dictBg[dictBgVKey[x]].Height);
                }

                // text
                { // text
                    // field text
                    for (int c = 0; c < dictText.Count; c++)
                    {
                        e.Graphics.DrawImage(dictText[dictTextVKey[c]].Image, dictText[dictTextVKey[c]].X, dictText[dictTextVKey[c]].Y, dictText[dictTextVKey[c]].Width, dictText[dictTextVKey[c]].Height);
                    }
                    // resources text top
                    string[] txtOutNames = resources.Keys.ToArray(); // get the resources names to array
                    int[] txtOutValues = resources.Values.ToArray(); // get the resources values to array
                    string[] txtOut = new string[txtOutNames.Length]; // declear a list to store the values
                    for (int r = 0; r < txtOutNames.Length; r++) // cycle thought the list of values
                    {
                        if (r == 0) txtOut[r] = $"{txtOutNames[r]} = {txtOutValues[r]}$"; // only if the symbol is a dollar sign
                        else txtOut[r] = $"{txtOutNames[r]} = {txtOutValues[r]}L"; // else put L
                    }
                    int spoty = 10; int spotx = 10; // make inital starting values
                    for (int i = 0; i < txtOut.Length; i++) // output the text
                    {
                        // sperate the text
                        string[] formatedText = new string[] { txtOut[i].Substring(0, txtOut[i].LastIndexOf("=")), txtOut[i].Substring(txtOut[i].LastIndexOf("="), 1), txtOut[i].Substring(txtOut[i].LastIndexOf("=") + 1, txtOut[i].Length - 1 - txtOut[i].LastIndexOf("=")) };

                        // outputting it
                        int[] line = { spotx, spotx + 80, spotx + 80 + 10 }; // make an array of x values
                        for (int l = 0; l < formatedText.Length; l++) e.Graphics.DrawString($"{formatedText[l]}", font[0], brush[0], line[l], spoty); // output text

                        // changing loction
                        spoty += 20;
                        if (spoty > 50)
                        {
                            if (spotx == 190) spotx = 360;
                            else spotx = 190;
                            spoty = 10;
                        }
                    }
                }

                // player
                for (int i = 0; i < Player.Length; i++)
                    if (i != ttruck) e.Graphics.DrawImage(Player[i].Image, Player[i].X, Player[i].Y, Player[i].Width, Player[i].Height);
                e.Graphics.DrawImage(Player[ttruck].Image, Player[ttruck].X, Player[ttruck].Y, Player[ttruck].Width, Player[ttruck].Height);

                // garage
                e.Graphics.DrawImage(dictBg["garage"].Image, dictBg["garage"].X, dictBg["garage"].Y, dictBg["garage"].Width, dictBg["garage"].Height);
                if (garageClosed) message(outC[9], font[0], brush[0], 180, 100, 165, 20);

                // message text
                if (importantMess != null)
                {
                    message(importantMess, font[1], brush[0], 130, 170, 260, 50);
                }
                string value = WhatPIsIn(); // find out what the player is in
                if (value == "field")
                {
                    // output the "wait for second key message"
                    if (waitForSecondKey != null) message("Waiting for second key...", font[0], brush[1], 160, 170, 260, 20);
                    // title
                    message("FIELD INFO", font[1], brush[1], 300, 320, 180, 24, bg: false);
                    // make a smaller, easyer to acsess varable
                    FieldMetadata meta = dictFieldsData[isIn];
                    // make a message string
                    string mess = $"{"Name",-5}{meta.Name,16}\n" +
                        $"{"Owner",-8}{meta.Owner,13}\n" +
                        $"{"Value",-8}{(int)(meta.Size * 1250) + "$",13}\n" +
                        $"{"Size",-8}{(meta.Size).ToString("N2") + "km\u00B2",13}\n" +
                        $"{"Type",-8}{meta.Type,13}\n" +
                        $"{"Status",-8}{meta.Status,13}\n" +
                        $"{"Fertilized",-8}{meta.FertAmount / 2f * 100 + "%  ",13}\n";
                    //$"{"asdfghjklqasdfghjklqa",21}" length of box
                    // output message string
                    message(mess, font[0], brush[1], 305, 350, 222, 120);
                }
                else if (value == "building")
                {
                    message(outC[10], font[0], brush[1], 195, 121, 135, 20);
                }

                // controls
                { // controls
                    if (showControls)
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(100, 95, 0, 57), 5), new Rectangle(50, 70, 450, 390)); // outline
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, 95, 117, 57)), new Rectangle(50, 70, 450, 390)); // backround
                        e.Graphics.DrawString("Controls", new Font("OCR A Extended", 18, System.Drawing.FontStyle.Bold), new SolidBrush(Color.White), 200, 80); // tittle
                        int spotx = 70; int spoty = 120; // defalt spots
                        for (int i = 0; i < outC.Length; i++) // output the text in spots
                        {
                            // sperate the text
                            string[] formatedText = new string[] { outC[i].Substring(0, outC[i].LastIndexOf("=")), outC[i].Substring(outC[i].LastIndexOf("="), 1), outC[i].Substring(outC[i].LastIndexOf("=") + 1, outC[i].Length - 1 - outC[i].LastIndexOf("=")) };

                            // outputting it
                            int[] line = { spotx, spotx + 25, spotx + 25 + 10 }; // make an array of x values
                            for (int l = 0; l < formatedText.Length; l++) e.Graphics.DrawString($"{formatedText[l]}", font[0], new SolidBrush(Color.WhiteSmoke), line[l], spoty);

                            // changing loction
                            spoty += 20;
                            if (spoty > 420)
                            {
                                spotx = 300;
                                spoty = 120;
                            }
                        }
                    }
                }
            }
            base.OnPaint(e);
        }
        #endregion
        #region my funtions
        private void ShowUi(Visible howMany)
        {
            if (howMany == Sim.Visible.Hide)
            {
                btnStart.Hide();
                btnLoad.Hide();
                lbltitlelogo.Hide();
                picText.Hide();
                picTitle.Hide();
                lblC.Hide();
                lblOpt.Hide();
                lbl3DN.Hide();
                txtOpt.Hide();
                lbl3DL.Hide();
                btn3DS.Hide();
                btn3DL.Hide();
                txt3DT.Hide();
                Focus();
                tmrKeyTimer.Start();
                start = true;
            }
            else if (howMany == Sim.Visible.Show)
            {
                btnStart.Show();
                btnLoad.Show();
                lbltitlelogo.Show();
                picText.Show();
                picTitle.Show();
                lblOpt.Show();
                lbl3DN.Show();
                txtOpt.Show();
                lblC.Show();
                lbl3DL.Show();
                btn3DS.Show();
                btn3DL.Show();
                txt3DT.Show();
                Focus();
                tmrKeyTimer.Stop();
                start = false;
            }
        }
        #endregion
        #region Simple timers
        readonly Timer tmrGarage = new Timer() { Enabled = false, Interval = 20 };
        readonly Timer tmrImportantMess = new Timer() { Enabled = false, Interval = 5_000 };
        readonly Timer tmrGasF = new Timer() { Enabled = false, Interval = 500 };

        private void TmrGarage_Tick(object sender, EventArgs e)
        { // 89, 88
            // the timer tick to slowy open and close the gargage
            if (garageClosed)
            {
                dictBg["garage"].Height -= 2;
                if (dictBg["garage"].Height <= 1)
                {
                    tmrGarage.Stop();
                    garageClosed = false;
                }
            }
            else
            {
                dictBg["garage"].Height += 2;
                if (dictBg["garage"].Height >= 88)
                {
                    tmrGarage.Stop();
                    garageClosed = true;
                }
            }
            Refresh();
        }
        private void ResetImportantMess(object sender, EventArgs e)
        {
            // to remove the custom message box from the screen
            importantMess = null;
            Refresh();
        }
        private void FillTank()
        {
            // to slowy fill up the gas tank
            if (resources["Diesel"] < 100 && WhatPIsIn() == "building")
            {
                if (resources["Dollars"] < 2)
                {
                    importantMess = $"Not Enought \nDollars (2$)";
                    tmrImportantMess.Start();
                }
                else
                {
                    resources["Dollars"] -= 2;
                    resources["Diesel"]++;
                }
            }
            else if (resources["Diesel"] == 100 || WhatPIsIn() != "building") tmrGasF.Stop();
            Refresh();
        }
        #endregion
        #region other methods
        private string WhatPIsIn()
        {
            string value = null;

            // find out what the player is in
            if (dictFieldsDataVKey.Contains(isIn)) value = "field";
            else if (dictBuildingDataVKey.Contains(isIn)) value = "building";

            return value;
        }
        private string NewName() { return OthersNames[rn.Next(0, OthersNames.Length)]; }
        #endregion
        #region action
        private void FieldAction(string name, CropStatus status, Color[] color, CropType type)
        {
            if (status == CropStatus.Stage1)
            {
                dictTimer[name].Stop(); // stop the old timer
                dictTimer[name] = new Timer(); // reset the timer events
                dictBg[name].Changecolor(color[0]); // change the color
                dictFieldsData[name].Status = CropStatus.Stage1; // change the status
                dictTimer[name].Interval = rn.Next(64, 82) * 1000; // make a new random interval
                dictTimer[name].Tick += delegate (object sende, EventArgs er) { FieldAction(name, CropStatus.Stage2, color, type); }; // setting for the next timer 
                dictTimer[name].Start(); // start the timer
            }
            else if (status == CropStatus.Stage2)
            {
                dictTimer[name].Stop(); // stop the old timer
                dictTimer[name] = new Timer(); // reset the timer events
                dictBg[name].Changecolor(color[1]); // change the color
                dictFieldsData[name].Status = CropStatus.Stage2; // change the status
                dictTimer[name].Interval = rn.Next(56, 72) * 1000; // make a new random interval
                dictTimer[name].Tick += delegate (object sende, EventArgs er) { FieldAction(name, CropStatus.Stage3, color, type); }; // setting for the next timer 
                dictTimer[name].Start(); // start the timer
            }
            else if (status == CropStatus.Stage3)
            {
                dictTimer[name].Stop(); // stop the old timer
                dictTimer[name] = new Timer(); // reset the timer events
                dictBg[name].Changecolor(color[2]); // change the color
                dictFieldsData[name].Status = CropStatus.Stage3; // change the status
                dictTimer[name].Interval = rn.Next(40, 51) * 1000; // make a new random interval
                dictTimer[name].Tick += delegate (object sende, EventArgs er) { FieldAction(name, CropStatus.Ready, color, type); }; // setting for the next timer 
                dictTimer[name].Start(); // start the timer
            }
            else if (status == CropStatus.Ready)
            {
                dictTimer[name].Stop(); // stop the old timer
                dictTimer[name] = new Timer(); // reset the timer events
                dictBg[name].Changecolor(color[3]); // change the color
                dictFieldsData[name].Status = CropStatus.Ready; // change the status
                if (dictFieldsData[name].Owner != "You") // if you don't own it
                {
                    dictTimer[name].Interval = rn.Next(60, 100) * 1000; // make a new random interval
                    dictTimer[name].Tick += delegate (object sende, EventArgs er) { FieldAction(name, CropStatus.Empty, color, type); }; // setting for the next timer 
                    dictTimer[name].Start(); // start the timer
                }
            }
            else if (status == CropStatus.Empty)
            {
                if (dictFieldsData[name].Owner != "You") // if you don't own it
                {
                    // make empty
                    dictBg[name].Changecolor(CropColors.Dirt); // set the crop color
                    dictFieldsData[name].Status = CropStatus.Empty; // set the crop status
                    dictFieldsData[name].Type = CropType.None; // set the crop type
                    dictFieldsData[name].FertAmount = 0; // set the fert amount
                    // set timer to plant
                    dictTimer[name].Interval = rn.Next(10, 60) * 1000; // make a new random interval
                    dictTimer[name].Tick += delegate { StartPlantAction(name); }; // setting for the next timer 
                    dictTimer[name].Start(); // start the timer
                }
            }
            Refresh();
        }
        private void StartPlantAction(string index)
        {
            if (!loaded)
            {
                if (dictFieldsData[index].Owner != "You") // if you don't own it
                {
                    int R = rn.Next(3);
                    string n = index;
                    dictBg[n].Changecolor(CropColors.Planted); // set the crop color
                    dictFieldsData[n].Status = CropStatus.Planted; // set the crop status
                    dictTimer[n].Interval = rn.Next(25, 36) * 1000; // make a new random interval

                    if (R == 0)
                    {
                        dictFieldsData[n].Type = CropType.Wheat; // set the crop type
                        dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage1, new Color[] { CropColors.Wheat1, CropColors.Wheat2, CropColors.Wheat3, CropColors.WheatReady }, CropType.Wheat); }; // set the tick
                    }
                    if (R == 1)
                    {
                        dictFieldsData[n].Type = CropType.Barley; // set the crop type
                        dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage1, new Color[] { CropColors.Barley1, CropColors.Barley2, CropColors.Barley3, CropColors.BarleyReady }, CropType.Barley); }; // set the tick
                    }
                    if (R == 2)
                    {
                        dictFieldsData[n].Type = CropType.Oats; // set the crop type
                        dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage1, new Color[] { CropColors.Oat1, CropColors.Oat2, CropColors.Oat3, CropColors.OatReady }, CropType.Oats); }; // set the tick
                    }
                    dictTimer[n].Start(); // start the timer
                }
            }
            else if (loaded)
            {
                string n = index;
                // changing colors
                Color ccolor = CropColors.Dirt;
                if (dictFieldsData[n].Type == CropType.None) ccolor = CropColors.Dirt; // if dirt
                else if (dictFieldsData[n].Status == CropStatus.Planted) ccolor = CropColors.Planted; // if planted
                else if (dictFieldsData[n].Type == CropType.Wheat) // if wheat and stage
                {
                    if (dictFieldsData[n].Status == CropStatus.Stage1) ccolor = CropColors.Wheat1;
                    else if (dictFieldsData[n].Status == CropStatus.Stage2) ccolor = CropColors.Wheat2;
                    else if (dictFieldsData[n].Status == CropStatus.Stage3) ccolor = CropColors.Wheat3;
                    else if (dictFieldsData[n].Status == CropStatus.Ready) ccolor = CropColors.WheatReady;
                }
                else if (dictFieldsData[n].Type == CropType.Barley) // if barley and stage
                {
                    if (dictFieldsData[n].Status == CropStatus.Stage1) ccolor = CropColors.Barley1;
                    else if (dictFieldsData[n].Status == CropStatus.Stage2) ccolor = CropColors.Barley2;
                    else if (dictFieldsData[n].Status == CropStatus.Stage3) ccolor = CropColors.Barley3;
                    else if (dictFieldsData[n].Status == CropStatus.Ready) ccolor = CropColors.BarleyReady;
                }
                else if (dictFieldsData[n].Type == CropType.Oats) // if oats and stage
                {
                    if (dictFieldsData[n].Status == CropStatus.Stage1) ccolor = CropColors.Oat1;
                    else if (dictFieldsData[n].Status == CropStatus.Stage2) ccolor = CropColors.Oat2;
                    else if (dictFieldsData[n].Status == CropStatus.Stage3) ccolor = CropColors.Oat3;
                    else if (dictFieldsData[n].Status == CropStatus.Ready) ccolor = CropColors.OatReady;
                }
                dictBg[n].Changecolor(ccolor);

                // setting the timer and adding the params to the timer
                int ttimer = 100;
                if (dictFieldsData[n].Status == CropStatus.Planted)
                {
                    ttimer = rn.Next(25, 36) * 1000;
                    if (dictFieldsData[n].Type == CropType.Wheat) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage1, new Color[] { CropColors.Wheat1, CropColors.Wheat2, CropColors.Wheat3, CropColors.WheatReady }, CropType.Wheat); }; // set the tick
                    if (dictFieldsData[n].Type == CropType.Barley) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage1, new Color[] { CropColors.Barley1, CropColors.Barley2, CropColors.Barley3, CropColors.BarleyReady }, CropType.Barley); }; // set the tick
                    if (dictFieldsData[n].Type == CropType.Oats) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage1, new Color[] { CropColors.Oat1, CropColors.Oat2, CropColors.Oat3, CropColors.OatReady }, CropType.Oats); }; // set the tick
                }
                else if (dictFieldsData[n].Status == CropStatus.Stage1)
                {
                    ttimer = rn.Next(64, 82) * 1000;
                    if (dictFieldsData[n].Type == CropType.Wheat) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage2, new Color[] { CropColors.Wheat1, CropColors.Wheat2, CropColors.Wheat3, CropColors.WheatReady }, CropType.Wheat); }; // set the tick
                    if (dictFieldsData[n].Type == CropType.Barley) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage2, new Color[] { CropColors.Barley1, CropColors.Barley2, CropColors.Barley3, CropColors.BarleyReady }, CropType.Barley); }; // set the tick
                    if (dictFieldsData[n].Type == CropType.Oats) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage2, new Color[] { CropColors.Oat1, CropColors.Oat2, CropColors.Oat3, CropColors.OatReady }, CropType.Oats); }; // set the tick
                }
                else if (dictFieldsData[n].Status == CropStatus.Stage2)
                {
                    ttimer = rn.Next(56, 72) * 1000;
                    if (dictFieldsData[n].Type == CropType.Wheat) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage3, new Color[] { CropColors.Wheat1, CropColors.Wheat2, CropColors.Wheat3, CropColors.WheatReady }, CropType.Wheat); }; // set the tick
                    if (dictFieldsData[n].Type == CropType.Barley) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage3, new Color[] { CropColors.Barley1, CropColors.Barley2, CropColors.Barley3, CropColors.BarleyReady }, CropType.Barley); }; // set the tick
                    if (dictFieldsData[n].Type == CropType.Oats) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage3, new Color[] { CropColors.Oat1, CropColors.Oat2, CropColors.Oat3, CropColors.OatReady }, CropType.Oats); }; // set the tick
                }
                else if (dictFieldsData[n].Status == CropStatus.Stage3)
                {
                    ttimer = rn.Next(40, 51) * 1000;
                    if (dictFieldsData[n].Type == CropType.Wheat) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Ready, new Color[] { CropColors.Wheat1, CropColors.Wheat2, CropColors.Wheat3, CropColors.WheatReady }, CropType.Wheat); }; // set the tick
                    if (dictFieldsData[n].Type == CropType.Barley) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Ready, new Color[] { CropColors.Barley1, CropColors.Barley2, CropColors.Barley3, CropColors.BarleyReady }, CropType.Barley); }; // set the tick
                    if (dictFieldsData[n].Type == CropType.Oats) dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Ready, new Color[] { CropColors.Oat1, CropColors.Oat2, CropColors.Oat3, CropColors.OatReady }, CropType.Oats); }; // set the tick
                }
                if (dictFieldsData[n].Owner != "You") // if you don't own it
                {
                    if (dictFieldsData[n].Status == CropStatus.Ready)
                    {
                        ttimer = rn.Next(60, 100) * 1000;
                        dictTimer[n].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Empty, null, CropType.None); }; // setting for the next timer 
                    }
                    else if (dictFieldsData[n].Status == CropStatus.Empty)
                    {
                        ttimer = rn.Next(10, 60) * 1000;
                        dictTimer[n].Tick += delegate { StartPlantAction(n); };
                    }
                }
                dictTimer[n].Interval = ttimer;

                dictTimer[n].Start(); // start the timer
            }
        }
        private void ChangeCar(int carTo)
        {
            // the original loction and the ending point for 
            PointF start = new PointF(Player[ttruck].X, Player[ttruck].Y);
            PointF desty = new PointF(Player[carTo].X, Player[carTo].Y);
            // set the new car to the current point so i can move the backround
            Player[carTo].X = start.X;
            Player[carTo].Y = start.Y;
            // set the current 
            ttruck = carTo;
            angle = Player[ttruck].Angle;
            // the values we need to move
            int loop = 1; // acuatecy 
            float moveX = start.X - desty.X, moveY = start.Y - desty.Y; // the diffrence
            float interX = moveX / loop, interY = moveY / loop; // amount of times to move 
            if (interX > 0) for (int i = 0; i < interX; i++) MakeMove(loop, Way.Right);
            if (interX < 0) for (int i = 0; i > interX; i--) MakeMove(loop, Way.Left);
            if (interY > 0) for (int i = 0; i < interY; i++) MakeMove(loop, Way.Down);
            if (interY < 0) for (int i = 0; i > interY; i--) MakeMove(loop, Way.Up);
        }
        #endregion
        #region save load
        private void Saver()
        {
            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "/Saves/Resources.txt"))
                for (int i = 0; i < resources.Count; i++) sw.WriteLine(resources.ElementAt(i).Value);
            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "/Saves/Field.txt"))
                for (int i = 0; i < dictFieldsData.Count; i++)
                {
                    FieldMetadata d = dictFieldsData.ElementAt(i).Value;
                    sw.WriteLine($"{d.Owner},{d.Type},{d.Status},{d.FertAmount}");
                }
            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "/Saves/Owner.txt"))
                for (int i = 0; i < carOwned.Length; i++) sw.WriteLine(carOwned[i]);
            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "/Saves/Coolors.txt"))
                for (int i = 0; i < tcolor.Length; i++) sw.WriteLine(tcolor[i]);

        }
        private void Loader()
        {
            try
            {
                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/Saves/Resources.txt"))
                    for (int i = 0; i < resources.Count; i++) resources[resources.ElementAt(i).Key] = Convert.ToInt32(sr.ReadLine());
                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/Saves/Field.txt"))
                    for (int i = 0; i < dictFieldsData.Count; i++)
                    {
                        // make a version of the dictarny
                        FieldMetadata d = dictFieldsData.ElementAt(i).Value;
                        // the current string we are reading
                        string[] r = sr.ReadLine().Split(',');
                        // loading and converting the file
                        d.Owner = r[0]; d.Type = (CropType)Enum.Parse(typeof(CropType), r[1]);
                        d.Status = (CropStatus)Enum.Parse(typeof(CropStatus), r[2]); d.FertAmount = Convert.ToInt32(r[3]);
                    }
                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/Saves/Owner.txt"))
                    for (int i = 0; i < carOwned.Length; i++) carOwned[i] = bool.Parse(sr.ReadLine());
                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/Saves/Coolors.txt"))
                    for (int i = 0; i < tcolor.Length; i++) tcolor[i] = int.Parse(sr.ReadLine());

                loaded = true;
            }
            catch (FileNotFoundException) { Saver(); }
            catch (ArgumentException) { Saver(); }
        }
        #endregion
        #region vs funtions
        private void Form1_Load(object sender, EventArgs e)
        {
            // set timer events
            tmrGarage.Tick += TmrGarage_Tick;
            tmrImportantMess.Tick += ResetImportantMess;
            tmrGasF.Tick += delegate { FillTank(); };

            // set paint and dealer panel
            #region Painting
            painting = new Panel
            {
                BackColor = Color.LightGray,
                Size = new Size(375, 325),
                Location = new Point(90, 100),

            };
            Label lblTitle = new Label
            {
                Text = "Joes Paint Shop",
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                Font = new Font("OCR A Extended", 14, FontStyle.Bold),
                Size = new Size(190, 20),
                Location = new Point(painting.Width / 2 - 190 / 2, 5),
            };
            rad1.Add("red", new RadioButton
            {
                Text = "Red",
                Checked = true,
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                Font = new Font("OCR A Extended", 12, FontStyle.Bold),
                Size = new Size(100, 28),
                Location = new Point(27, 70),
            });
            rad1.Add("yellow", new RadioButton
            {
                Text = "Yellow",
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                Font = new Font("OCR A Extended", 12, FontStyle.Bold),
                Size = new Size(100, 28),
                Location = new Point(27, 110),
            });
            rad1.Add("blue", new RadioButton
            {
                Text = "Blue",
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                Font = new Font("OCR A Extended", 12, FontStyle.Bold),
                Size = new Size(100, 28),
                Location = new Point(27, 150),
            });
            rad1.Add("green", new RadioButton
            {
                Text = "Green",
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                Font = new Font("OCR A Extended", 12, FontStyle.Bold),
                Size = new Size(100, 28),
                Location = new Point(27, 190),
            });
            Label lblCost = new Label
            {
                Text = "Total cost =",
                ForeColor = Color.Black,
                BackColor = Color.Gray,
                Font = new Font("OCR A Extended", 13, FontStyle.Bold),
                Size = new Size(180, 50),
                Location = new Point(24, 250),
            };
            Label lblTotal = new Label
            {
                Text = "1750 $",
                ForeColor = Color.Black,
                BackColor = Color.Gray,
                Font = new Font("OCR A Extended", 13, FontStyle.Bold),
                Size = new Size(90, 50),
                Location = new Point(210, 250),
            };
            Button btnOK = new Button
            {
                Text = "OK",
                ForeColor = Color.Black,
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("OCR A Extended", 13, FontStyle.Bold),
                Size = new Size(50, 40),
                Location = new Point(310, 255),
            };
            Button btnEXT = new Button
            {
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.Red,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("OCR A Extended", 13, FontStyle.Bold),
                Size = new Size(50, 30),
                Location = new Point(325, 0),
            };
            picColor = new PictureBox
            {
                BackColor = Color.Red,
                Size = new Size(140, 160),
                Location = new Point(180, 70),
            };
            painting.Controls.Add(lblTitle);
            painting.Controls.Add(rad1["red"]);
            painting.Controls.Add(rad1["yellow"]);
            painting.Controls.Add(rad1["blue"]);
            painting.Controls.Add(rad1["green"]);
            painting.Controls.Add(lblCost);
            painting.Controls.Add(lblTotal);
            painting.Controls.Add(btnOK);
            painting.Controls.Add(btnEXT);
            painting.Controls.Add(picColor);

            // progarm commands
            string[] k = rad1.Keys.ToArray();
            for (int lmno = 0; lmno < k.Length; lmno++)
            {
                string l = k[lmno];
                rad1[l].CheckedChanged += delegate { Rad1Action(l); };
            }
            btnOK.Click += delegate { PickCoolor(); };
            btnEXT.Click += delegate { XX(); };
            #endregion
            #region Dealing
            dealing = new Panel
            {
                BackColor = Color.LightGray,
                Size = new Size(375, 325),
                Location = new Point(90, 100),

            };
            Label lblTitle1 = new Label
            {
                Text = "Case Dealer",
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                Font = new Font("OCR A Extended", 14, FontStyle.Bold),
                Size = new Size(190, 20),
                Location = new Point(dealing.Width / 2 - 190 / 2, 5),
            };
            rad2.Add("van", new RadioButton
            {
                Text = "Van",
                Checked = true,
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                Font = new Font("OCR A Extended", 12, FontStyle.Bold),
                Size = new Size(100, 28),
                Location = new Point(27, 100),
            });
            rad2.Add("harvester", new RadioButton
            {
                Text = "Harvester",
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                Font = new Font("OCR A Extended", 12, FontStyle.Bold),
                Size = new Size(130, 28),
                Location = new Point(27, 150),
            });
            Label lblCost1 = new Label
            {
                Text = "Total cost =",
                ForeColor = Color.Black,
                BackColor = Color.Gray,
                Font = new Font("OCR A Extended", 13, FontStyle.Bold),
                Size = new Size(180, 50),
                Location = new Point(24, 250),
            };
            Label lblTotal1 = new Label
            {
                Text = "5000 $",
                ForeColor = Color.Black,
                BackColor = Color.Gray,
                Font = new Font("OCR A Extended", 13, FontStyle.Bold),
                Size = new Size(90, 50),
                Location = new Point(210, 250),
            };
            Button btnOK1 = new Button
            {
                Text = "OK",
                ForeColor = Color.Black,
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("OCR A Extended", 13, FontStyle.Bold),
                Size = new Size(50, 40),
                Location = new Point(310, 255),
            };
            Button btnEXT1 = new Button
            {
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.Red,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("OCR A Extended", 13, FontStyle.Bold),
                Size = new Size(50, 30),
                Location = new Point(325, 0),
            };
            picColor1 = new PictureBox
            {
                BackColor = Color.Red,
                Size = new Size(140, 160),
                Location = new Point(180, 70),
            };
            dealing.Controls.Add(lblTitle1);
            dealing.Controls.Add(rad2["van"]);
            dealing.Controls.Add(rad2["harvester"]);
            dealing.Controls.Add(lblCost1);
            dealing.Controls.Add(lblTotal1);
            dealing.Controls.Add(btnOK1);
            dealing.Controls.Add(btnEXT1);
            dealing.Controls.Add(picColor1);

            // progarm commands
            string[] k1 = rad2.Keys.ToArray();
            for (int lmno = 0; lmno < k1.Length; lmno++)
            {
                string l = k1[lmno];
                rad2[l].CheckedChanged += delegate { Rad2Action(l); };
            }
            btnOK1.Click += delegate { PickCoolor1(); };
            btnEXT1.Click += delegate { XX1(); };
            #endregion
        }
        private void BtnStart_Click(object sender, EventArgs e)
        {
            ShowUi(Sim.Visible.Hide);
            // get address/name text
            address = (txtOpt.Text.Length > 0) ? txtOpt.Text : "A";
            // format it
            address = address[0].ToString().ToUpper() + address.Substring(1) + "'s";
            // set it for output
            dictText["address"].MakeImage(Color.Black, address, font[2], new Rectangle((100) - (int)(12f * (address.Length / 2f)), 190, 12 * address.Length, 17));
            // load the old game
            if (((Button)sender).Text == "Load") Loader();
            for (int i = 0; i < dictFieldsData.Count; i++) // start field timers
                StartPlantAction(dictFieldsDataVKey[i]);
            loaded = false;
            // update the picture
            Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 0];
            Player[ttruck].RotatePic(angle);
            Refresh();
        }
        private void Sim94_FormClosing(object sender, FormClosingEventArgs e)
        {
            Saver();
        }
        #endregion
        #region Shopping
        private void Rad1Action(string name)
        {
            // change the panel picture
            if (name == "red") picColor.BackColor = Color.Red;
            if (name == "yellow") picColor.BackColor = Color.Yellow;
            if (name == "blue") picColor.BackColor = Color.Blue;
            if (name == "green") picColor.BackColor = Color.Green;
        }
        private void Rad2Action(string name)
        {
            // change the panel picture
            if (name == "red") picColor1.BackColor = Color.Red;
            if (name == "yellow") picColor1.BackColor = Color.Yellow;
            if (name == "blue") picColor1.BackColor = Color.Blue;
            if (name == "green") picColor1.BackColor = Color.Green;
        }
        private void PickCoolor()
        {
            // set ccolor of the car
            string ccolor = null;
            if (tcolor[ttruck] == 0) ccolor = "red";
            if (tcolor[ttruck] == 1) ccolor = "yellow";
            if (tcolor[ttruck] == 2) ccolor = "blue";
            if (tcolor[ttruck] == 3) ccolor = "green";
            // find the checked one
            string[] k = rad1.Keys.ToArray();
            for (int lmno = 0; lmno < k.Length; lmno++)
            {
                if (rad1[k[lmno]].Checked && ccolor != k[lmno] && resources["Dollars"] >= 1750) { resources["Dollars"] -= 1750; tcolor[ttruck] = lmno; }
                else if (rad1[k[lmno]].Checked && ccolor != k[lmno] && resources["Dollars"] <= 1750)
                {
                    importantMess = $"Not Enought \nDollars (1750$)";
                    tmrImportantMess.Start();
                }
            }
            // close
            XX();
        }
        private void PickCoolor1()
        {
            // buying a car
            if (!carOwned[1] && rad2["van"].Checked)
                if (resources["Dollars"] >= 5000) { resources["Dollars"] -= 5000; carOwned[1] = true; }
            if (!carOwned[2] && rad2["harvester"].Checked)
                if (resources["Dollars"] >= 5000) { resources["Dollars"] -= 5000; carOwned[2] = true; }
            // close
            XX1();

        }
        private void XX() { Controls.Remove(painting); Focus(); Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 0]; Player[ttruck].RotatePic(angle); Refresh(); }
        private void XX1() { Controls.Remove(dealing); Focus(); Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 0]; Player[ttruck].RotatePic(angle); Refresh(); }
        #endregion
        #region Keys
        // left, down, right, up
        private readonly bool[] wasdPressed = new bool[] { false, false, false, false };
        private readonly bool[] arrowPressed = new bool[] { false, false, false, false };
        private void TxtOpt_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // if enter on text widget
            if (e.KeyCode == Keys.Enter) BtnStart_Click(null, null);
        }
        private void MakeMove(float amount, Way dierection = Way.Down)
        {
            // hide the painting panel and dealing
            Controls.Remove(painting);
            Controls.Remove(dealing);
            // move the pic
            void MoveYay(float num, Way die = Way.Down)
            {
                for (int b = 0; b < dictBg.Count; b++) // move the whole background
                {
                    dictBg[dictBgVKey[b]].MovePic(num, die);
                }
                for (int b = 0; b < dictText.Count; b++) // move the whole text
                {
                    dictText[dictTextVKey[b]].MovePic(num, die);
                }
                for (int b = 0; b < Player.Length; b++) // move the player
                {
                    if (b != ttruck) Player[b].MovePic(num, die);
                }
                // get the player values
                float px = s.X + Player[ttruck].X;
                float py = s.Y + Player[ttruck].Y;
                float pw = Player[ttruck].Width;
                float ph = Player[ttruck].Height;
                float pxc = px + (pw / 2);
                float pyc = py + (ph / 2);
                isIn = null; // set isIn to defalt
                for (int b = 0; b < dictBg.Count; b++) // check if in field or building
                {
                    if (dictBgVKey[b] != "bg")
                    {
                        // get the object values
                        float ox = s.X + dictBg[dictBgVKey[b]].X;
                        float oy = s.Y + dictBg[dictBgVKey[b]].Y;
                        float ow = dictBg[dictBgVKey[b]].Width;
                        float oh = dictBg[dictBgVKey[b]].Height;

                        // set the isIn varable
                        if (pxc > ox && pyc > oy && pxc < ox + ow && pyc < oy + oh) isIn = dictBgVKey[b];

                        //if (isIn != null) Console.WriteLine(isIn);
                    }
                }
            }
            // move the player
            void MoveYeah(float num, Way die = Way.Down)
            {
                Player[ttruck].MovePic(num, die); // move the player
                isIn = null; // set isIn to defalt
                for (int b = 0; b < dictBg.Count; b++) // check if in field or building
                {
                    if (dictBgVKey[b] != "bg")
                    {
                        // get the player values
                        float px = s.X + Player[ttruck].X;
                        float py = s.Y + Player[ttruck].Y;
                        float pw = Player[ttruck].Width;
                        float ph = Player[ttruck].Height;
                        float pxc = px + (pw / 2);
                        float pyc = py + (ph / 2);
                        // get the object values
                        float ox = s.X + dictBg[dictBgVKey[b]].X;
                        float oy = s.Y + dictBg[dictBgVKey[b]].Y;
                        float ow = dictBg[dictBgVKey[b]].Width;
                        float oh = dictBg[dictBgVKey[b]].Height;

                        // set the isIn varable
                        if (pxc > ox && pyc > oy && pxc < ox + ow && pyc < oy + oh) isIn = dictBgVKey[b];

                        //if (isIn != null) Console.WriteLine(isIn);
                    }
                }
            }
            // check bounds for player in map
            if (dierection == Way.Left) // left
            {
                if (Player[ttruck].X < 220) MoveYeah(amount, Way.Right); // move pic back to center
                else
                {
                    // to prevent the piture from showing the void on the right side
                    if (dictBg["bg"].X + dictBg["bg"].Width < picTitle.Right + 5) // if picture max then move player
                    {
                        if (Player[ttruck].X + Player[ttruck].Width < picTitle.Right) // if player not max
                            MoveYeah(amount, Way.Right);
                    }
                    else MoveYay(amount, dierection);
                }
            }
            else if (dierection == Way.Down) // down
            {
                if (Player[ttruck].Y > 215) MoveYeah(amount, Way.Up); // move pic back to center
                else
                {
                    // to prevent the piture from showing the void on the top side
                    if (dictBg["bg"].Y > picTitle.Top - 10) // if picture max then move player
                    {
                        if (Player[ttruck].Y > picTitle.Top) // if player not max
                            MoveYeah(amount, Way.Up);
                    }
                    else MoveYay(amount, dierection);
                }
            }
            else if (dierection == Way.Right) // right
            {
                if (Player[ttruck].X > 220) MoveYeah(amount, Way.Left); // move pic back to center
                else
                {
                    // to prevent the piture from showing the void on the left side
                    if (dictBg["bg"].X > picTitle.Left - 5) // picture max then move player
                    {
                        if (Player[ttruck].X > picTitle.Left) // if player not max
                            MoveYeah(amount, Way.Left);
                    }
                    else MoveYay(amount, dierection);
                }
            }
            else if (dierection == Way.Up) // up
            {
                if (Player[ttruck].Y < 215) MoveYeah(amount, Way.Down); // move pic back to center
                else
                {
                    // to prevent the piture from showing the void on the bottom side
                    if (dictBg["bg"].Y + dictBg["bg"].Height < picTitle.Bottom + 5) // if picture max then move player
                    {
                        if (Player[ttruck].Y + Player[ttruck].Height < picTitle.Bottom - 5) // if player not max
                            MoveYeah(amount, Way.Down);
                    }
                    else MoveYay(amount, dierection);
                }
            }
        }
        private void Form1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (start == true)
            {
                // game contorls WASD
                if (e.KeyCode == Keys.A) wasdPressed[0] = false;
                if (e.KeyCode == Keys.S) wasdPressed[1] = false;
                if (e.KeyCode == Keys.D) wasdPressed[2] = false;
                if (e.KeyCode == Keys.W) wasdPressed[3] = false;

                // game contorls ARROW
                if (e.KeyCode == Keys.Left) arrowPressed[0] = false;
                if (e.KeyCode == Keys.Down) arrowPressed[1] = false;
                if (e.KeyCode == Keys.Right) arrowPressed[2] = false;
                if (e.KeyCode == Keys.Up) arrowPressed[3] = false;

                // show controls
                if (e.KeyCode == Keys.C) showControls = !showControls;

                // garage
                if (e.KeyCode == Keys.G) tmrGarage.Start();

                // changing cars
                if (e.KeyCode == Keys.D1 && carOwned[0]) ChangeCar(0);
                if (e.KeyCode == Keys.D2 && carOwned[1]) ChangeCar(1);
                if (e.KeyCode == Keys.D3 && carOwned[2]) ChangeCar(2);

                // saving
                if (e.KeyCode == Keys.I) Saver();

                if ((e.KeyCode == Keys.I || e.KeyCode == Keys.F || e.KeyCode == Keys.H || e.KeyCode == Keys.B || e.KeyCode == Keys.V) && isIn != null) // check if isIn is not null then interact with the coraspwanding action
                {
                    // find out what the player is in
                    string value = WhatPIsIn();
                    if (value == "field" && ttruck != 1)
                    {
                        // make a smaller, easyer to acsess varable
                        FieldMetadata meta = dictFieldsData[isIn];
                        if (meta.Owner == "You") // if you are the owner
                        {
                            // check the status of the field for empty
                            if (meta.Status == CropStatus.Empty && e.KeyCode == Keys.I) // if its empty then show message and wait for second key
                            {
                                waitForSecondKey = isIn;
                            }
                            // check the status of the field if growing to fert
                            else if ((e.KeyCode == Keys.F || e.KeyCode == Keys.I) && (meta.Status == CropStatus.Planted || meta.Status == CropStatus.Stage1 || meta.Status == CropStatus.Stage2 || meta.Status == CropStatus.Stage3))
                            {
                                if (meta.FertAmount == 0)
                                {
                                    int minus = (int)(dictFieldsData[isIn].Size * rn.Next(250, 350)); // see if we can cost the player fert
                                    if (resources["Compost"] >= minus)
                                    {
                                        resources["Compost"] -= minus; // yes, cost the player fert
                                        dictFieldsData[isIn].FertAmount++; // incress the amount
                                        dictBg[isIn].Changecolor(CropColors.Fert1); // changes the color
                                        dictFieldsData[isIn].FertOnStage = meta.Status; // sets the on fert stage
                                    }
                                    else
                                    {
                                        importantMess = $"Not Enought \nCompost ({minus}L)";
                                    }

                                }
                                else if (meta.FertAmount == 1 && meta.FertOnStage != meta.Status) // also checks if its a new stage
                                {
                                    int minus = (int)(dictFieldsData[isIn].Size * rn.Next(250, 350)); // see if we can cost the player fert
                                    if (resources["Compost"] >= minus)
                                    {
                                        resources["Compost"] -= minus; // yes, cost the player fert
                                        dictFieldsData[isIn].FertAmount++; // incress the amount
                                        dictBg[isIn].Changecolor(CropColors.Fert2); // changes the color
                                        dictFieldsData[isIn].FertOnStage = meta.Status; // sets the on fert stage
                                    }
                                    else
                                    {
                                        importantMess = $"Not Enought \nCompost ({minus}L)";
                                    }
                                }
                            }
                            else if ((e.KeyCode == Keys.H || e.KeyCode == Keys.I) && meta.Status == CropStatus.Ready) // harvest
                            {
                                int add = (int)(dictFieldsData[isIn].Size * rn.Next(550, 650)); // create the harvest amount
                                add += (int)(add * meta.FertAmount * 0.5); // add the fert percent
                                if (ttruck == 2) add += (int)(add * 0.6); // add the harvester amount if harvester
                                if (meta.Type == CropType.Wheat) resources["Wheat"] += add; // if wheat
                                else if (meta.Type == CropType.Barley) resources["Barley"] += add; // if barley
                                else if (meta.Type == CropType.Oats) resources["Oats"] += add; // if oats
                                dictBg[isIn].Changecolor(CropColors.Dirt); // set the crop color
                                dictFieldsData[isIn].Status = CropStatus.Empty; // set the crop status
                                dictFieldsData[isIn].Type = CropType.None; // set the crop type
                                dictFieldsData[isIn].FertAmount = 0; // set the fert amount
                            }
                            // selling the field
                            else if (e.KeyCode == Keys.V)
                            {
                                resources["Dollars"] += (int)(meta.Size * 1250); // return the monney back to the player
                                dictFieldsData[isIn].Owner = NewName(); // change the owner the a new random name
                                importantMess = $"Field Sold \nCost {(int)(meta.Size * 1250)}$";
                                // start dirt timer
                                if (dictFieldsData[isIn].Status == CropStatus.Empty)
                                {
                                    string local = isIn;
                                    // set timer to plant
                                    dictTimer[local].Interval = rn.Next(10, 60) * 1000; // make a new random interval
                                    dictTimer[local].Tick += delegate { StartPlantAction(local); }; // setting for the next timer 
                                    dictTimer[local].Start(); // start the timer
                                }
                            }
                        }
                        else if (e.KeyCode == Keys.B || e.KeyCode == Keys.I) // buying the field
                        {
                            int cost = (int)(meta.Size * 1250);
                            if (e.KeyCode == Keys.I)
                            {
                                importantMess = $"Field Not Owned \nCost {(int)(meta.Size * 1250)}$";
                            }
                            else if (resources["Dollars"] >= cost) // see if we can cost the player monney
                            {
                                resources["Dollars"] -= cost; // yes, cost the player monney
                                dictFieldsData[isIn].Owner = "You"; // make owner
                                importantMess = $"Field Bought \nCost {(int)(meta.Size * 1250)}$";
                            }
                            else
                            {
                                importantMess = $"Not Enought \nDollars ({cost}$)";
                            }
                        }
                    }
                    else if (value == "building")
                    {
                        // make a smaller, easyer to acsess varable
                        BuildingMetadata meta = dictBuildingData[isIn];
                        if (meta.Business == "Selling" && (e.KeyCode == Keys.I || e.KeyCode == Keys.V)) // for selling any crop
                        {
                            int add = (int)(resources[meta.Type] * 0.001f * rn.Next(650, 750)); // make the add about
                            resources["Dollars"] += add; // add it to resouses
                            resources[meta.Type] = 0; // empty the resouses used
                            importantMess = $"{meta.Type} Sold \nProfit ({add}$)";
                        }
                        else if (meta.Business == "Buying" && (e.KeyCode == Keys.I || e.KeyCode == Keys.B)) // for selling any crop
                        {
                            int minus = 0; // make a varable
                            if (meta.Type == "Seeds") minus = rn.Next(450, 550); // make the minus about
                            if (meta.Type == "Compost") minus = rn.Next(400, 500); // make the minus about
                            if (resources["Dollars"] >= minus) // see if we can cost the player monney
                            {
                                resources["Dollars"] -= minus; // minus it from resouses
                                resources[meta.Type] += 1000; // add to the resouses used
                                importantMess = $"{meta.Type} Bought \nCost ({minus}$)";
                            }
                            else
                            {
                                importantMess = $"Not Enought \nDollars ({minus}$)";
                            }
                        }
                        else if (meta.Business == "Painting")
                        {
                            Controls.Add(painting);
                        }
                        else if (meta.Business == "Dealing")
                        {
                            Controls.Add(dealing);
                        }
                        else if (meta.Business == "Gas")
                        {
                            tmrGasF.Start();
                        }
                    }
                }
                else if (waitForSecondKey != null)
                {
                    // check for the coraspwanding second key
                    if (e.KeyCode == Keys.J)
                    {
                        waitForSecondKey = null; // reset the var
                        int minus = (int)(dictFieldsData[isIn].Size * rn.Next(450, 550)); // see if we can cost the player seeds
                        if (resources["Seeds"] >= minus)
                        {
                            resources["Seeds"] -= minus; // yes, cost the player seeds
                            string n = isIn; // make the isIn var local
                            dictBg[isIn].Changecolor(CropColors.Planted); // set the crop color
                            dictFieldsData[isIn].Status = CropStatus.Planted; // set the crop status
                            dictFieldsData[isIn].Type = CropType.Wheat; // set the crop type
                            dictTimer[isIn].Interval = rn.Next(25, 36) * 1000; // make a new random interval
                            dictTimer[isIn].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage1, new Color[] { CropColors.Wheat1, CropColors.Wheat2, CropColors.Wheat3, CropColors.WheatReady }, CropType.Wheat); }; // set the tick
                            dictTimer[isIn].Start(); // start the timer
                        }
                        else
                        {
                            importantMess = $"Not Enought \nSeeds ({minus}L)";
                        }
                    }
                    else if (e.KeyCode == Keys.K)
                    {
                        waitForSecondKey = null; // reset the var
                        int minus = (int)(dictFieldsData[isIn].Size * rn.Next(450, 550)); // see if we can cost the player seeds
                        if (resources["Seeds"] >= minus)
                        {
                            resources["Seeds"] -= minus; // yes, cost the player seeds
                            string n = isIn;
                            dictBg[isIn].Changecolor(CropColors.Planted); // set the crop color
                            dictFieldsData[isIn].Status = CropStatus.Planted; // set the crop status
                            dictFieldsData[isIn].Type = CropType.Barley; // set the crop type
                            dictTimer[isIn].Interval = rn.Next(25, 36) * 1000; // make a new random interval
                            dictTimer[isIn].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage1, new Color[] { CropColors.Barley1, CropColors.Barley2, CropColors.Barley3, CropColors.BarleyReady }, CropType.Barley); }; // set the tick
                            dictTimer[isIn].Start(); // start the timer
                        }
                        else
                        {
                            importantMess = $"Not Enought \nSeeds ({minus}L)";
                        }
                    }
                    else if (e.KeyCode == Keys.L)
                    {
                        waitForSecondKey = null; // reset the var
                        int minus = (int)(dictFieldsData[isIn].Size * rn.Next(450, 550)); // see if we can cost the player seeds
                        if (resources["Seeds"] >= minus)
                        {
                            resources["Seeds"] -= minus; // yes, cost the player seeds
                            string n = isIn;
                            dictBg[isIn].Changecolor(CropColors.Planted); // set the crop color
                            dictFieldsData[isIn].Status = CropStatus.Planted; // set the crop status
                            dictFieldsData[isIn].Type = CropType.Oats; // set the crop type
                            dictTimer[isIn].Interval = rn.Next(25, 36) * 1000; // make a new random interval
                            dictTimer[isIn].Tick += delegate (object sende, EventArgs er) { FieldAction(n, CropStatus.Stage1, new Color[] { CropColors.Oat1, CropColors.Oat2, CropColors.Oat3, CropColors.OatReady }, CropType.Oats); }; // set the tick
                            dictTimer[isIn].Start(); // start the timer
                        }
                        else
                        {
                            importantMess = $"Not Enought \nSeeds ({minus}L)";
                        }
                    }
                }

                if (importantMess != null && !tmrImportantMess.Enabled) tmrImportantMess.Start(); // reset the important message
                Refresh();

            }
        }
        private void TmrKeyTimer_Tick(object sender, EventArgs e)
        {
            // reset the waitForSecondKey var if the the player left the field
            if (isIn == null && waitForSecondKey != null) waitForSecondKey = null;

            // set the speed of the player based of the truck
            if (ttruck == 0) tspeed = TRUCK_SPEED;
            else if (ttruck == 1) tspeed = VAN_SPEED;
            else if (ttruck == 2) tspeed = HARVESTER_SPEED;

            #region Key down event
            if (start && Focused) // keydown
            {
                // game contorls WASD
                if (Keyboard.IsKeyDown(Key.A)) wasdPressed[0] = true;
                if (Keyboard.IsKeyDown(Key.S)) wasdPressed[1] = true;
                if (Keyboard.IsKeyDown(Key.D)) wasdPressed[2] = true;
                if (Keyboard.IsKeyDown(Key.W)) wasdPressed[3] = true;

                // game contorls ARROW
                if (Keyboard.IsKeyDown(Key.Left)) arrowPressed[0] = true;
                if (Keyboard.IsKeyDown(Key.Down)) arrowPressed[1] = true;
                if (Keyboard.IsKeyDown(Key.Right)) arrowPressed[2] = true;
                if (Keyboard.IsKeyDown(Key.Up)) arrowPressed[3] = true;
            }
            #endregion
            #region Action for arrow
            if (arrowPressed[3]) // move forword
            {
                float ax;
                float ay;
                float incerment = tspeed / 90f;

                Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 0]; // reset the defalt image
                if (arrowPressed[0] && arrowPressed[2]) // if trying to trun both ways then make it stright
                {
                    Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 0]; // change to stright pic
                }
                else if (arrowPressed[0]) // trun left
                {
                    angle += -tspeed / 2;
                    Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 1]; // change to truning pic
                }
                else if (arrowPressed[2]) // trun right
                {
                    angle += tspeed / 2;
                    Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 2]; // change to truning pic
                }

                if (angle <= 90) // up
                {
                    ay = tspeed; // starting y
                    ax = incerment * angle; // make the x move amount based off the angle
                    ay -= (incerment * angle); // subtract the y by the angle

                    MakeMove(ay, Way.Down);
                    MakeMove(ax, Way.Left);
                }
                else if (angle <= 180) // right
                {
                    angle -= 90;
                    ax = tspeed; // starting x
                    ay = incerment * angle; // make the y move amount based off the angle
                    ax -= (incerment * angle); // subtract the x by the angle
                    angle += 90;
                    MakeMove(ax, Way.Left);
                    MakeMove(ay, Way.Up);
                }
                else if (angle <= 270) // down
                {
                    angle -= 180;
                    ay = tspeed; // starting y
                    ax = incerment * angle; // make the x move amount based off the angle
                    ay -= (incerment * angle); // subtract the y by the angle
                    angle += 180;
                    MakeMove(ay, Way.Up);
                    MakeMove(ax, Way.Right);
                }
                else if (angle <= 360) // left
                {
                    angle -= 270;
                    ax = tspeed; // starting x
                    ay = incerment * angle; // make the y move amount based off the angle
                    ax -= (incerment * angle); // subtract the x by the angle
                    angle += 270;
                    MakeMove(ax, Way.Right);
                    MakeMove(ay, Way.Down);
                }
            }
            else if (arrowPressed[1]) // move backword
            {
                float ax;
                float ay;
                float incerment = tspeed / 90f;

                Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 0]; // reset the defalt image
                if (arrowPressed[0] && arrowPressed[2]) // if trying to trun both ways then make it stright
                {
                    Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 0]; // change to stright pic
                }
                else if (arrowPressed[0]) // trun left
                {
                    angle += tspeed / 2;
                    Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 1]; // change to truning pic
                }
                else if (arrowPressed[2]) // trun right
                {
                    angle += -tspeed / 2;
                    Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 2]; // change to truning pic
                }

                if (angle <= 90) // up
                {
                    ay = tspeed; // starting y
                    ax = incerment * angle; // make the x move amount based off the angle
                    ay -= (incerment * angle); // subtract the y by the angle

                    MakeMove(ay, Way.Up);
                    MakeMove(ax, Way.Right);
                }
                else if (angle <= 180) // right
                {
                    angle -= 90;
                    ax = tspeed; // starting x
                    ay = incerment * angle; // make the y move amount based off the angle
                    ax -= (incerment * angle); // subtract the x by the angle
                    angle += 90;
                    MakeMove(ax, Way.Right);
                    MakeMove(ay, Way.Down);
                }
                else if (angle <= 270) // down
                {
                    angle -= 180;
                    ay = tspeed; // starting y
                    ax = incerment * angle; // make the x move amount based off the angle
                    ay -= (incerment * angle); // subtract the y by the angle
                    angle += 180;
                    MakeMove(ay, Way.Down);
                    MakeMove(ax, Way.Left);
                }
                else if (angle <= 360) // left
                {
                    angle -= 270;
                    ax = tspeed; // starting x
                    ay = incerment * angle; // make the y move amount based off the angle
                    ax -= (incerment * angle); // subtract the x by the angle
                    angle += 270;
                    MakeMove(ax, Way.Left);
                    MakeMove(ay, Way.Up);
                }
            }
            else if (arrowPressed[0] && arrowPressed[2]) // if trying to trun both ways then make it stright
            {
                Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 0]; // change to stright pic
            }
            else if (arrowPressed[0]) // trun left
            {
                Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 1]; // change to truning pic
            }
            else if (arrowPressed[2]) // trun right
            {
                Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 2]; // change to truning pic
            }
            #endregion
            #region Action for WASD
            // you can press WASD + Up to go double speed, its not a bug
            if (wasdPressed[0]) // left
            {
                angle = 270;
                if (wasdPressed[1]) // down
                {
                    angle += -45;
                    MakeMove(tspeed / 2, Way.Right);
                    MakeMove(tspeed / 2, Way.Up);
                }
                else if (wasdPressed[3]) // up
                {
                    angle += 45;
                    MakeMove(tspeed / 2, Way.Right);
                    MakeMove(tspeed / 2, Way.Down);
                }
                else // left
                    MakeMove(tspeed, Way.Right);
            }
            else if (wasdPressed[1]) // down
            {
                angle = 180;
                if (wasdPressed[2]) // right
                {
                    angle += -45;
                    MakeMove(tspeed / 2, Way.Up);
                    MakeMove(tspeed / 2, Way.Left);
                }
                else if (wasdPressed[0]) // left
                {
                    angle += 45;
                    MakeMove(tspeed / 2, Way.Up);
                    MakeMove(tspeed / 2, Way.Right);
                }
                else // down
                    MakeMove(tspeed, Way.Up);
            }
            else if (wasdPressed[2]) // right
            {
                angle = 90;
                if (wasdPressed[3]) // up
                {
                    angle += -45;
                    MakeMove(tspeed / 2, Way.Left);
                    MakeMove(tspeed / 2, Way.Down);
                }
                else if (wasdPressed[1]) // down
                {
                    angle += 45;
                    MakeMove(tspeed / 2, Way.Left);
                    MakeMove(tspeed / 2, Way.Up);
                }
                else // right
                    MakeMove(tspeed, Way.Left);
            }
            else if (wasdPressed[3]) // up
            {
                angle = 0;
                if (wasdPressed[0]) // left
                {
                    angle += -45;
                    MakeMove(tspeed / 2, Way.Down);
                    MakeMove(tspeed / 2, Way.Right);
                }
                else if (wasdPressed[2]) // right
                {
                    angle += 45;
                    MakeMove(tspeed / 2, Way.Down);
                    MakeMove(tspeed / 2, Way.Left);
                }
                else // up
                    MakeMove(tspeed, Way.Down);
            }
            #endregion
            // reset the angle if over 360 or less than 0
            if (angle >= 360) angle = 0;
            if (angle < 0) angle = 360;
            // set a bool to check if i need to refresh the screen, run a for loop with both keys pressed varables list
            bool paint = false;
            for (int i = 0; i < wasdPressed.Length; i++) if (wasdPressed[i]) { paint = true; Player[ttruck].Image = VAkal[ttruck, tcolor[ttruck], 0]; }
            for (int i = 0; i < arrowPressed.Length; i++) if (arrowPressed[i]) paint = true;
            // if any keys pressed then refresh
            if (paint) { Player[ttruck].RotatePic(angle); Refresh(); }
            // if moving lose gas
            if (paint)
            {
                int GL = rn.Next(0, 200);
                if (GL == 0)
                {
                    resources["Diesel"]--;
                    if (resources["Diesel"] <= 0) { MessageBox.Show("Game over"); start = false; }
                }
            }
        }
        #endregion
    }
}
