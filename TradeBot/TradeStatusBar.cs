using IBApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradeBot.Events;
using TradeBot.Extensions;
using TradeBot.TwsAbstractions;
using TradeBot.WinGui;
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
            IList<string> infoStrings = new List<string>();

            string appName = Messages.AppName;
            if (!string.IsNullOrWhiteSpace(appName))
            {
                infoStrings.Add(appName);
            }

            bool hasTickerSymbol = service.HasTickerSymbol;
            string tickerSymbol = service.Stock1TickerSymbol;
            string tickerDisplayValue = hasTickerSymbol ? tickerSymbol : Messages.TitleUnavailable;
            infoStrings.Add(string.Format(Messages.TitleRiskPerTrade, service.RiskPercent));
            infoStrings.Add(string.Format(Messages.TitleTickerSymbol, tickerDisplayValue));

            infoStrings.Add(string.Format(Messages.TitleShares, controller.Shares));

            if (hasTickerSymbol)
            {
                Position currentPosition = await service.RequestCurrentPositionAsync();
                double positionSize = currentPosition?.PositionSize ?? 0;
                infoStrings.Add(string.Format(Messages.TitlePositionSize, positionSize));

                infoStrings.Add(string.Format(Messages.TitleLastFormat, GetTickAsCurrencyString(TickType.LAST)));
                infoStrings.Add(string.Format(Messages.TitleBidAskFormat, GetTickAsCurrencyString(TickType.BID), GetTickAsCurrencyString(TickType.ASK), GetTickAsCommaFormattedString(TickType.BID_SIZE), GetTickAsCommaFormattedString(TickType.ASK_SIZE)));
                infoStrings.Add(string.Format(Messages.TitleVolumeFormat, GetTickAsCommaFormattedString(TickType.VOLUME)));
                infoStrings.Add(string.Format(Messages.TitleCloseFormat, GetTickAsCurrencyString(TickType.CLOSE)));
                infoStrings.Add(string.Format(Messages.TitleOpenFormat, GetTickAsCurrencyString(TickType.OPEN)));

                if (tradePanel.InvokeRequired)
                {
                    tradePanel.BeginInvoke(() =>
                    {
                        tradePanel.Text = string.Join(Messages.TitleDivider, infoStrings);

                        tradePanel.stock1PositionOutputLabel.Text = positionSize.ToString("N0");
                        tradePanel.stock1LastPriceOutputLabel.Text = GetTickAsCurrencyString(TickType.LAST);
                        tradePanel.stock1PercentageChangeOutputLabel.Text = "todo";
                        tradePanel.stock1BidAskOutput.Text = string.Format("{0} x {1}", GetTickAsCurrencyString(TickType.BID), GetTickAsCurrencyString(TickType.ASK));
                        tradePanel.stock1BidAskSizeOutput.Text = string.Format("{0} x {1}", GetTickAsCommaFormattedString(TickType.BID_SIZE), GetTickAsCommaFormattedString(TickType.ASK_SIZE));
                    });
                }
                else
                {
                    tradePanel.Text = string.Join(Messages.TitleDivider, infoStrings);

                    tradePanel.stock1PositionOutputLabel.Text = positionSize.ToString("N0");
                    tradePanel.stock1LastPriceOutputLabel.Text = GetTickAsCurrencyString(TickType.LAST);
                    tradePanel.stock1PercentageChangeOutputLabel.Text = "todo";
                    tradePanel.stock1BidAskOutput.Text = string.Format("{0} x {1}", GetTickAsCurrencyString(TickType.BID), GetTickAsCurrencyString(TickType.ASK));
                    tradePanel.stock1BidAskSizeOutput.Text = string.Format("{0} x {1}", GetTickAsCommaFormattedString(TickType.BID_SIZE), GetTickAsCommaFormattedString(TickType.ASK_SIZE));
                }
            }           
        }



        private string GetTickAsString(int tickType)
        {
            return GetTickAsFormattedString(tickType, v => v.ToString());
        }

        private string GetTickAsCurrencyString(int tickType)
        {
            return GetTickAsFormattedString(tickType, v => v.ToCurrencyString());
        }

        private string GetTickAsCommaFormattedString(int tickType)
        {
            return GetTickAsFormattedString(tickType, v => v.ToCommaFormattedNumberString());
        }

        private string GetTickAsFormattedString(int tickType, Func<double, string> messageFormatter)
        {
            double? tick = service.GetTick(tickType);
            return tick.HasValue && tick.Value >= 0
                ? messageFormatter(tick.Value)
                : Messages.TitleUnavailable;
        }
    }
}
