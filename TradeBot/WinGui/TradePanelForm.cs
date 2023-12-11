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

            controller = new TradeController(GlobalOutputTextBox, stock1GroupBox, stock2GroupBox);
            statusBar = new TradeStatusBar(controller, controller.service, this);
        }

        //Stock 1
        private async void stock1Long05Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Long05Percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(1, 10, TickType.ASK, sellStopPrice, 0.5, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Long05Percent.Enabled = true;
                }
            }
        }
        private async void stock1Long1Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Long1Percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(1, 10, TickType.ASK, sellStopPrice, 1.0, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Long1Percent.Enabled = true;
                }
            }
        }
        private async void stock1Long125percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Long125percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(1, 10, TickType.ASK, sellStopPrice, 1.25, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Long125percent.Enabled = true;
                }
            }
        }
        private async void stock1Long15Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Long15Percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(1, 10, TickType.ASK, sellStopPrice, 1.5, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Long15Percent.Enabled = true;
                }
            }
        }
        private async void stock1Long2Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Long2Percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(1, 10, TickType.ASK, sellStopPrice, 2.0, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Long2Percent.Enabled = true;
                }
            }
        }
        private async void stock1Long25Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock1SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Long25Percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(1, 10, TickType.ASK, sellStopPrice, 2.5, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Long25Percent.Enabled = true;
                }
            }
        }

        private async void stock1Short05Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Short05Percent.Enabled = false;
                    controller.service.PlaceSellLimitOrder(1,10, TickType.BID, buyStopPrice, 0.5, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Short05Percent.Enabled = true;
                }
            }
        }

        private async void stock1Short1Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Short1Percent.Enabled = false;
                    controller.service.PlaceSellLimitOrder(1, 10, TickType.BID, buyStopPrice, 1.0, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Short1Percent.Enabled = true;
                }
            }
        }

        private async void stock1Short125Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Short125Percent.Enabled = false;
                    controller.service.PlaceSellLimitOrder(1, 10, TickType.BID, buyStopPrice, 1.25, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Short125Percent.Enabled = true;
                }
            }

        }

        private async void stock1Short15Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Short15Percent.Enabled = false;
                    controller.service.PlaceSellLimitOrder(1, 10, TickType.BID, buyStopPrice, 1.5, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Short15Percent.Enabled = true;
                }
            }
        }

        private async void stock1Short2Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock1BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock1BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSetGUI(1, controller.service)
                    && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
                {
                    stock1Short2Percent.Enabled = false;
                    controller.service.PlaceSellLimitOrder(1, 10, TickType.BID, buyStopPrice, 2.0, stock1OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock1Short2Percent.Enabled = true;
                }
            }
        }

        private async void stock1BuyStop05Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1BuyStop05Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(1, 10, TickType.ASK, buyStopPrice, sellStopPrice, 0.5, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1BuyStop05Percent.Enabled = true;
            }
        }

        private async void stock1BuyStop1Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1BuyStop1Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(1, 10, TickType.ASK, buyStopPrice, sellStopPrice, 1.0, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1BuyStop1Percent.Enabled = true;
            }
        }

        private async void stock1BuyStop125Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1BuyStop125Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(1, 10, TickType.ASK, buyStopPrice, sellStopPrice, 1.25, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1BuyStop125Percent.Enabled = true;
            }
        }

        private async void stock1BuyStop15Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1BuyStop15Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(1, 10, TickType.ASK, buyStopPrice, sellStopPrice, 1.5, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1BuyStop15Percent.Enabled = true;
            }
        }

        private async void stock1BuyStop2Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1BuyStop2Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(1, 10, TickType.ASK, buyStopPrice, sellStopPrice, 2.0, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1BuyStop2Percent.Enabled = true;
            }
        }

        private async void stock1BuyStop25Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1BuyStop25Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(1, 10, TickType.ASK, buyStopPrice, sellStopPrice, 2.5, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1BuyStop25Percent.Enabled = true;
            }
        }

        private async void stock1SellStop05Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1SellStop05Percent.Enabled = false;
                controller.service.PlaceSellStopLimitOrder(1, 10, TickType.BID, sellStopPrice, buyStopPrice, 0.5, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1SellStop05Percent.Enabled = true;
            }
        }
        private async void stock1SellStop1Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1SellStop1Percent.Enabled = false; 
                controller.service.PlaceSellStopLimitOrder(1, 10, TickType.BID, sellStopPrice, buyStopPrice, 1.0, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1SellStop1Percent.Enabled = true;
            }
        }

        private async void stock1SellStop125Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1SellStop125Percent.Enabled = false;
                controller.service.PlaceSellStopLimitOrder(1, 10, TickType.BID, sellStopPrice, buyStopPrice, 1.25, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1BuyStop125Percent.Enabled = true;
            }
        }

        private async void stock1SellStop15Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1SellStop15Percent.Enabled = false;
                controller.service.PlaceSellStopLimitOrder(1, 10, TickType.BID, sellStopPrice, buyStopPrice, 1.5, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1SellStop15Percent.Enabled = true;
            }
        }

        private async void stock1SellStop2Percent_Click(object sender, EventArgs e)
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
                && Validation.TickerSetGUI(1, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(1, controller.service, COMMON_TICKS))
            {
                stock1SellStop2Percent.Enabled = false;
                controller.service.PlaceSellStopLimitOrder(1, 10, TickType.BID, sellStopPrice, buyStopPrice, 2.0, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1SellStop2Percent.Enabled = true;
            }
        }

        private async void stock1Close100Percent_Click(object sender, EventArgs e)
        {
            stock1Close100Percent.Enabled = false;
            await controller.ScalePositionAsync(1, controller.service.Stock1TickerSymbol, -1.0, stock1OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock1Close100Percent.Enabled = true;
        }

        private async void stock1Close50Percent_Click(object sender, EventArgs e)
        {
            stock1Close50Percent.Enabled = false;
            await controller.ScalePositionAsync(1, controller.service.Stock1TickerSymbol, -0.5, stock1OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock1Close50Percent.Enabled = true;
        }

        private async void stock1Close33Percent_Click(object sender, EventArgs e)
        {
            stock1Close33Percent.Enabled = false;
            await controller.ScalePositionAsync(1, controller.service.Stock1TickerSymbol, -0.33, stock1OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock1Close33Percent.Enabled = true;
        }

        private async void stock1Close67Percent_Click(object sender, EventArgs e)
        {
            stock1Close67Percent.Enabled = false;
            await controller.ScalePositionAsync(1, controller.service.Stock1TickerSymbol, -0.67, stock1OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock1Close67Percent.Enabled = true;
        }

        private async void stock1Close25Percent_Click(object sender, EventArgs e)
        {
            stock1Close25Percent.Enabled = false;
            await controller.ScalePositionAsync(1, controller.service.Stock1TickerSymbol, -0.25, stock1OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock1Close25Percent.Enabled = true;
        }

        private async void stock1ReverseButton_Click(object sender, EventArgs e)
        {
            stock1ReverseButton.Enabled = false;
            await controller.ScalePositionAsync(1, controller.service.Stock1TickerSymbol, -2.0, stock1OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock1ReverseButton.Enabled = true;
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
                stock1TakeProfit100Percent.Enabled = false;
                await controller.LimitTakeProfitAsync(1, controller.service.Stock1TickerSymbol, 1.0, limitPrice, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1TakeProfit100Percent.Enabled = true;
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
                stock1TakeProfit67Percent.Enabled = false;
                await controller.LimitTakeProfitAsync(1, controller.service.Stock1TickerSymbol, 0.67, limitPrice, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1TakeProfit67Percent.Enabled = true;
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
                stock1TakeProfit50Percent.Enabled = false;
                await controller.LimitTakeProfitAsync(1, controller.service.Stock1TickerSymbol, 0.5, limitPrice, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1TakeProfit50Percent.Enabled = true;

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
                stock1TakeProfit33Percent.Enabled = false;
                await controller.LimitTakeProfitAsync(1, controller.service.Stock1TickerSymbol, 0.33, limitPrice, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1TakeProfit33Percent.Enabled = true;
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
                stock1TakeProfit25Percent.Enabled = false;
                await controller.LimitTakeProfitAsync(1, controller.service.Stock1TickerSymbol, 0.25, limitPrice, stock1OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock1TakeProfit25Percent.Enabled = true;
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
                controller.service.Stock1UseCFD = true;

                controller.service.Stock1TickerSymbol = tickerSymbol;
                controller.service.RequestStockContractDetails(tickerSymbol, 1);//Rick: We need stock contract for market data
                controller.service.RequestCFDContractDetails(tickerSymbol, 1);//Rick
                await controller.SetInitialSharesAsync(1);
            }
        }

        private async void stock1ButtonStock_Click(object sender, EventArgs e)
        {
            string tickerSymbol = stock1TickerInput.Text;

            if (Validation.NotNullOrWhiteSpace(tickerSymbol))
            {
                controller.service.Stock1UseCFD = false;

                controller.service.Stock1TickerSymbol = tickerSymbol;
                controller.service.RequestStockContractDetails(tickerSymbol, 1);//Rick
                await controller.SetInitialSharesAsync(1);
            }
        }

        private void stock1OutsideRTHCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            GlobalOutputTextBox.AppendText("Stock 1 OutsideRTH: " + stock1OutsideRTHCheckbox.Checked + "\r\n");
        }

        //Stock 2
        private async void stock2Long05Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Long05Percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(2, 10, TickType.ASK, sellStopPrice, 0.5, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Long05Percent.Enabled = true;
                }
            }
        }
        private async void stock2Long1Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Long1Percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(2, 10, TickType.ASK, sellStopPrice, 1.0, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Long1Percent.Enabled = true;
                }
            }
        }
        private async void stock2Long125percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Long125percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(2, 10, TickType.ASK, sellStopPrice, 1.25, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Long125percent.Enabled = true;
                }
            }
        }
        private async void stock2Long15Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Long15Percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(2, 10, TickType.ASK, sellStopPrice, 1.5, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Long15Percent.Enabled = true;
                }
            }
        }
        private async void stock2Long2Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Long2Percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(2, 10, TickType.ASK, sellStopPrice, 2.0, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Long2Percent.Enabled = true;
                }
            }
        }
        private async void stock2Long25Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

                if (Validation.HasValue(sellStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Long25Percent.Enabled = false;
                    controller.service.PlaceBuyLimitOrder(2, 10, TickType.ASK, sellStopPrice, 2.5, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Long25Percent.Enabled = true;
                }
            }
        }

        private async void stock2Short05Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Short05Percent.Enabled = false;
                    controller.service.PlaceSellLimitOrder(2, 10, TickType.BID, buyStopPrice, 0.5, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Short05Percent.Enabled = true;
                }
            }
        }

        private async void stock2Short1Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Short1Percent.Enabled = false;
                    controller.service.PlaceSellLimitOrder(2, 10, TickType.BID, buyStopPrice, 1.0, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Short1Percent.Enabled = true;
                }
            }
        }

        private async void stock2Short125Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Short125Percent.Enabled = false;
                    controller.service.PlaceSellLimitOrder(2, 10, TickType.BID, buyStopPrice, 1.25, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Short125Percent.Enabled = true;
                }
            }

        }

        private async void stock2Short15Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Short15Percent.Enabled = false;
                    controller.service.PlaceSellLimitOrder(2, 10, TickType.BID, buyStopPrice, 1.5, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Short15Percent.Enabled = true;
                }
            }
        }

        private async void stock2Short2Percent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);

                if (Validation.HasValue(buyStopPrice)
                    && Validation.TickerSetGUI(2, controller.service)
                    && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
                {
                    stock2Short2Percent.Enabled = false;
                    controller.service.PlaceSellLimitOrder(2, 10, TickType.BID, buyStopPrice, 2.0, stock2OutsideRTHCheckbox.Checked);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    stock2Short2Percent.Enabled = true;
                }
            }
        }

        private async void stock2BuyStop05Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2BuyStop05Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(2, 10, TickType.ASK, buyStopPrice, sellStopPrice, 0.5, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2BuyStop05Percent.Enabled = true;
            }
        }

        private async void stock2BuyStop1Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2BuyStop1Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(2, 10, TickType.ASK, buyStopPrice, sellStopPrice, 1.0, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2BuyStop1Percent.Enabled = true;
            }
        }

        private async void stock2BuyStop125Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2BuyStop125Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(2, 10, TickType.ASK, buyStopPrice, sellStopPrice, 1.25, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2BuyStop125Percent.Enabled = true;
            }
        }

        private async void stock2BuyStop15Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2BuyStop15Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(2, 10, TickType.ASK, buyStopPrice, sellStopPrice, 1.5, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2BuyStop15Percent.Enabled = true;
            }
        }

        private async void stock2BuyStop2Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2BuyStop2Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(2, 10, TickType.ASK, buyStopPrice, sellStopPrice, 2.0, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2BuyStop2Percent.Enabled = true;
            }
        }

        private async void stock2BuyStop25Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);
            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2BuyStop25Percent.Enabled = false;
                controller.service.PlaceBuyStopLimitOrder(2, 10, TickType.ASK, buyStopPrice, sellStopPrice, 2.5, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2BuyStop25Percent.Enabled = true;
            }
        }

        private async void stock2SellStop05Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2SellStop05Percent.Enabled = false;
                controller.service.PlaceSellStopLimitOrder(2, 10, TickType.BID, sellStopPrice, buyStopPrice, 0.5, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2SellStop05Percent.Enabled = true;
            }
        }
        private async void stock2SellStop1Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2SellStop1Percent.Enabled = false;
                controller.service.PlaceSellStopLimitOrder(2, 10, TickType.BID, sellStopPrice, buyStopPrice, 1.0, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2SellStop1Percent.Enabled = true;
            }
        }

        private async void stock2SellStop125Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2SellStop125Percent.Enabled = false;
                controller.service.PlaceSellStopLimitOrder(2, 10, TickType.BID, sellStopPrice, buyStopPrice, 1.25, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2SellStop125Percent.Enabled = true;
            }
        }

        private async void stock2SellStop15Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2SellStop15Percent.Enabled = false;
                controller.service.PlaceSellStopLimitOrder(2, 10, TickType.BID, sellStopPrice, buyStopPrice, 1.5, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2SellStop15Percent.Enabled = true;
            }
        }

        private async void stock2SellStop2Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2SellStopPriceTextBox.Text))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(stock2BuyStopPriceTextBox.Text))
            {
                return;
            }

            double sellStopPrice = Double.Parse(stock2SellStopPriceTextBox.Text);
            double buyStopPrice = Double.Parse(stock2BuyStopPriceTextBox.Text);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSetGUI(2, controller.service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailableGUI(2, controller.service, COMMON_TICKS))
            {
                stock2SellStop2Percent.Enabled = false;
                controller.service.PlaceSellStopLimitOrder(2, 10, TickType.BID, sellStopPrice, buyStopPrice, 2.0, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2SellStop2Percent.Enabled = true;
            }
        }

        private async void stock2Close100Percent_Click(object sender, EventArgs e)
        {
            stock2Close100Percent.Enabled = false;
            await controller.ScalePositionAsync(2, controller.service.Stock2TickerSymbol, -1.0, stock2OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock2Close100Percent.Enabled = true;
        }

        private async void stock2Close50Percent_Click(object sender, EventArgs e)
        {
            stock2Close50Percent.Enabled = false; 
            await controller.ScalePositionAsync(2, controller.service.Stock2TickerSymbol, -0.5, stock2OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock2Close50Percent.Enabled = true;
        }

        private async void stock2Close33Percent_Click(object sender, EventArgs e)
        {
            stock2Close33Percent.Enabled = false;
            await controller.ScalePositionAsync(2, controller.service.Stock2TickerSymbol, -0.33, stock2OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock2Close33Percent.Enabled = true;
        }

        private async void stock2Close67Percent_Click(object sender, EventArgs e)
        {
            stock2Close67Percent.Enabled = false;
            await controller.ScalePositionAsync(2, controller.service.Stock2TickerSymbol, -0.67, stock2OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock2Close67Percent.Enabled = true;
        }

        private async void stock2Close25Percent_Click(object sender, EventArgs e)
        {
            stock2Close25Percent.Enabled = false;
            await controller.ScalePositionAsync(2, controller.service.Stock2TickerSymbol, -0.25, stock2OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock2Close25Percent.Enabled = true;
        }

        private async void stock2ReverseButton_Click(object sender, EventArgs e)
        {
            stock2ReverseButton.Enabled = false;
            await controller.ScalePositionAsync(2, controller.service.Stock2TickerSymbol, -2.0, stock2OutsideRTHCheckbox.Checked);
            await Task.Delay(TimeSpan.FromSeconds(5));
            stock2ReverseButton.Enabled = true;
        }
        private async void stock2TakeProfit100Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(stock2TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                stock2TakeProfit100Percent.Enabled = false;
                await controller.LimitTakeProfitAsync(2, controller.service.Stock2TickerSymbol, 1.0, limitPrice, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2TakeProfit100Percent.Enabled = true;
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void stock2TakeProfit67Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(stock2TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                stock2TakeProfit67Percent.Enabled = false;
                await controller.LimitTakeProfitAsync(2, controller.service.Stock2TickerSymbol, 0.67, limitPrice, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2TakeProfit67Percent.Enabled = true;
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void stock2TakeProfit50Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(stock2TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                stock2TakeProfit50Percent.Enabled = false;
                await controller.LimitTakeProfitAsync(2, controller.service.Stock2TickerSymbol, 0.5, limitPrice, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2TakeProfit50Percent.Enabled = true;
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void stock2TakeProfit33Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(stock2TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                stock2TakeProfit33Percent.Enabled = false;
                await controller.LimitTakeProfitAsync(2, controller.service.Stock2TickerSymbol, 0.33, limitPrice, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2TakeProfit33Percent.Enabled = true;
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void stock2TakeProfit25Percent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stock2TakeProfitLimitPriceTextBox.Text))
            {
                return;
            }

            double limitPrice = Double.Parse(stock2TakeProfitLimitPriceTextBox.Text);

            if (Validation.HasValue(limitPrice))
            {
                stock2TakeProfit25Percent.Enabled = false;
                await controller.LimitTakeProfitAsync(2, controller.service.Stock2TickerSymbol, 0.25, limitPrice, stock2OutsideRTHCheckbox.Checked);
                await Task.Delay(TimeSpan.FromSeconds(5));
                stock2TakeProfit25Percent.Enabled = true;
            }
            else
            {
                //"Invalid Take Profit Limit Price" 
                return;
            }
        }

        private async void stock2ButtonCFD_Click(object sender, EventArgs e)
        {
            string tickerSymbol = stock2TickerInput.Text;

            if (Validation.NotNullOrWhiteSpace(tickerSymbol))
            {
                controller.service.Stock2UseCFD = true;

                controller.service.Stock2TickerSymbol = tickerSymbol;
                controller.service.RequestStockContractDetails(tickerSymbol, 2);//Rick: We need stock contract for market data
                controller.service.RequestCFDContractDetails(tickerSymbol, 2);//Rick
                await controller.SetInitialSharesAsync(2);
            }
        }

        private async void stock2ButtonStock_Click(object sender, EventArgs e)
        {
            string tickerSymbol = stock2TickerInput.Text;

            if (Validation.NotNullOrWhiteSpace(tickerSymbol))
            {
                controller.service.Stock2UseCFD = false;

                controller.service.Stock2TickerSymbol = tickerSymbol;
                controller.service.RequestStockContractDetails(tickerSymbol, 2);//Rick
                await controller.SetInitialSharesAsync(2);
            }
        }

        private void stock2OutsideRTHCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            GlobalOutputTextBox.AppendText("Stock 2 OutsideRTH: " + stock2OutsideRTHCheckbox.Checked + "\r\n");
        }

    }
}