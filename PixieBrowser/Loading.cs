using System.Windows.Forms;

namespace PixivScooper
{
    public partial class Loading : Form
    {
        private int currentProgress;
        private int maxProgress;
        public Loading(int barSize, string formName)
        {
            currentProgress = 0;
            maxProgress = barSize;
            InitializeComponent();
            this.Text = formName;
            loadingStatus.Maximum = maxProgress;
        }
        public void processValue()
        {
            currentProgress++;
            loadingStatus.Value = currentProgress;
            this.Update();
        }
        public void manualScroll(int process)
        {
            loadingStatus.Value = process;
            this.Update();
        }
    }
}
