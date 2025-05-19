using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.GameLogic
{
    public class InitializePlayerMarker
    {
        private Panel[] panels;
        private Dictionary<int, Panel> playerMarkers;
        private MainForm MainForm;
        // Khi sử dụng Action, nhớ kiểm tra null:
        public InitializePlayerMarker(Panel[] panels, Dictionary<int, Panel> playerMarkers, MainForm mainForm)
        {
            this.panels = panels;
            this.playerMarkers = playerMarkers;
            MainForm = mainForm;
        }
        public void InitializePlayerMarkerUI(Player player)
        {
            Panel marker = new Panel
            {
                Size = new Size(24, 24),
                BackColor = player.Color,
                Name = $"player{player.ID}Marker"
            };
            // Tạo hình tròn
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, marker.Width, marker.Height);
            marker.Region = new Region(path);
            var tilePanel = panels[player.TileIndex];
            var tile = tilePanel?.Tag as Tile;
            if (tilePanel != null)
            {
                // Thêm sự kiện Paint để vẽ lại khi cần
                marker.Paint += (sender, e) => {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (var brush = new SolidBrush(player.Color))
                    { // chỉnh màu
                        e.Graphics.FillEllipse(brush, 0, 0, marker.Width, marker.Height);
                    }
                };
                Point position = CalculateMarkerPosition(tilePanel, tile!, player.ID);
                marker.Location = position;
                tilePanel.Controls.Add(marker);
                marker.BringToFront();
                playerMarkers[player.ID] = marker;
            }
        }
        public void UpdatePlayerMarkerPosition(Player player, int newIndex)
        {
            MovePlayerToTile(player, newIndex, true);
        }
        public async Task MovePlayerStepByStep(Player player, int steps, int totalTiles)
        {
            for (int i = 1; i <= steps; i++)
            {
                int nextIndex = (player.TileIndex + 1) % totalTiles;
                bool isLastStep = (i == steps);
                MovePlayerToTile(player, nextIndex, isLastStep); // chỉ hiện form ở bước cuối
                await Task.Delay(200);
            }
        }
        private Point CalculateMarkerPosition(Panel tilePanel, Tile tile, int playerId)
        {
            const int markerSize = 24;
            int offsetX = 0;
            int offsetY = 0;
            switch (playerId)
            {
                case 1: // Player 1: trên trái
                    offsetX = -20;
                    offsetY = -15;
                    break;
                case 2: // Player 2: trên phải
                    offsetX = 20;
                    offsetY = -15;
                    break;
                case 3: // Player 3: dưới trái
                    offsetX = -20;
                    offsetY = 15;
                    break;
                case 4: // Player 4: dưới phải
                    offsetX = 20;
                    offsetY = 15;
                    break;
            }
            int centerX = (tilePanel.Width - markerSize) / 2 + offsetX;
            int centerY = (tilePanel.Height - markerSize) / 2 + offsetY;
            return new Point(centerX, centerY);
        }
        private void MovePlayerToTile(Player player, int tileIndex, bool showAction)
        {
            if (playerMarkers.TryGetValue(player.ID, out var marker))
            {
                if (player.TileIndex >= 0 && player.TileIndex < panels.Length)
                {
                    panels[player.TileIndex].Controls.Remove(marker);
                }

                player.TileIndex = tileIndex;
                var newTilePanel = panels[tileIndex];
                var tile = newTilePanel?.Tag as Tile;

                if (newTilePanel != null)
                {
                    Point newPosition = CalculateMarkerPosition(newTilePanel, tile!, player.ID);
                    newTilePanel.Controls.Add(marker);
                    marker.Location = newPosition;
                    marker.BringToFront();
                    if (tile != null && showAction) MainForm.ShowTileActionForm(tile, player);
                }
            }
        }
    }
}
