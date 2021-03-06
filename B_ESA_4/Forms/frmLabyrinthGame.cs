﻿using B_ESA_4.Common;
using B_ESA_4.Pawn;
using System;
using System.Drawing;
using System.Windows.Forms;
using B_ESA_4.Playground;
using System.Collections.Generic;
using System.Linq;

namespace B_ESA_4.Forms
{
    public partial class FrmLabyrinthGame : Form
    {
        const int FONT_SIZE = 25;
        const int FRAMES_PER_SECOND = 60;
        const int WS_EX_COMPOSITE_ON = 0x02000000;
        const string AUTOMATIK = "Automatik";
        const string MANUAL = "Manuell";
        const string FILE_OPEN_FILTER = "Labyrinth File (*.dat) |*.dat";

        readonly Timer _renderTimer;
        readonly DataLoader _dataLoader;
        PlayGround _playground;
        private Score _score;
        IPawn _pawn;
        string _pathToFile;
        private PlaygroundRenderer _playgroundRenderer;

        public FrmLabyrinthGame(string pathToFile)
        {
            InitializeComponent();
            _dataLoader = new DataLoader();
            _pathToFile = pathToFile;
            SetLabyrinth();

            _renderTimer = new Timer()
            {
                Interval = CommonConstants.ONE_SECOND / FRAMES_PER_SECOND,
                Enabled = true
            };
            _renderTimer.Tick += OnRenderFrame;
        }

        private void OnRenderFrame(object sender, EventArgs e)
        {
            Invalidate();
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
                    _pawn.MoveLeft();
                    break;
                case Keys.Up:
                    _pawn.MoveUp();
                    break;
                case Keys.Right:
                    _pawn.MoveRight();
                    break;
                case Keys.Down:
                    _pawn.MoveDown();
                    break;
                default:
                    break;
            }
        }

        private void automatikToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_pawn != null)
            {
                _pawn.Dispose();

                if (automatikToolStripMenuItem.Text == AUTOMATIK)
                {
                    _pawn = new ComputerPlayer(_playground);
                    automatikToolStripMenuItem.Text = MANUAL;
                }
                else
                {
                    _pawn = new ManualMovingPawn(_playground);
                    automatikToolStripMenuItem.Text = AUTOMATIK;
                }
            }
        }

        private void labyrinthLadenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = FILE_OPEN_FILTER;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                _pathToFile = openDialog.FileName;
            }
            SetLabyrinth();
        }

        private void frmLabyrinthGame_Paint(object sender, PaintEventArgs e)
        {
            _playgroundRenderer?.UpDatePlayground(e.Graphics);
            if (_playground != null && _score != null)
            {
                tlStrpPoints.Text = _score.Points.ToString();
                tlStrpSteps.Text = _score.Steps.ToString();
            }
        }

        private void hilfeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmHelp().ShowDialog();
        }

        private void neuStartenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLabyrinth();
        }
        private void autorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmAutor().Show();
        }

        private void frmLabyrinthGame_Load(object sender, EventArgs e)
        {
            Icon = Icon.GetKepplerIcon(Application.StartupPath);
        }

        private void SetLabyrinth()
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(_pathToFile))
                {
                    var lab = _dataLoader.LoadDataFromFile(_pathToFile);
                    if (lab != null)
                    {
                        _score?.Dispose();
                        _score = new Score();

                        _playground = new PlayGround(lab);
                        _playgroundRenderer = new PlaygroundRenderer(_playground);
                        _pawn = new ManualMovingPawn(_playground);
                        automatikToolStripMenuItem.Text = AUTOMATIK;
                        Height = _playgroundRenderer.Size.Height;
                        Width = _playgroundRenderer.Size.Width;
                        return;
                    }
                }
                _playgroundRenderer = new PlaygroundRenderer(this.Size);
            }
            catch (PawnMissingException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (InvalidFormatException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
