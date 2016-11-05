using B_ESA_4.Forms;
using B_ESA_4.Pawn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B_ESA_4.Forms
{
    public partial class frmLabyrinthGame : Form
    {
        const int FRAMES_PER_SECOND = 60;
        const int ONE_SECOND = 1000;
        const int WS_EX_COMPOSITE_ON = 0x02000000;
        const string AUTOMATIK = "Automatik";
        const string MANUAL = "Manuell";
        System.Windows.Forms.Timer renderTimer;
        DataLoader internalDataLoader;
        PlayGround interalPlayground;
        IPawn internalPawn;
        string internalPathToFile;

        public frmLabyrinthGame(string pathToFile)
        {
            InitializeComponent();
            internalDataLoader = new DataLoader();
            if (!String.IsNullOrWhiteSpace(pathToFile))
            {
                internalPathToFile = pathToFile;
                setLabyrinth();
            }

            renderTimer = new Timer()
            {
                Interval = ONE_SECOND / FRAMES_PER_SECOND,
                Enabled = true
            };
            renderTimer.Tick += OnRenderFrame;
        }

        private void OnRenderFrame(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_COMPOSITE_ON;
                return cp;
            }
        }

        private void frmLabyrinthGame_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    internalPawn.MoveLeft();
                    break;
                case Keys.Up:
                    internalPawn.MoveUp();
                    break;
                case Keys.Right:
                    internalPawn.MoveRight();
                    break;
                case Keys.Down:
                    internalPawn.MoveDown();
                    break;
                default:
                    break;
            }
        }

        private void automatikToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = ((PawnBase)internalPawn).PawnX;
            int y = ((PawnBase)internalPawn).PawnY;

            internalPawn?.Dispose();

            if (automatikToolStripMenuItem.Text == AUTOMATIK)
            {             
                internalPawn = new ComputerPlayer(interalPlayground, x, y);
                automatikToolStripMenuItem.Text = MANUAL;
            }
            else
            {
                internalPawn = new ManualMovingPawn(interalPlayground, x, y);
                automatikToolStripMenuItem.Text = AUTOMATIK;
            }            
        }

        private void labyrinthLadenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                internalPathToFile = openDialog.FileName;
            }
            setLabyrinth();
        }

        private void frmLabyrinthGame_Paint(object sender, PaintEventArgs e)
        {
            interalPlayground?.DrawLab(e.Graphics);
        }

        private void autorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmAutor().ShowDialog();
        }

        private void resetLabyrinthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setLabyrinth();
        }

        private void setLabyrinth()
        {
            var lab = internalDataLoader.LoadDataFromFile(internalPathToFile);
            interalPlayground = new PlayGround(lab);
            this.Height = interalPlayground.Height;
            this.Width = interalPlayground.Width;
            internalPawn = new ManualMovingPawn(interalPlayground);
            automatikToolStripMenuItem.Text = AUTOMATIK;
        }

        private void hilfeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmHelp().ShowDialog();
        }
    }
}
