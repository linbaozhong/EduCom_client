﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;

namespace Snow.Edu
{
    public partial class frmMain : Form
    {
        public frmMain()
        {


            InitializeComponent();

            GeckoWebBrowser web = new GeckoWebBrowser();
            this.Controls.Add(web);
            web.Dock = DockStyle.Fill;
        }
    }
}
