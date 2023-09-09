﻿using IBApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeBot.Events;
using TradeBot.Extensions;
using TradeBot.TwsAbstractions;
using static TradeBot.AppProperties;

namespace TradeBot
{
    public class TradeStatusBar
    {
        private TradeController controller;
        private TradeService service;

        public TradeStatusBar(TradeController controller, TradeService service)
        {
            this.controller = controller;
            this.service = service;

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
                case nameof(service.TickerSymbol):
                    await UpdateHeaderAsync();
                    break;
            }
        }

        private async void OnTickUpdated(int tickType, double value)
        {
            await UpdateHeaderAsync();
        }

        private async void OnPositionUpdated(Position position)
        {
            await UpdateHeaderAsync();
        }

        private async Task UpdateHeaderAsync()
        {
            IList<string> infoStrings = new List<string>();

            string appName = Messages.AppName;
            if (!string.IsNullOrWhiteSpace(appName))
            {
                infoStrings.Add(appName);
            }

            bool hasTickerSymbol = service.HasTickerSymbol;
            string tickerSymbol = service.TickerSymbol;
            string tickerDisplayValue = hasTickerSymbol ? tickerSymbol : Messages.TitleUnavailable;
            infoStrings.Add(string.Format(Messages.TitleRiskPerTrade, service.riskPercent));
            infoStrings.Add(string.Format(Messages.TitleTickerSymbol, tickerDisplayValue));

            infoStrings.Add(string.Format(Messages.TitleShares, controller.Shares));

            if (hasTickerSymbol)
            {
                Position currentPosition = await service.RequestCurrentPositionAsync();
                double positionSize = currentPosition?.PositionSize ?? 0;
                infoStrings.Add(string.Format(Messages.TitlePositionSize, positionSize));

                infoStrings.Add(string.Format(Messages.TitleLastFormat, GetTickAsCurrencyString(TickType.LAST)));
                infoStrings.Add(string.Format(Messages.TitleBidAskFormat, GetTickAsString(TickType.BID_SIZE), GetTickAsCurrencyString(TickType.BID), GetTickAsCurrencyString(TickType.ASK), GetTickAsString(TickType.ASK_SIZE)));
                infoStrings.Add(string.Format(Messages.TitleVolumeFormat, GetTickAsString(TickType.VOLUME)));
                infoStrings.Add(string.Format(Messages.TitleCloseFormat, GetTickAsCurrencyString(TickType.CLOSE)));
                infoStrings.Add(string.Format(Messages.TitleOpenFormat, GetTickAsCurrencyString(TickType.OPEN)));
            }

            Console.Title = string.Join(Messages.TitleDivider, infoStrings);
        }

        private string GetTickAsString(int tickType)
        {
            return GetTickAsFormattedString(tickType, v => v.ToString());
        }

        private string GetTickAsCurrencyString(int tickType)
        {
            return GetTickAsFormattedString(tickType, v => v.ToCurrencyString());
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
