using IBApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradeBot.Events;
using TradeBot.Extensions;
using TradeBot.TwsAbstractions;
using TradeBot.WinGui;
using static TradeBot.Utils.Constants;
using static TradeBot.AppProperties;

namespace TradeBot
{
    public class TradeStatusBar
    {
        private TradeController controller;
        private TradeService service;

        public TradePanelForm tradePanel;

        public TradeStatusBar(TradeController controller, TradeService service, TradePanelForm tradePanel)
        {
            this.controller = controller;
            this.service = service;
            this.tradePanel = tradePanel;

            controller.PropertyChanged += OnPropertyChanged;
            service.PropertyChanged += OnPropertyChanged;
            service.TickUpdated += OnTickUpdated;
            service.PositionUpdated += OnPositionUpdated;
        }

        private async void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(controller.Shares):
                case nameof(controller.Cash):
                case nameof(service.Stock1TickerSymbol):
                case nameof(service.Stock2TickerSymbol):
                case nameof(service.Stock3TickerSymbol):
                case nameof(service.Stock4TickerSymbol):
                case nameof(service.RiskPercent):
                    await UpdateHeaderAsyncGUI();
                    break;
            }
        }

        private async void OnTickUpdated(int tickType, double value)
        {
            await UpdateHeaderAsyncGUI();
        }

        private async void OnPositionUpdated(Position position)
        {
            await UpdateHeaderAsyncGUI();
        }

        //private async Task UpdateHeaderAsync()
        //{
        //    IList<string> infoStrings = new List<string>();

        //    string appName = Messages.AppName;
        //    if (!string.IsNullOrWhiteSpace(appName))
        //    {
        //        infoStrings.Add(appName);
        //    }

        //    bool hasTickerSymbol = service.HasTickerSymbol;
        //    string tickerSymbol = service.TickerSymbol;
        //    string tickerDisplayValue = hasTickerSymbol ? tickerSymbol : Messages.TitleUnavailable;
        //    infoStrings.Add(string.Format(Messages.TitleRiskPerTrade, service.RiskPercent));
        //    infoStrings.Add(string.Format(Messages.TitleTickerSymbol, tickerDisplayValue));

        //    infoStrings.Add(string.Format(Messages.TitleShares, controller.Shares));

        //    if (hasTickerSymbol)
        //    {
        //        Position currentPosition = await service.RequestCurrentPositionAsync();
        //        double positionSize = currentPosition?.PositionSize ?? 0;
        //        infoStrings.Add(string.Format(Messages.TitlePositionSize, positionSize));

        //        infoStrings.Add(string.Format(Messages.TitleLastFormat, GetTickAsCurrencyString(TickType.LAST)));
        //        infoStrings.Add(string.Format(Messages.TitleBidAskFormat, GetTickAsCurrencyString(TickType.BID), GetTickAsCurrencyString(TickType.ASK), GetTickAsCommaFormattedString(TickType.BID_SIZE), GetTickAsCommaFormattedString(TickType.ASK_SIZE)));
        //        infoStrings.Add(string.Format(Messages.TitleVolumeFormat, GetTickAsCommaFormattedString(TickType.VOLUME)));
        //        infoStrings.Add(string.Format(Messages.TitleCloseFormat, GetTickAsCurrencyString(TickType.CLOSE)));
        //        infoStrings.Add(string.Format(Messages.TitleOpenFormat, GetTickAsCurrencyString(TickType.OPEN)));
        //    }

        //    //Console.Title = string.Join(Messages.TitleDivider, infoStrings);
        //}

        //Rick: UpdateHeaderAsync() for GUI
        private async Task UpdateHeaderAsyncGUI()
        {
            //IList<string> infoStrings = new List<string>();

            string appName = Messages.AppName;
            //if (!string.IsNullOrWhiteSpace(appName))
            //{
            //    infoStrings.Add(appName);
            //}

            //Stock 1
            bool hasStock1TickerSymbol = service.HasStock1TickerSymbol;
            string stock1TickerSymbol = service.Stock1TickerSymbol;
            string stock1TickerDisplayValue = hasStock1TickerSymbol ? stock1TickerSymbol : Messages.TitleUnavailable;
            //infoStrings.Add(string.Format(Messages.TitleRiskPerTrade, service.RiskPercent));
            //infoStrings.Add(string.Format(Messages.TitleTickerSymbol, stock1TickerDisplayValue));

            //infoStrings.Add(string.Format(Messages.TitleShares, controller.Shares));

            if (hasStock1TickerSymbol)
            {
                Position stock1CurrentPosition = await service.RequestCurrentPositionAsync(stock1TickerSymbol);
                double stock1PositionSize = stock1CurrentPosition?.PositionSize ?? 0;
                //infoStrings.Add(string.Format(Messages.TitlePositionSize, stock1PositionSize));

                //infoStrings.Add(string.Format(Messages.TitleLastFormat, GetTickAsCurrencyString(1, TickType.LAST)));
                //infoStrings.Add(string.Format(Messages.TitleBidAskFormat, GetTickAsCurrencyString(1, TickType.BID), GetTickAsCurrencyString(1, TickType.ASK), GetTickAsCommaFormattedString(1, TickType.BID_SIZE), GetTickAsCommaFormattedString(1, TickType.ASK_SIZE)));
                //infoStrings.Add(string.Format(Messages.TitleVolumeFormat, GetTickAsCommaFormattedString(1, TickType.VOLUME)));
                //infoStrings.Add(string.Format(Messages.TitleCloseFormat, GetTickAsCurrencyString(1, TickType.CLOSE)));
                //infoStrings.Add(string.Format(Messages.TitleOpenFormat, GetTickAsCurrencyString(1, TickType.OPEN)));

                if (tradePanel.InvokeRequired)
                {
                    tradePanel.BeginInvoke(() =>
                    {
                        //tradePanel.Text = string.Join(Messages.TitleDivider, infoStrings);

                        tradePanel.stock1PositionOutputLabel.Text = stock1PositionSize.ToString("N0");
                        tradePanel.stock1LastPriceOutputLabel.Text = GetTickAsCurrencyString(STOCK_ONE, TickType.LAST);
                        tradePanel.stock1PercentageChangeOutputLabel.Text = "todo";
                        tradePanel.stock1BidAskOutput.Text = string.Format("{0} x {1} ({2}%)", GetTickAsCurrencyString(STOCK_ONE, TickType.BID), GetTickAsCurrencyString(STOCK_ONE, TickType.ASK), GetAskBidPercentageDifference(STOCK_ONE, TickType.BID, TickType.ASK));
                        tradePanel.stock1BidAskSizeOutput.Text = string.Format("{0} x {1}", GetTickAsCommaFormattedString(STOCK_ONE, TickType.BID_SIZE), GetTickAsCommaFormattedString(STOCK_ONE, TickType.ASK_SIZE));
                    });
                }
                else
                {
                    //tradePanel.Text = string.Join(Messages.TitleDivider, infoStrings);

                    tradePanel.stock1PositionOutputLabel.Text = stock1PositionSize.ToString("N0");
                    tradePanel.stock1LastPriceOutputLabel.Text = GetTickAsCurrencyString(STOCK_ONE, TickType.LAST);
                    tradePanel.stock1PercentageChangeOutputLabel.Text = "todo";
                    tradePanel.stock1BidAskOutput.Text = string.Format("{0} x {1} ({2}%)", GetTickAsCurrencyString(STOCK_ONE, TickType.BID), GetTickAsCurrencyString(STOCK_ONE, TickType.ASK), GetAskBidPercentageDifference(STOCK_ONE, TickType.BID, TickType.ASK));
                    tradePanel.stock1BidAskSizeOutput.Text = string.Format("{0} x {1}", GetTickAsCommaFormattedString(STOCK_ONE, TickType.BID_SIZE), GetTickAsCommaFormattedString(STOCK_ONE, TickType.ASK_SIZE));
                }
            }

            //Stock 2
            bool hasStock2TickerSymbol = service.HasStock2TickerSymbol;
            string stock2TickerSymbol = service.Stock2TickerSymbol;
            string stock2TickerDisplayValue = hasStock2TickerSymbol ? stock2TickerSymbol : Messages.TitleUnavailable;
            //infoStrings.Add(string.Format(Messages.TitleRiskPerTrade, service.RiskPercent));
            //infoStrings.Add(string.Format(Messages.TitleTickerSymbol, stock1TickerDisplayValue));

            //infoStrings.Add(string.Format(Messages.TitleShares, controller.Shares));

            if (hasStock2TickerSymbol)
            {
                Position stock2CurrentPosition = await service.RequestCurrentPositionAsync(stock2TickerSymbol);
                double stock2PositionSize = stock2CurrentPosition?.PositionSize ?? 0;
                //infoStrings.Add(string.Format(Messages.TitlePositionSize, stock1PositionSize));

                //infoStrings.Add(string.Format(Messages.TitleLastFormat, GetTickAsCurrencyString(TickType.LAST)));
                //infoStrings.Add(string.Format(Messages.TitleBidAskFormat, GetTickAsCurrencyString(TickType.BID), GetTickAsCurrencyString(TickType.ASK), GetTickAsCommaFormattedString(TickType.BID_SIZE), GetTickAsCommaFormattedString(TickType.ASK_SIZE)));
                //infoStrings.Add(string.Format(Messages.TitleVolumeFormat, GetTickAsCommaFormattedString(TickType.VOLUME)));
                //infoStrings.Add(string.Format(Messages.TitleCloseFormat, GetTickAsCurrencyString(TickType.CLOSE)));
                //infoStrings.Add(string.Format(Messages.TitleOpenFormat, GetTickAsCurrencyString(TickType.OPEN)));

                if (tradePanel.InvokeRequired)
                {
                    tradePanel.BeginInvoke(() =>
                    {
                        //tradePanel.Text = string.Join(Messages.TitleDivider, infoStrings);

                        tradePanel.stock2PositionOutputLabel.Text = stock2PositionSize.ToString("N0");
                        tradePanel.stock2LastPriceOutputLabel.Text = GetTickAsCurrencyString(STOCK_TWO, TickType.LAST);
                        tradePanel.stock2PercentageChangeOutputLabel.Text = "todo";
                        tradePanel.stock2BidAskOutput.Text = string.Format("{0} x {1} ({2}%)", GetTickAsCurrencyString(STOCK_TWO, TickType.BID), GetTickAsCurrencyString(STOCK_TWO, TickType.ASK), GetAskBidPercentageDifference(STOCK_TWO, TickType.BID, TickType.ASK));
                        tradePanel.stock2BidAskSizeOutput.Text = string.Format("{0} x {1}", GetTickAsCommaFormattedString(STOCK_TWO, TickType.BID_SIZE), GetTickAsCommaFormattedString(STOCK_TWO, TickType.ASK_SIZE));
                    });
                }
                else
                {
                    //tradePanel.Text = string.Join(Messages.TitleDivider, infoStrings);

                    tradePanel.stock2PositionOutputLabel.Text = stock2PositionSize.ToString("N0");
                    tradePanel.stock2LastPriceOutputLabel.Text = GetTickAsCurrencyString(STOCK_TWO, TickType.LAST);
                    tradePanel.stock2PercentageChangeOutputLabel.Text = "todo";
                    tradePanel.stock2BidAskOutput.Text = string.Format("{0} x {1} ({2}%)", GetTickAsCurrencyString(STOCK_TWO, TickType.BID), GetTickAsCurrencyString(STOCK_TWO, TickType.ASK), GetAskBidPercentageDifference(STOCK_TWO, TickType.BID, TickType.ASK));
                    tradePanel.stock2BidAskSizeOutput.Text = string.Format("{0} x {1}", GetTickAsCommaFormattedString(STOCK_TWO, TickType.BID_SIZE), GetTickAsCommaFormattedString(STOCK_TWO, TickType.ASK_SIZE));
                }
            }

            //Stock 3
            bool hasStock3TickerSymbol = service.HasStock3TickerSymbol;
            string stock3TickerSymbol = service.Stock3TickerSymbol;
            string stock3TickerDisplayValue = hasStock3TickerSymbol ? stock3TickerSymbol : Messages.TitleUnavailable;
            //infoStrings.Add(string.Format(Messages.TitleRiskPerTrade, service.RiskPercent));
            //infoStrings.Add(string.Format(Messages.TitleTickerSymbol, stock1TickerDisplayValue));

            //infoStrings.Add(string.Format(Messages.TitleShares, controller.Shares));

            if (hasStock3TickerSymbol)
            {
                Position stock3CurrentPosition = await service.RequestCurrentPositionAsync(stock3TickerSymbol);
                double stock3PositionSize = stock3CurrentPosition?.PositionSize ?? 0;
                //infoStrings.Add(string.Format(Messages.TitlePositionSize, stock1PositionSize));

                //infoStrings.Add(string.Format(Messages.TitleLastFormat, GetTickAsCurrencyString(TickType.LAST)));
                //infoStrings.Add(string.Format(Messages.TitleBidAskFormat, GetTickAsCurrencyString(TickType.BID), GetTickAsCurrencyString(TickType.ASK), GetTickAsCommaFormattedString(TickType.BID_SIZE), GetTickAsCommaFormattedString(TickType.ASK_SIZE)));
                //infoStrings.Add(string.Format(Messages.TitleVolumeFormat, GetTickAsCommaFormattedString(TickType.VOLUME)));
                //infoStrings.Add(string.Format(Messages.TitleCloseFormat, GetTickAsCurrencyString(TickType.CLOSE)));
                //infoStrings.Add(string.Format(Messages.TitleOpenFormat, GetTickAsCurrencyString(TickType.OPEN)));

                if (tradePanel.InvokeRequired)
                {
                    tradePanel.BeginInvoke(() =>
                    {
                        //tradePanel.Text = string.Join(Messages.TitleDivider, infoStrings);

                        tradePanel.stock3PositionOutputLabel.Text = stock3PositionSize.ToString("N0");
                        tradePanel.stock3LastPriceOutputLabel.Text = GetTickAsCurrencyString(STOCK_THREE, TickType.LAST);
                        tradePanel.stock3PercentageChangeOutputLabel.Text = "todo";
                        tradePanel.stock3BidAskOutput.Text = string.Format("{0} x {1} ({2}%)", GetTickAsCurrencyString(STOCK_THREE, TickType.BID), GetTickAsCurrencyString(3, TickType.ASK), GetAskBidPercentageDifference(STOCK_THREE, TickType.BID, TickType.ASK));
                        tradePanel.stock3BidAskSizeOutput.Text = string.Format("{0} x {1}", GetTickAsCommaFormattedString(STOCK_THREE, TickType.BID_SIZE), GetTickAsCommaFormattedString(STOCK_THREE, TickType.ASK_SIZE));
                    });
                }
                else
                {
                    //tradePanel.Text = string.Join(Messages.TitleDivider, infoStrings);

                    tradePanel.stock3PositionOutputLabel.Text = stock3PositionSize.ToString("N0");
                    tradePanel.stock3LastPriceOutputLabel.Text = GetTickAsCurrencyString(STOCK_THREE, TickType.LAST);
                    tradePanel.stock3PercentageChangeOutputLabel.Text = "todo";
                    tradePanel.stock3BidAskOutput.Text = string.Format("{0} x {1} ({2}%)", GetTickAsCurrencyString(STOCK_THREE, TickType.BID), GetTickAsCurrencyString(STOCK_THREE, TickType.ASK), GetAskBidPercentageDifference(STOCK_THREE, TickType.BID, TickType.ASK));
                    tradePanel.stock3BidAskSizeOutput.Text = string.Format("{0} x {1}", GetTickAsCommaFormattedString(STOCK_THREE, TickType.BID_SIZE), GetTickAsCommaFormattedString(STOCK_THREE, TickType.ASK_SIZE));
                }
            }
            //Stock 4
            bool hasStock4TickerSymbol = service.HasStock4TickerSymbol;
            string stock4TickerSymbol = service.Stock4TickerSymbol;
            string stock4TickerDisplayValue = hasStock4TickerSymbol ? stock4TickerSymbol : Messages.TitleUnavailable;
            //infoStrings.Add(string.Format(Messages.TitleRiskPerTrade, service.RiskPercent));
            //infoStrings.Add(string.Format(Messages.TitleTickerSymbol, stock1TickerDisplayValue));

            //infoStrings.Add(string.Format(Messages.TitleShares, controller.Shares));

            if (hasStock4TickerSymbol)
            {
                Position stock4CurrentPosition = await service.RequestCurrentPositionAsync(stock4TickerSymbol);
                double stock4PositionSize = stock4CurrentPosition?.PositionSize ?? 0;
                //infoStrings.Add(string.Format(Messages.TitlePositionSize, stock1PositionSize));

                //infoStrings.Add(string.Format(Messages.TitleLastFormat, GetTickAsCurrencyString(TickType.LAST)));
                //infoStrings.Add(string.Format(Messages.TitleBidAskFormat, GetTickAsCurrencyString(TickType.BID), GetTickAsCurrencyString(TickType.ASK), GetTickAsCommaFormattedString(TickType.BID_SIZE), GetTickAsCommaFormattedString(TickType.ASK_SIZE)));
                //infoStrings.Add(string.Format(Messages.TitleVolumeFormat, GetTickAsCommaFormattedString(TickType.VOLUME)));
                //infoStrings.Add(string.Format(Messages.TitleCloseFormat, GetTickAsCurrencyString(TickType.CLOSE)));
                //infoStrings.Add(string.Format(Messages.TitleOpenFormat, GetTickAsCurrencyString(TickType.OPEN)));

                if (tradePanel.InvokeRequired)
                {
                    tradePanel.BeginInvoke(() =>
                    {
                        //tradePanel.Text = string.Join(Messages.TitleDivider, infoStrings);

                        tradePanel.stock4PositionOutputLabel.Text = stock4PositionSize.ToString("N0");
                        tradePanel.stock4LastPriceOutputLabel.Text = GetTickAsCurrencyString(STOCK_FOUR, TickType.LAST);
                        tradePanel.stock4PercentageChangeOutputLabel.Text = "todo";
                        tradePanel.stock4BidAskOutput.Text = string.Format("{0} x {1} ({2}%)", GetTickAsCurrencyString(STOCK_FOUR, TickType.BID), GetTickAsCurrencyString(STOCK_FOUR, TickType.ASK), GetAskBidPercentageDifference(STOCK_FOUR, TickType.BID, TickType.ASK));
                        tradePanel.stock4BidAskSizeOutput.Text = string.Format("{0} x {1}", GetTickAsCommaFormattedString(STOCK_FOUR, TickType.BID_SIZE), GetTickAsCommaFormattedString(STOCK_FOUR, TickType.ASK_SIZE));
                    });
                }
                else
                {
                    //tradePanel.Text = string.Join(Messages.TitleDivider, infoStrings);

                    tradePanel.stock4PositionOutputLabel.Text = stock4PositionSize.ToString("N0");
                    tradePanel.stock4LastPriceOutputLabel.Text = GetTickAsCurrencyString(STOCK_FOUR, TickType.LAST);
                    tradePanel.stock4PercentageChangeOutputLabel.Text = "todo";
                    tradePanel.stock4BidAskOutput.Text = string.Format("{0} x {1} ({2}%)", GetTickAsCurrencyString(STOCK_FOUR, TickType.BID), GetTickAsCurrencyString(STOCK_FOUR, TickType.ASK), GetAskBidPercentageDifference(STOCK_FOUR, TickType.BID, TickType.ASK));
                    tradePanel.stock4BidAskSizeOutput.Text = string.Format("{0} x {1}", GetTickAsCommaFormattedString(STOCK_FOUR, TickType.BID_SIZE), GetTickAsCommaFormattedString(STOCK_FOUR, TickType.ASK_SIZE));
                }
            }
        }



        private string GetTickAsString(int stockNum, int tickType)
        {
            return GetTickAsFormattedString(stockNum, tickType, v => v.ToString());
        }

        private string GetTickAsCurrencyString(int stockNum, int tickType)
        {
            return GetTickAsFormattedString(stockNum, tickType, v => v.ToCurrencyString());
        }

        private string GetTickAsCommaFormattedString(int stockNum, int tickType)
        {
            return GetTickAsFormattedString(stockNum, tickType, v => v.ToCommaFormattedNumberString());
        }

        private string GetTickAsFormattedString(int stockNum, int tickType, Func<double, string> messageFormatter)
        {
            double? tick = service.GetTick(stockNum, tickType);
            return tick.HasValue && tick.Value >= 0
                ? messageFormatter(tick.Value)
                : Messages.TitleUnavailable;
        }

        private string GetAskBidPercentageDifference(int stockNum, int bidTickType, int askTickType)
        {
            double? bidTick = service.GetTick(stockNum, bidTickType);
            double? askTick = service.GetTick(stockNum, askTickType);

            if (bidTick.HasValue && askTick.HasValue)
            {
                double askBidPercentageDiff = ((askTick.Value - bidTick.Value) / askTick.Value * 100);
                string askBidPercentageDiffStr = Math.Round(askBidPercentageDiff, 2).ToString();
                return askBidPercentageDiffStr;
            }
            else
            {
                return "N/A";
            }
        }

    }
}
