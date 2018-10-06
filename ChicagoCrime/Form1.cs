﻿//
// GUI app to analyze Chicago crime data, using SQL and ADO.NET
//
// <<Tejash Contractor>>
// U. of Illinois, Chicago
// CS341, Spring 2018
// Project 07
//

using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace ChicagoCrime
{
  public partial class Form1 : Form
  {
     public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      this.clearForm();
    }

    private bool fileExists(string filename)
    {
      if (!System.IO.File.Exists(filename))
      {
        string msg = string.Format("Input file not found: '{0}'",
          filename);

        MessageBox.Show(msg);
        return false;
      }

      // exists!
      return true;
    }

    private void clearForm()
    {
      this.chart.Series.Clear();
      this.chart.Titles.Clear();
      this.chart.Legends.Clear();
    }

    private void cmdByYear_Click(object sender, EventArgs e)
    {
      //
      // Check to make sure database filename in text box actually exists:
      //
      string filename = this.txtFilename.Text;

      if (!fileExists(filename))
        return;

      this.Cursor = Cursors.WaitCursor;

      clearForm();

            //
            // Retrieve data from database:
            //

            //
            // ????????????????
            //

            string version, connectionInfo;
            SqlConnection db;

            version = "MSSQLLocalDB";
            filename = "CrimeDB.mdf";

            connectionInfo = string.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);

            db = new SqlConnection(connectionInfo);
            db.Open();

            string sql = string.Format(@"SELECT Year, Count(*) AS NumCrimes FROM Crimes GROUP BY Year ORDER BY Year ASC");

            MessageBox.Show(sql);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            string msg = db.State.ToString();
            MessageBox.Show(msg);

            db.Close();


            //
            // Build a set of (x,y) points for plotting:
            //
            List<int> X = new List<int>();
            List<int> Y = new List<int>();

            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                X.Add(Convert.ToInt32(row["Year"]));
                Y.Add(Convert.ToInt32(row["NumCrimes"]));
            }

            //
            // now graph as a line chart:
            //
            this.chart.Titles.Add("Total # of Crimes Per Year");

      var series = this.chart.Series.Add("total # of crimes");

      series.ChartType = SeriesChartType.Line;

      for (int i = 0; i < X.Count; ++i)
      {
        series.Points.AddXY(X[i], Y[i]);
      }

      var legend = new Legend();
      legend.Docking = Docking.Top;
      this.chart.Legends.Add(legend);
    
            // 
            // done:
            //
            this.Cursor = Cursors.Default;
    }

    private void cmdArrested_Click(object sender, EventArgs e)
    {
      //
      // Check to make sure database filename in text box actually exists:
      //
      string filename = this.txtFilename.Text;

      if (!fileExists(filename))
        return;

      this.Cursor = Cursors.WaitCursor;

      clearForm();

            //
            // Retrieve data from database:
            //

            //
            // ????????????????
            //
            // NOTE: you can do this with one SQL query by summing the
            // Arrested column.  Alternatively, you can execute 2 queries,
            // one to get the total counts, and then another to just 
            // count where an arrest was made.
            //

            string version, connectionInfo;
            SqlConnection db;

            version = "MSSQLLocalDB";
            filename = "CrimeDB.mdf";

            connectionInfo = string.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);

            db = new SqlConnection(connectionInfo);
            db.Open();

            string sql = string.Format(@"SELECT Year, Count(*) AS Total FROM Crimes GROUP BY Year ORDER BY Year ASC");

            //MessageBox.Show(sql);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            string msg = db.State.ToString();
            // MessageBox.Show(msg);

            db.Close();

           

            List<int> X = new List<int>();
            List<int> Y1 = new List<int>();
            List<int> Y2 = new List<int>();

            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                X.Add(Convert.ToInt32(row["Year"]));
                Y1.Add(Convert.ToInt32(row["Total"]));
            }

            db.Open();

            string sql1 = string.Format(@"SELECT Year, Count(*) AS Arrested FROM Crimes GROUP BY Year ORDER BY Year ASC");

            // MessageBox.Show(sql);

            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = db;

            SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1);
            DataSet ds2 = new DataSet();
            cmd1.CommandText = sql1;
            adapter.Fill(ds2);
            string msg1 = db.State.ToString();
            //MessageBox.Show(msg1);

            db.Close();
            //
            // Build a set of (x,y) points for plotting:
            //

            foreach (DataRow row in ds2.Tables["TABLE"].Rows)
            {
                Y2.Add(Convert.ToInt32(row["Total"]));
                
            }
            //
            // now graph as a line chart:
            //
            this.chart.Titles.Add("Total # of Crimes Per Year vs. Number Arrested");

      var series = this.chart.Series.Add("total # of crimes");

      series.ChartType = SeriesChartType.Line;

      for (int i = 0; i < X.Count; ++i)
      {
        series.Points.AddXY(X[i], Y1[i]);
      }

      var series2 = this.chart.Series.Add("# arrested");

      series2.ChartType = SeriesChartType.Line;

            for (int i = 0; i < X.Count; ++i)
            {
                series2.Points.AddXY(X[i], Y2[i]);
            }

            var legend = new Legend();
      legend.Docking = Docking.Top;
      this.chart.Legends.Add(legend);

      //
      // done:
      //
      this.Cursor = Cursors.Default;
    }

    private void cmdOneArea_Click(object sender, EventArgs e)
    {
      //
      // Check to make sure database filename in text box actually exists:
      //
      string filename = this.txtFilename.Text;

      if (!fileExists(filename))
        return;

      this.Cursor = Cursors.WaitCursor;

      clearForm();

            //
            // Retrieve data from database:
            //

            //
            // ????????????????
            //
            // NOTE: you might be able to do this with one SQL query,
            // but probably easier to just execute 2 queries: one to
            // get the total counts, and then another to get the counts
            // for the area specified by the user.  You may assume the
            // area name entered by the user exists (though FYI using a 
            // different type of join yields the necessary counts of 0
            // for plotting, and then it always works no matter what the
            // user enters).
            //

            string version, connectionInfo;
            SqlConnection db;

            version = "MSSQLLocalDB";
            filename = "CrimeDB.mdf";

            connectionInfo = string.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);

            db = new SqlConnection(connectionInfo);
            db.Open();

            string sql = string.Format(@"SELECT Year, Count(*) As Total From Crimes GROUP BY Year ORDER By Year ASC");

            MessageBox.Show(sql);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.CommandText = sql;
            adapter.Fill(ds);
           
            string sql2 = string.Format(@"SELECT Year, Count(*) AS Total FROM Crimes WHERE Area = (SELECT Area FROM Areas WHERE AreaName = '{0}') GROUP BY Year ORDER BY Year ASC;", this.txtArea.Text);

            DataSet ds2 = new DataSet();

            cmd.CommandText = sql2;
            adapter.Fill(ds2);

            db.Close();



            //
            // Build a set of (x,y) points for plotting:
            //
            List<int> X = new List<int>();
      List<int> Y1 = new List<int>();
      List<int> Y2 = new List<int>();

      foreach (DataRow row in ds.Tables["TABLE"].Rows)
      {
        X.Add(Convert.ToInt32(row["Year"]));
        Y1.Add(Convert.ToInt32(row["Total"]));
      }

      foreach (DataRow row in ds2.Tables["TABLE"].Rows)
      {
        Y2.Add(Convert.ToInt32(row["Total"]));
      }

      //
      // now graph as a line chart:
      //
      this.chart.Titles.Add("Total # of Crimes Per Year vs. Particular Area");

      var series = this.chart.Series.Add("total # of crimes");

      series.ChartType = SeriesChartType.Line;

      for (int i = 0; i < X.Count; ++i)
      {
        series.Points.AddXY(X[i], Y1[i]);
      }

      var series2 = this.chart.Series.Add("# in this area");

      series2.ChartType = SeriesChartType.Line;

      for (int i = 0; i < X.Count; ++i)
      {
        series2.Points.AddXY(X[i], Y2[i]);
      }

      var legend = new Legend();
      legend.Docking = Docking.Top;
      this.chart.Legends.Add(legend);

      //
      // done:
      //
      this.Cursor = Cursors.Default;
    }

    private void cmdChicagoAreas_Click(object sender, EventArgs e)
    {
      //
      // Check to make sure database filename in text box actually exists:
      //
      string filename = this.txtFilename.Text;

      if (!fileExists(filename))
        return;

      this.Cursor = Cursors.WaitCursor;

      clearForm();

            //
            // Retrieve data from database:
            //
            //
            // ????????????????
            //

            string version, connectionInfo;
            SqlConnection db;

            version = "MSSQLLocalDB";
            filename = "CrimeDB.mdf";

            connectionInfo = string.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);

            db = new SqlConnection(connectionInfo);
            db.Open();

            string sql = string.Format(@"SELECT Area, Count(*) AS NumCrimes From Crimes Where Area > 0 GROUP BY Area ORDER BY Area ASC");

            MessageBox.Show(sql);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            string msg = db.State.ToString();
            MessageBox.Show(msg);

            db.Close();



            //
            // Build a set of (x,y) points for plotting:
            //
            List<int> X = new List<int>();
      List<int> Y = new List<int>();

      foreach (DataRow row in ds.Tables["TABLE"].Rows)
      {
        X.Add(Convert.ToInt32(row["Area"]));
        Y.Add(Convert.ToInt32(row["NumCrimes"]));
      }

      //
      // now graph as a line chart:
      //
      this.chart.Titles.Add("Total # of Crimes in each Chicago Area");

      var series = this.chart.Series.Add("total # of crimes");

      series.ChartType = SeriesChartType.Line;

      for (int i = 0; i < X.Count; ++i)
      {
        series.Points.AddXY(X[i], Y[i]);
      }

      var legend = new Legend();
      legend.Docking = Docking.Top;
      this.chart.Legends.Add(legend);

      //
      // done:
      //
      this.Cursor = Cursors.Default;
    }

  }//class
}//namespace
