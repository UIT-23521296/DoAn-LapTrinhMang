using System.Diagnostics.Eventing.Reader;

namespace MonopolyWinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ClientSize = new Size(1140, 800);

            CreateBoardTiles();
            PositionTiles();
        }

        private List<Panel> tiles = new List<Panel>();

        private void CreateBoardTiles()
        {
            for (int i = 0; i < 40; i++)
            {
                Panel tile = new Panel();
                tile.BorderStyle = BorderStyle.FixedSingle;
                tile.BackColor = Color.White;

                // Góc: 0, 10, 20, 30
                if (i % 10 == 0)
                {
                    tile.Size = new Size(120, 120); // Góc vuông
                }
                else if((i < 10) || (i > 20) && (i < 30))
                {
                    tile.Size = new Size(120, 60); // Các ô còn lại hình chữ nhật
                }
                else
                {
                    tile.Size = new Size(60, 120);
                }

                Label label = new Label();
                label.Text = i.ToString(); // sau này sẽ là tên đất
                label.Dock = DockStyle.Fill;
                label.TextAlign = ContentAlignment.MiddleCenter;
                tile.Controls.Add(label);

                tiles.Add(tile);
                this.Controls.Add(tile);
            }
        }

        private void PositionTiles()
        {
            int boardSize = 780;
            int tileSize = 60;
            int cornerSize = 120;
            this.ClientSize = new Size(1140, 800);

            for (int i = 0; i < 40; i++)
            {
                Panel tile = tiles[i];
                Point pos = new Point();

                // Cột trái: bottom → top
                if (i >= 0 && i <= 10)
                {
                    if (i == 0)
                        pos = new Point(180, boardSize - cornerSize + 10); // Góc dưới trái
                    else if (i == 10)
                        pos = new Point(180, boardSize - cornerSize - (i + 1) * tileSize + 10 ); // Ô số 10
                    else
                        pos = new Point(180, boardSize - cornerSize - i * tileSize + 10); // Các ô liền kề nhau
                }

                // Hàng trên: left → right
                else if (i >= 11 && i <= 20)
                {
                    int offset = i - 11;
                    if (i == 20)
                        pos = new Point(boardSize - cornerSize + 180, 10);
                    else
                        pos = new Point(cornerSize + offset * tileSize + 180, 10);
                }

                // Cột phải: top → bottom
                else if (i >= 21 && i <= 30)
                {
                    int offset = i - 21;
                    if (i == 30)
                        pos = new Point(boardSize - cornerSize + 180, boardSize - cornerSize +10); // Góc dưới phải
                    else
                        pos = new Point(boardSize - cornerSize + 180, cornerSize + offset * tileSize +10);
                }

                // Hàng dưới: right → left
                else
                {
                    int offset = i - 30;
                    pos = new Point(boardSize - cornerSize - offset * tileSize +180, boardSize - cornerSize +10);
                }

                tile.Location = pos;
            }
        }



    }
}
