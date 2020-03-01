using System;
using System.Drawing;
using System.Windows.Forms;
using DrawGrid;
using System.IO;

/* Jathurshan Theivikaran
 * December 2019
 * this program solves a maze that the user inputs in a specific format
 */

//sample mazes are in the folder of this solution
namespace MazeWin
{
    public partial class frmMain : Form
    {
        //private class variables
        char[,] Maze;
        int rows;
        int cols;
        Grid grid;

        public frmMain()
        {
            InitializeComponent();

            //init any global variables
            rows = 0;
            cols = 0;
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            //get file from Open Dialog box
            OpenFileDialog fd = new OpenFileDialog();

            //don't forget error checking

            if (fd.ShowDialog() == DialogResult.OK)
            {
                //load the file
                StreamReader sr = new StreamReader(fd.OpenFile());

                rows = int.Parse(sr.ReadLine());
                cols = int.Parse(sr.ReadLine());
                //initialize the Maze array
                Maze = new char[rows, cols];
                grid = new Grid(new Size(cols, rows), 20, new Point(20, 20));

                //populate the Maze array
                for (int r = 0; r < rows; r++)
                {
                    string line = sr.ReadLine();
                    for (int c = 0; c < cols; c++)
                    {
                        Maze[r, c] = line[c];
                    }
                }

                //configure grid so each cell is drawn properly
                ConfigureGrid();

                //resize form to grid height and width
                this.Width = this.cols * grid.CellSize + 60;
                this.Height = this.rows * grid.CellSize + 80;

                //tell form to redraw
                this.Refresh();
            }
        }//mnuFileOpen Click method

        private void ConfigureGrid()
        {
            for (int r = 0; r < this.rows; r++)
            {
                for (int c = 0; c < this.cols; c++)
                {
                    //change colour of cell depending on what
                    //is in it
                    if (Maze[r, c] == 'S') grid.GetCell(r, c).BackColor = Color.Purple;
                    else if (Maze[r, c] == 'E') grid.GetCell(r, c).BackColor = Color.Blue;
                    else if (Maze[r, c] == '#') grid.GetCell(r, c).BackColor = Color.Gray;
                    else if (Maze[r, c] == '.') grid.GetCell(r, c).BackColor = Color.White;
                    else if (Maze[r, c] == '+') grid.GetCell(r, c).BackColor = Color.Green;
                    else if (Maze[r, c] == 'X') grid.GetCell(r, c).BackColor = Color.Yellow;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (grid != null)
                grid.Draw(e.Graphics);
        }

        private bool SolveMaze(int r, int c)
        {
            // if out of bounds of maze, return false
            if (r < 0 || r >= rows || c < 0 || c >= cols) return false;
            // if curr maze location is E, return true
            if (Maze[r, c] == 'E')
            {
                Maze[r, c] = '+';
                return true;
            }

            // if you cannot move on the following square, return false
            // you have been on, not a wall
            if (Maze[r, c] != '.' && Maze[r, c] != 'S') return false;

            Maze[r, c] = '+';
            // if you can, set to +

            // call solvemaze
            // if solvemaze to the right (all directions), return true
            if (SolveMaze(r + 1, c)) return true;
            if (SolveMaze(r, c + 1)) return true;
            if (SolveMaze(r - 1, c)) return true;
            if (SolveMaze(r, c - 1)) return true;
            // else return false and change to X

            Maze[r, c] = 'X';
            return false;
        }

        private void mnuFileSolve_Click(object sender, EventArgs e)
        {
            //solve maze
            SolveMaze(0, 0);
            ConfigureGrid();

            this.Refresh();
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}