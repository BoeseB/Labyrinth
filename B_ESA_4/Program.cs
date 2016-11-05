﻿using B_ESA_4.Forms;
using System;
using System.Windows.Forms;

namespace B_ESA_4
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args != null && args.Length > 0)
            {
                Application.Run(new frmLabyrinthGame(args[0]));
            }
            else
            {
                MessageBox.Show("Es wurde kein Pfad angegeben!\nProgramm wird beendet.");
            }
        }
    }
}
