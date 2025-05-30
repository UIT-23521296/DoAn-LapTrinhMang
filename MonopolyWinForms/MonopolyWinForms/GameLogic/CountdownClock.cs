using System;
using System.Drawing;
using System.Windows.Forms;

public class CountdownClock
{
    private System.Windows.Forms.Timer timer;
    private TimeSpan remainingTime;
    private Label displayLabel;
    private Panel targetPanel;
    private Action onTimeUp;

    public CountdownClock(Panel panel, Action onTimeUpCallback)
    {
        timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000;
        timer.Tick += Timer_Tick;
        this.onTimeUp = onTimeUpCallback;
        this.targetPanel = panel;
        displayLabel = new Label
        {
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = Color.Black,
            AutoSize = true,
        };
        displayLabel.Location = new Point(
            (panel.Width - displayLabel.Width) / 3,
            (panel.Height - displayLabel.Height) *2/5
        );
        panel.Controls.Add(displayLabel);
    }
    public void Start(int minutes)
    {
        remainingTime = TimeSpan.FromMinutes(minutes);
        UpdateLabel();
        timer.Start();
    }
    public void Stop()
    {
        timer.Stop();
    }
    private void Timer_Tick(object? sender, EventArgs e)
    {
        remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
        UpdateLabel();

        if (remainingTime.TotalSeconds <= 0)
        {
            timer.Stop();
            onTimeUp?.Invoke();
        }
    }
    private void UpdateLabel()
    {
        displayLabel.Text = "⏳: " + remainingTime.ToString(@"mm\:ss");
    }
}
