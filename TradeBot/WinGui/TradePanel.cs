using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradeBot
{
    public partial class TradePanel : Form
    {
        public TradeController controller;
        public TradeStatusBar statusBar;

        public TradePanel()
        {
            InitializeComponent();

            controller = new TradeController(textBox1);
            statusBar = new TradeStatusBar(controller, controller.service, this);
        }
    }
}
