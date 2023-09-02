﻿using IBApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeBot.Events;
using TradeBot.Extensions;
using TradeBot.FileIO;
using TradeBot.Generated;
using TradeBot.Gui;
using TradeBot.TwsAbstractions;
using static TradeBot.AppProperties;

namespace TradeBot
{
    public class TradeController
    {
        private const int REQUEST_TIMEOUT = (int)(1.5 * 1000);

        private static readonly int[] COMMON_TICKS = {
            TickType.LAST,
            TickType.ASK,
            TickType.BID
        };

        private TradeService service;
        private TradeMenu menu;
        private TradeStatusBar statusBar;

        public TradeController()
        {
            service = new TradeService(Preferences.ClientId);
            menu = new TradeMenu(this);
            statusBar = new TradeStatusBar(this, service);

            PropertyChanged += OnPropertyChanged;
            service.PropertyChanged += OnPropertyChanged;
            service.Error += OnError;
        }

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        private double _shares;
        public double Shares
        {
            get
            {
                return _shares;
            }
            set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _shares, value);
            }
        }

        private double _cash;
        public double Cash
        {
            get
            {
                return _cash;
            }
            set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _cash, value);
            }
        }
        #endregion

        #region Public methods
        public async Task Run()
        {
            IO.ShowMessage(Messages.WelcomeMessage);
            service.Connect(Preferences.ClientUrl, Preferences.ClientPort);
            while (service.IsConnected)
            {
                await menu.Run();
            }
        }
        #endregion

        #region Menu commands
        public async Task PromptForTickerSymbolCommand(string[] args)
        {
            string tickerSymbol = IO.PromptForInputIfNecessary(args, 0, Messages.SelectTickerPrompt);

            if (Validation.NotNullOrWhiteSpace(tickerSymbol))
            {
                service.UseCFD = false;

                service.TickerSymbol = tickerSymbol;
                await SetInitialSharesAsync();
            }
        }

        //Rick - we need to treat a CFD as a regular stock ticker until just before we place order so we can get prices
        public async Task PromptForCFDtickerSymbolCommand(string[] args)
        {
            string tickerSymbol = IO.PromptForInputIfNecessary(args, 0, Messages.SelectTickerPrompt);

            if (Validation.NotNullOrWhiteSpace(tickerSymbol))
            {
                service.UseCFD = true;

                service.TickerSymbol = tickerSymbol;
                await SetInitialSharesAsync();
            }
        }

        public async Task PromptForCashCommand(string[] args)
        {
            string cashInput = IO.PromptForInputIfNecessary(args, 0, Messages.CashPrompt);
            double? cash = cashInput.ToDouble();

            if (Validation.HasValue(cash)
                && Validation.Positive(cash ?? -1))
            {
                Cash = cash.Value;

                if (service.HasTickerSymbol)
                {
                    await SetSharesFromCashAsync();
                }
            }
        }

        public Task PromptForSharesCommand(string[] args)
        {
            string sharesInput = IO.PromptForInputIfNecessary(args, 0, Messages.SharesPrompt);
            int? shares = sharesInput.ToInt();

            if (Validation.HasValue(shares)
                && Validation.Positive(shares ?? -1))
            {
                Shares = shares.Value;
            }

            return Task.CompletedTask;
        }

        public async Task SetSharesFromPositionCommand(string[] args)
        {
            if (Validation.TickerSet(service))
            {
                Position currentPosition = await service
                    .RequestCurrentPositionAsync();

                if (Validation.PositionExists(currentPosition))
                {
                    Shares = currentPosition.PositionSize;
                }
            }
        }

        public Task BuyCommand(string[] args)
        {
            double sellStopPrice = Double.Parse(args[0]);
            //Rick TODO - check sell stop price is valid

            if (Validation.TickerSet(service)
                && Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(service, COMMON_TICKS))
            {
                service.PlaceBuyLimitOrder(Shares, TickType.ASK, sellStopPrice);
            }

            return Task.CompletedTask;
        }

        //rick
        public Task BuyStopCommand(string[] args)
        {

            double buyStopPrice = Double.Parse(args[0]);
            double sellStopPrice = Double.Parse(args[1]);
            if (Validation.TickerSet(service)
                && Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(service, COMMON_TICKS))
            {
                service.PlaceBuyStopLimitOrder(Shares, TickType.ASK, buyStopPrice, sellStopPrice);
            }

            return Task.CompletedTask;
        }

        //rick
        public Task SellStopCommand(string[] args)
        {

            double sellStopPrice = Double.Parse(args[0]);
            double buyStopPrice = Double.Parse(args[1]);
            if (Validation.TickerSet(service)
                && Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(service, COMMON_TICKS))
            {
                service.PlaceSellStopLimitOrder(Shares, TickType.BID, sellStopPrice, buyStopPrice);
            }

            return Task.CompletedTask;
        }

        public Task SellCommand(string[] args)
        {
            double sellStopPrice = Double.Parse(args[0]);
            //Rick TODO - check sell stop price is valid

            if (Validation.TickerSet(service)
                && Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(service, COMMON_TICKS))
            {
                service.PlaceSellLimitOrder(Shares, TickType.BID, sellStopPrice);
            }

            return Task.CompletedTask;
        }

        public async Task ReversePositionCommand(string[] args)
        {
            await ScalePositionAsync(-2);
        }

        public async Task ClosePositionCommand(string[] args)
        {
            await ScalePositionAsync(-1);
        }

        public async Task CloseHalfPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.5);
        }

        public async Task CloseThirdPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.33);
        }

        public async Task CloseTwoThirdsPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.67);
        }

        public async Task ListPositionsCommand(string[] args)
        {
            IEnumerable<Position> positions = await service
                .RequestPositionsAsync();

            if (Validation.PositionsExist(positions))
            {
                foreach (var position in positions)
                {
                    IO.ShowMessage(position.ToString());
                }
            }
        }

        public Task LoadStateCommand(string[] args)
        {
            AppState state = PropertySerializer.Deserialize<AppState>(PropertyFiles.STATE_FILE);

            service.TickerSymbol = state.TickerSymbol;
            Cash = state.Cash ?? 0;
            Shares = state.Shares ?? 10;

            IO.ShowMessage(Messages.LoadedStateFormat, PropertyFiles.STATE_FILE);

            return Task.CompletedTask;
        }

        public Task SaveStateCommand(string[] args)
        {
            AppState state = new AppState();
            state.TickerSymbol = service.TickerSymbol;
            state.Shares = Shares;
            state.Cash = Cash;

            PropertySerializer.Serialize(state, PropertyFiles.STATE_FILE);

            IO.ShowMessage(Messages.SavedStateFormat, PropertyFiles.STATE_FILE);

            return Task.CompletedTask;
        }

        public Task ClearScreenCommand(string[] args)
        {
            Console.Clear();

            return Task.CompletedTask;
        }

        public Task ShowMenuCommand(string[] args)
        {
            IO.ShowMessage(menu.Render());

            return Task.CompletedTask;
        }
        #endregion

        #region Private helper methods
        private void SetPosition(Position position)
        {
            service.TickerSymbol = position?.Symbol ?? null;
            Shares = position?.PositionSize ?? 10;
        }

        private async Task SetInitialSharesAsync()
        {
            if (!service.HasTickerSymbol)
            {
                return;
            }

            Position existingPosition = await service.RequestCurrentPositionAsync();
            double existingPositionSize = existingPosition?.PositionSize ?? 0;
            if (existingPositionSize > 0)
            {
                Shares = existingPositionSize;
            }
            else if (Cash > 0)
            {
                await SetSharesFromCashAsync();
            }
        }

        private async Task SetSharesFromCashAsync()
        {
            await Timeout(service.HasTicksAsync(COMMON_TICKS));

            int tickType = TickType.LAST;
            if (Validation.TickDataAvailable(service, tickType))
            {
                double sharePrice = service.GetTick(tickType).Value;
                if (sharePrice > 0)
                {
                    Shares = (int)Math.Floor(Cash / sharePrice);
                }
            }
        }

        private async Task Timeout(Task task)
        {
            try
            {
                await task.TimeoutAfter(REQUEST_TIMEOUT);
            }
            catch (TimeoutException)
            {
                IO.ShowMessage(LogLevel.Error, Messages.TimeoutErrorFormat,
                               TimeSpan.FromMilliseconds(REQUEST_TIMEOUT).TotalSeconds);
            }
        }

        private async Task ScalePositionAsync(double percent)
        {
            Position position = await service.RequestCurrentPositionAsync();
            if (Validation.TickerSet(service)
                && Validation.PositionExists(position)
                && Validation.TickDataAvailable(service, COMMON_TICKS))
            {
                int orderDelta = (int)Math.Round(position.PositionSize * percent);
                int orderQuantity = Math.Abs(orderDelta);

                if (orderDelta > 0)
                {
                    service.PlaceCloseLimitOrder(OrderActions.BUY, orderQuantity, TickType.ASK);
                }
                else if (orderDelta < 0)
                {
                    service.PlaceCloseLimitOrder(OrderActions.SELL, orderQuantity, TickType.BID);
                }
            }
        }
        #endregion

        #region Event handlers
        private void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(Shares):
                    OnSharesChanged(eventArgs);
                    break;
                case nameof(Cash):
                    OnCashChanged(eventArgs);
                    break;

                case nameof(service.IsConnected):
                    OnIsConnectedChanged(eventArgs);
                    break;
                case nameof(service.Accounts):
                    OnAccountsChanged(eventArgs);
                    break;
                case nameof(service.TickerSymbol):
                    OnTickerSymbolChanged(eventArgs);
                    break;
                case nameof(service.CommissionReports):
                    OnCommissionReportsChanged(eventArgs);
                    break;
            }
        }

        private void OnSharesChanged(PropertyChangedEventArgs eventArgs)
        {
            IO.ShowMessage(Messages.SharesSetFormat, Shares);
        }

        private void OnCashChanged(PropertyChangedEventArgs eventArgs)
        {
            IO.ShowMessage(Messages.CashSetFormat, Cash.ToCurrencyString());
        }

        private void OnIsConnectedChanged(PropertyChangedEventArgs eventArgs)
        {
            if (service.IsConnected)
            {
                IO.ShowMessage(LogLevel.Trace, Messages.TwsConnected);
            }
            else
            {
                IO.ShowMessage(LogLevel.Fatal, Messages.TwsDisconnected);
            }
        }

        //Rick: The accounts received from IBApi.EWrapper.managedAccounts
        private async void OnAccountsChanged(PropertyChangedEventArgs eventArgs)
        {
            string[] accounts = service.Accounts;
            if (accounts != null && accounts.Length > 0)
            {
                //Rick: TODO: Use Account summary to get AvaialbleFunds from each account and choose account with highest amount - may have to move code below to end of accountSummary
                string tradedAccount = accounts[0];//Rick: Temp Hack to get the traded account with the money
                service.TradedAccount = tradedAccount;

                Position largestPosition = await service.RequestLargestPosition();
                SetPosition(largestPosition);

                // Warn about multiple accounts
                if (accounts.Length > 1)
                {
                    IO.ShowMessage(LogLevel.Error, Messages.MultipleAccountsWarningFormat, tradedAccount);
                }

                // Show account type message
                if (tradedAccount.StartsWith(Messages.PaperAccountPrefix, StringComparison.InvariantCulture))
                {
                    IO.ShowMessage(LogLevel.Warn, Messages.AccountTypePaper);
                }
                else
                {
                    IO.ShowMessage(LogLevel.Error, Messages.AccountTypeLive);
                }
            }
        }

        private void OnTickerSymbolChanged(PropertyChangedEventArgs eventArgs)
        {
            var args = eventArgs as PropertyValueChangedEventArgs<string>;
            var oldValue = args.OldValue;
            var newValue = args.NewValue;

            if (!string.IsNullOrWhiteSpace(oldValue))
            {
                IO.ShowMessage(LogLevel.Trace, Messages.TickerSymbolClearedFormat, oldValue);
            }

            if (!string.IsNullOrWhiteSpace(newValue))
            {
                IO.ShowMessage(Messages.TickerSymbolSetFormat, newValue);
            }
        }

        private void OnCommissionReportsChanged(PropertyChangedEventArgs eventArgs)
        {
            IList<CommissionReport> reports = service.CommissionReports;
            if (reports.IsNullOrEmpty())
            {
                return;
            }

            CommissionReport lastReport = reports.Last();
            double lastCommission = lastReport.Commission;
            double totalCommission = reports.Sum(report => report.Commission);

            IO.ShowMessage(Messages.CommissionFormat,
                lastCommission.ToCurrencyString(),
                totalCommission.ToCurrencyString());
        }

        private void OnError(int id, int errorCode, string errorMessage, Exception exception)
        {
            // Pre-output
            switch (errorCode)
            {
                // Ignore common error codes
                case ErrorCodes.MARKET_DATA_FARM_DISCONNECTED:
                case ErrorCodes.MARKET_DATA_FARM_CONNECTED:
                case ErrorCodes.HISTORICAL_DATA_FARM_DISCONNECTED:
                case ErrorCodes.HISTORICAL_DATA_FARM_CONNECTED:
                case ErrorCodes.HISTORICAL_DATA_FARM_INACTIVE:
                case ErrorCodes.MARKET_DATA_FARM_INACTIVE:
                case ErrorCodes.TICKER_ID_NOT_FOUND:
                case ErrorCodes.CROSS_SIDE_WARNING:
                    return;
            }

            // Output
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                IO.ShowMessage(LogLevel.Error, Messages.TwsErrorFormat, errorMessage);
            }
            if (exception != null)
            {
                IO.ShowMessage(LogLevel.Error, exception.ToString());
            }

            // Post-output
            switch (errorCode)
            {
                case ErrorCodes.TICKER_NOT_FOUND:
                    Shares = 10;
                    break;
            }
        }
        #endregion
    }
}