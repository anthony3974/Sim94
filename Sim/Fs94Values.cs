using System.Drawing;

// my enums
public enum CropType { None, Wheat, Barley, Oats, }
// empty is dirt color, no timer
// planted is green,brown, 30 sec timer
// stage1 is light (crop color), 40% of random, individual crop timer
// stage2 is medium (crop color), 35% of random, individual crop timer
// stage3 is dark (crop color), 25% of random, individual crop timer
// ready is yellow, orange, no time limit
// total min time = 160 seconds; total max time = 205 seconds
public enum CropStatus { Empty, Planted, Stage1, Stage2, Stage3, Ready, }

namespace Fs94Values
{
    class CropColors
    {
        public static Color Dirt { get { return Color.FromArgb(130, 90, 60); } }
        public static Color Planted { get { return Color.FromArgb(50, 230, 30); } }
        public static Color Wheat1 { get { return Color.FromArgb(195, 215, 50); } }
        public static Color Wheat2 { get { return Color.FromArgb(215, 195, 50); } }
        public static Color Wheat3 { get { return Color.FromArgb(235, 200, 5); } }
        public static Color WheatReady { get { return Color.FromArgb(255, 190, 0); } }
        public static Color Barley1 { get { return Color.FromArgb(170, 180, 35); } }
        public static Color Barley2 { get { return Color.FromArgb(210, 195, 30); } }
        public static Color Barley3 { get { return Color.FromArgb(190, 140, 30); } }
        public static Color BarleyReady { get { return Color.FromArgb(150, 110, 25); } }
        public static Color Oat1 { get { return Color.FromArgb(195, 200, 35); } }
        public static Color Oat2 { get { return Color.FromArgb(200, 180, 25); } }
        public static Color Oat3 { get { return Color.FromArgb(215, 160, 50); } }
        public static Color OatReady { get { return Color.FromArgb(230, 130, 40); } }
        public static Color Fert1 { get { return Color.FromArgb(100, 185, 235); } }
        public static Color Fert2 { get { return Color.FromArgb(80, 155, 235); } }

    }

    class FieldMetadata
    {
        private string name;
        private string owner;
        private float size;
        private CropType type;
        private CropStatus status;
        private int fertAmount;
        private CropStatus fertOnStage;
        #region Main class funtions

        // construster
        public FieldMetadata(string name, string owner, float size, CropType type, CropStatus status, int fertAmount)
        {
            // yieldRange is 2 values
            // color is 3 values
            this.name = name;
            this.owner = owner;
            this.size = size;
            this.type = type;
            this.status = status;
            this.fertAmount = fertAmount;

        }
        // getter/setter
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        public float Size
        {
            get { return size; }
            set { this.size = value; }
        }
        public CropType Type
        {
            get { return type; }
            set { type = value; }
        }
        public CropStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        public int FertAmount
        {
            get { return fertAmount; }
            set { fertAmount = value; }
        }
        public CropStatus FertOnStage
        {
            get { return fertOnStage; }
            set { fertOnStage = value; }
        }

        // End class
        #endregion
    }

    class BuildingMetadata
    {
        private string name;
        private string business;
        private string type;
        #region Main class funtions

        // construster
        public BuildingMetadata(string name, string business, string type = null)
        {
            this.name = name;
            this.business = business;
            this.type = type;
        }
        // getter/setter
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Business
        {
            get { return business; }
            set { business = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        // End class
        #endregion
    }
}
