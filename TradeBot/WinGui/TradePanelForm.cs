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

            controller = new TradeController(GlobalOutputTextBox);
            statusBar = new TradeStatusBar(controller, controller.service, this);
        }

        private void stock1GroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void long05Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 0.5);
                } 
            }
        }
        private void Long1Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 1.0);
                }
            }
        }
        private void Long125percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 1.25);
                }
            }
        }
        private void Long15Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 1.5);
                }
            }
        }
        private void Long2Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 2.0);
                }
            }
        }
        private void Long25Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceBuyLimitOrder(10, TickType.ASK, sellStopPrice, 2.5);
                }
            }
        }

        private void short05Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceSellLimitOrder(10, TickType.BID, buyStopPrice, 0.5);
                }
            }
        }

        private void Short1Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceSellLimitOrder(10, TickType.BID, buyStopPrice, 1.0);
                }
            }
        }

        private void Short125Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceSellLimitOrder(10, TickType.BID, buyStopPrice, 1.25);
                }
            }

        }

        private void Short15Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceSellLimitOrder(10, TickType.BID, buyStopPrice, 1.5);
                }
            }
        }

        private void Short2Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSet(controller.service)
                    && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
                {
                    controller.service.PlaceSellLimitOrder(10, TickType.BID, buyStopPrice, 2.0);
                }
            }
        }

        private void BuyStop05Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text); 
            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 0.5);
            }
        }

        private void BuyStop1Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 1.0);
            }
        }

        private void BuyStop125Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 1.25);
            }
        }

        private void BuyStop15Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 1.5);
            }
        }

        private void BuyStop2Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 2.0);
            }
        }

        private void BuyStop25Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceBuyStopLimitOrder(10, TickType.ASK, buyStopPrice, sellStopPrice, 2.5);
            }
        }

        private void SellStop05Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceSellStopLimitOrder(10, TickType.BID, sellStopPrice, buyStopPrice, 0.5);
            }
        }
        private void SellStop1Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceSellStopLimitOrder(10, TickType.BID, sellStopPrice, buyStopPrice, 1.0);
            }
        }

        private void SellStop125Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceSellStopLimitOrder(10, TickType.BID, sellStopPrice, buyStopPrice, 1.25);
            }
        }

        private void SellStop15Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceSellStopLimitOrder(10, TickType.BID, sellStopPrice, buyStopPrice, 1.5);
            }
        }

        private void SellStop2Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                controller.service.PlaceSellStopLimitOrder(10, TickType.BID, sellStopPrice, buyStopPrice, 2.0);
            }
        }

        private async Task ScalePositionAsync(double percent)
        {
            Position position = await controller.service.RequestCurrentPositionAsync();
            if (Validation.TickerSet(controller.service)
                && Validation.PositionExists(position)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                int orderDelta = (int)Math.Round(position.PositionSize * percent);
                int orderQuantity = Math.Abs(orderDelta);

                if (orderDelta > 0)
                {
                    controller.service.PlaceCloseLimitOrder(OrderActions.BUY, orderQuantity, TickType.ASK);
                }
                else if (orderDelta < 0)
                {
                    controller.service.PlaceCloseLimitOrder(OrderActions.SELL, orderQuantity, TickType.BID);
                }
            }
        }

        private async void Close100Percent_Click(object sender, EventArgs e)
        {
            await ScalePositionAsync(-1.0);
        }

        private async void Close50Percent_Click(object sender, EventArgs e)
        {
            await ScalePositionAsync(-0.5);
        }

        private async void Close33Percent_Click(object sender, EventArgs e)
        {
            await ScalePositionAsync(-0.33);
        }

        private async void Close67Percent_Click(object sender, EventArgs e)
        {
            await ScalePositionAsync(-0.67);
        }

        private async void Close25Percent_Click(object sender, EventArgs e)
        {
            await ScalePositionAsync(-0.25);
        }
        
        private async void reverseButton_Click(object sender, EventArgs e)
        {
            await ScalePositionAsync(-2.0);
        }

        private async Task LimitTakeProfitAsync(double percent, double limitPrice)
        {
            Position position = await controller.service.RequestCurrentPositionAsync();
            if (Validation.TickerSet(controller.service)
                && Validation.PositionExists(position)
                && Validation.TickDataAvailable(controller.service, COMMON_TICKS))
            {
                int orderDelta = (int)Math.Round(position.PositionSize * percent);
                int orderQuantity = Math.Abs(orderDelta);

                if (orderDelta > 0)
                {
                    controller.service.PlaceTakeProfitLimitOrder(OrderActions.SELL, orderQuantity, limitPrice);
                }
                else if (orderDelta < 0)
                {
                    controller.service.PlaceTakeProfitLimitOrder(OrderActions.BUY, orderQuantity, limitPrice);
                }
            }
        }

        private async void TakeProfit50Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                await LimitTakeProfitAsync(0.5, limitPrice);
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void TakeProfit33Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                await LimitTakeProfitAsync(0.33, limitPrice);
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void TakeProfit25Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                await LimitTakeProfitAsync(0.25, limitPrice);
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void buttonCFD_Click(object sender, EventArgs e)
        {
            string tickerSymbol = TickerInput.Text;

            if (Validation.NotNullOrWhiteSpace(tickerSymbol))
            {
                controller.service.UseCFD = true;

                controller.service.TickerSymbol = tickerSymbol;
                controller.service.RequestStockContractDetails(tickerSymbol);//Rick: We need stock contract for market data
                controller.service.RequestCFDContractDetails(tickerSymbol);//Rick
                await controller.SetInitialSharesAsync();
            }
        }

        private async void buttonStock_Click(object sender, EventArgs e)
        {
            string tickerSymbol = TickerInput.Text;

            if (Validation.NotNullOrWhiteSpace(tickerSymbol))
            {
                controller.service.UseCFD = false;

                controller.service.TickerSymbol = tickerSymbol;
                controller.service.RequestStockContractDetails(tickerSymbol);//Rick
                await controller.SetInitialSharesAsync();
            }
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