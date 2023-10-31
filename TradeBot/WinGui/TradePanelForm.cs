using IBApi;
using System.Windows.Forms;
using TradeBot.Gui;
using TradeBot.TwsAbstractions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TradeBot.WinGui
{
    public partial class TradePanelForm : Form
    {

        public TradeController controller;
        public TradeStatusBar statusBar;

        private static readonly int[] COMMON_TICKS = {
            TickType.LAST,
            TickType.ASK,
            TickType.BID
        };

        public TradePanelForm()
        {
            InitializeComponent();

            controller = new TradeController(GlobalOutputTextBox, stock1GroupBox);
            statusBar = new TradeStatusBar(controller, controller.service, this);
        }

        private void stock1GroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void stock1Long05Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 0.5, stock1OutsideRTHCheckbox.Checked);
                }
            }
        }
        private void stock1Long1Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 1.0, stock1OutsideRTHCheckbox.Checked);
                }
            }
        }
        private void stock1Long125percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 1.25, stock1OutsideRTHCheckbox.Checked);
                }
            }
        }
        private void stock1Long15Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 1.5, stock1OutsideRTHCheckbox.Checked);
                }
            }
        }
        private void stock1Long2Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 2.0, stock1OutsideRTHCheckbox.Checked);
                }
            }
        }
        private void stock1Long25Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 2.5, stock1OutsideRTHCheckbox.Checked);
                }
            }
        }

        private void stock1Short05Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceSellLimitOrder(10, TickType.BID, buyStopPrice, 0.5, stock1OutsideRTHCheckbox.Checked);
                }
            }
        }

        private void stock1Short1Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceSellLimitOrder(10, TickType.BID, buyStopPrice, 1.0, stock1OutsideRTHCheckbox.Checked);
                }
            }
        }

        private void stock1Short125Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceSellLimitOrder(10, TickType.BID, buyStopPrice, 1.25, stock1OutsideRTHCheckbox.Checked);
                }
            }

        }

        private void stock1Short15Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceSellLimitOrder(10, TickType.BID, buyStopPrice, 1.5, stock1OutsideRTHCheckbox.Checked);
                }
            }
        }

        private void stock1Short2Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceSellLimitOrder(10, TickType.BID, buyStopPrice, 2.0, stock1OutsideRTHCheckbox.Checked);
                }
            }
        }

        private void stock1BuyStop05Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 0.5, stock1OutsideRTHCheckbox.Checked);
            }
        }

        private void stock1BuyStop1Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 1.0, stock1OutsideRTHCheckbox.Checked);
            }
        }

        private void stock1BuyStop125Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 1.25, stock1OutsideRTHCheckbox.Checked);
            }
        }

        private void stock1BuyStop15Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 1.5, stock1OutsideRTHCheckbox.Checked);
            }
        }

        private void stock1BuyStop2Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 2.0, stock1OutsideRTHCheckbox.Checked);
            }
        }

        private void stock1BuyStop25Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 2.5, stock1OutsideRTHCheckbox.Checked);
            }
        }

        private void stock1SellStop05Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceSellStopLimitOrder(10, TickType.BID, sellStopPrice, buyStopPrice, 0.5, stock1OutsideRTHCheckbox.Checked);
            }
        }
        private void stock1SellStop1Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceSellStopLimitOrder(10, TickType.BID, sellStopPrice, buyStopPrice, 1.0, stock1OutsideRTHCheckbox.Checked);
            }
        }

        private void stock1SellStop125Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceSellStopLimitOrder(10, TickType.BID, sellStopPrice, buyStopPrice, 1.25, stock1OutsideRTHCheckbox.Checked);
            }
        }

        private void stock1SellStop15Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceSellStopLimitOrder(10, TickType.BID, sellStopPrice, buyStopPrice, 1.5, stock1OutsideRTHCheckbox.Checked);
            }
        }

        private void stock1SellStop2Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceSellStopLimitOrder(10, TickType.BID, sellStopPrice, buyStopPrice, 2.0, stock1OutsideRTHCheckbox.Checked);
            }
        }

        private async void stock1Close100Percent_Click(object sender, EventArgs e)
        {
            await controller.ScalePositionAsync(-1.0, stock1OutsideRTHCheckbox.Checked);
        }

        private async void stock1Close50Percent_Click(object sender, EventArgs e)
        {
            await controller.ScalePositionAsync(-0.5, stock1OutsideRTHCheckbox.Checked);
        }

        private async void stock1Close33Percent_Click(object sender, EventArgs e)
        {
            await controller.ScalePositionAsync(-0.33, stock1OutsideRTHCheckbox.Checked);
        }

        private async void stock1Close67Percent_Click(object sender, EventArgs e)
        {
            await controller.ScalePositionAsync(-0.67, stock1OutsideRTHCheckbox.Checked);
        }

        private async void stock1Close25Percent_Click(object sender, EventArgs e)
        {
            await controller.ScalePositionAsync(-0.25, stock1OutsideRTHCheckbox.Checked);
        }

        private async void stock1ReverseButton_Click(object sender, EventArgs e)
        {
            await controller.ScalePositionAsync(-2.0, stock1OutsideRTHCheckbox.Checked);
        }
        private async void stock1TakeProfit100Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(stock1TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                await controller.LimitTakeProfitAsync(1.0, limitPrice, stock1OutsideRTHCheckbox.Checked);
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void stock1TakeProfit67Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(stock1TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                await controller.LimitTakeProfitAsync(0.67, limitPrice, stock1OutsideRTHCheckbox.Checked);
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void stock1TakeProfit50Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(stock1TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                await controller.LimitTakeProfitAsync(0.5, limitPrice, stock1OutsideRTHCheckbox.Checked);
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void stock1TakeProfit33Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(stock1TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                await controller.LimitTakeProfitAsync(0.33, limitPrice, stock1OutsideRTHCheckbox.Checked);
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void stock1TakeProfit25Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock1TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(stock1TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                await controller.LimitTakeProfitAsync(0.25, limitPrice, stock1OutsideRTHCheckbox.Checked);
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void stock1ButtonCFD_Click(object sender, EventArgs e)
        {
            string tickerSymbol = stock1TickerInput.Text;

            if (Validation.NotNullOrWhiteSpace(tickerSymbol))
            {
                controller.service.UseCFD = true;

                controller.service.TickerSymbol = tickerSymbol;
                controller.service.RequestStockContractDetails(tickerSymbol);//Rick: We need stock contract for market data
                controller.service.RequestCFDContractDetails(tickerSymbol);//Rick
                await controller.SetInitialSharesAsync();
            }
        }

        private async void stock1ButtonStock_Click(object sender, EventArgs e)
        {
            string tickerSymbol = stock1TickerInput.Text;

            if (Validation.NotNullOrWhiteSpace(tickerSymbol))
            {
                controller.service.UseCFD = false;

                controller.service.TickerSymbol = tickerSymbol;
                controller.service.RequestStockContractDetails(tickerSymbol);//Rick
                await controller.SetInitialSharesAsync();
            }
        }

        private void stock1OutsideRTHCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            GlobalOutputTextBox.AppendText("OutsideRTH: " + stock1OutsideRTHCheckbox.Checked + "\r\n");
        }

        private void stock1SellStopPriceTextBox_TextChanged(object sender, EventArgs e)
        {
            //GlobalOutputTextBox.AppendText("SellStop Updated");
        }

        private void stock1BuyStopPriceTextBox_TextChanged(object sender, EventArgs e)
        {
            //GlobalOutputTextBox.AppendText("BuyStop Updated");
        }

        private void stock1TakeProfitLimitPriceTextBox_TextChanged(object sender, EventArgs e)
        {
            //GlobalOutputTextBox.AppendText("TakeProfitLimitPrice Updated");
        }
    }
}