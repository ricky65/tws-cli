using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TradeBot.WinGui
{
    public partial class TradePanelForm : Form
    {

        public TradeController controller;
        public TradeStatusBar statusBar;

        public TradePanelForm()
        {
            InitializeComponent();

            controller = new TradeController(GlobalOutputTextBox);
            statusBar = new TradeStatusBar(controller, controller.service, this);
        }

        private void stock1GroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void long05Percent_Click(object sender, EventArgs e)
        {
            //string sellStopPriceInput = IO.PromptForInputIfNecessary(args, 0, Messages.SellStopPrompt);
            double sellStopPrice = Double.Parse(Long05Percent.Text);

            //if (Validation.HasValue(sellStopPrice)
            //    && Validation.TickerSet(service)
            //    && Validation.TickDataAvailable(service, COMMON_TICKS))
            ///{
            //    service.PlaceBuyLimitOrder(TickType.ASK, sellStopPrice, 0.5);
            //}
        }
        private void Long1Percent_Click(object sender, EventArgs e)
        {

        }
        private void Long125percent_Click(object sender, EventArgs e)
        {

        }
        private void Long15Percent_Click(object sender, EventArgs e)
        {

        }
        private void Long2Percent_Click(object sender, EventArgs e)
        {

        }
        private void Long25Percent_Click(object sender, EventArgs e)
        {

        }

        private void short05Percent_Click(object sender, EventArgs e)
        {

        }

        private void Short1Percent_Click(object sender, EventArgs e)
        {

        }

        private void Short125Percent_Click(object sender, EventArgs e)
        {

        }

        private void Short15Percent_Click(object sender, EventArgs e)
        {

        }

        private void Short2Percent_Click(object sender, EventArgs e)
        {

        }
        private void BuyStop05Percent_Click(object sender, EventArgs e)
        {

        }
        private void BuyStop1Percent_Click(object sender, EventArgs e)
        {

        }

        private void BuyStop125Percent_Click(object sender, EventArgs e)
        {

        }

        private void BuyStop15Percent_Click(object sender, EventArgs e)
        {

        }

        private void BuyStop2Percent_Click(object sender, EventArgs e)
        {

        }

        private void BuyStop25Percent_Click(object sender, EventArgs e)
        {

        }

        private void SellStop05Percent_Click(object sender, EventArgs e)
        {

        }
        private void SellStop1Percent_Click(object sender, EventArgs e)
        {

        }

        private void SellStop125Percent_Click(object sender, EventArgs e)
        {

        }

        private void SellStop15Percent_Click(object sender, EventArgs e)
        {

        }

        private void SellStop2Percent_Click(object sender, EventArgs e)
        {

        }

        private void Close100Percent_Click(object sender, EventArgs e)
        {

        }

        private void Close50Percent_Click(object sender, EventArgs e)
        {

        }

        private void Close33Percent_Click(object sender, EventArgs e)
        {

        }

        private void Close67Percent_Click(object sender, EventArgs e)
        {

        }

        private void Close25Percent_Click(object sender, EventArgs e)
        {

        }

        private void TakeProfit50Percent_Click(object sender, EventArgs e)
        {

        }

        private void TakeProfit33Percent_Click(object sender, EventArgs e)
        {

        }

        private void TakeProfit25Percent_Click(object sender, EventArgs e)
        {

        }

        private void reverseButton_Click(object sender, EventArgs e)
        {

        }

        private void buttonCFD_Click(object sender, EventArgs e)
        {

        }

        private void buttonStock_Click(object sender, EventArgs e)
        {

        }

        private void OutsideRTHCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            GlobalOutputTextBox.AppendText("OutsideRTH: " + OutsideRTHCheckbox.Checked);
        }

        private void SellStopPriceTextBox_TextChanged(object sender, EventArgs e)
        {
            GlobalOutputTextBox.AppendText("SellStop Updated");
        }

        private void BuyStopPriceTextBox_TextChanged(object sender, EventArgs e)
        {
            GlobalOutputTextBox.AppendText("BuyStop Updated");
        }

        private void TakeProfitLimitPriceTextBox_TextChanged(object sender, EventArgs e)
        {
            GlobalOutputTextBox.AppendText("TakeProfitLimitPrice Updated");
        }
    }
}