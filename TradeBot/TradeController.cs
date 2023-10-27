﻿using IBApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradeBot.Events;
using TradeBot.Extensions;
using TradeBot.FileIO;
using TradeBot.Generated;
using TradeBot.Gui;
using TradeBot.TwsAbstractions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static TradeBot.AppProperties;
using TextBox = System.Windows.Forms.TextBox;

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

        public TradeService service;
        private TradeMenu menu;
        public TradeStatusBar statusBar;

        private TextBox globalOutputTextBox;

        public TradeController(TextBox textBox, GroupBox stock1GroupBox)
        {

            service = new TradeService(Preferences.ClientId, textBox, stock1GroupBox);
            //menu = new TradeMenu(this);//Rick TODO: remove this for GUI 
            //Rick TODO: Moved TradePanel  
            //statusBar = new TradeStatusBar(this, service);

            //Rick: GUI
            globalOutputTextBox = textBox;

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
        public void Run()
        {
            IO.ShowMessageTextBox(globalOutputTextBox, Messages.WelcomeMessage);//Rick:GUI
            //IO.ShowMessageCLI(Messages.WelcomeMessage);
            service.Connect(Preferences.ClientUrl, Preferences.ClientPort);

            
            //Rick: Not using Console menu in GUI mode 
            //while (service.IsConnected)
            //{
            //    await menu.Run();
            //}

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
                service.RequestStockContractDetails(tickerSymbol);//Rick
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
                service.RequestStockContractDetails(tickerSymbol);//Rick: We need stock contract for market data
                service.RequestCFDContractDetails(tickerSymbol);//Rick
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

        //Rick: Long at the current price with attached Sell Stop
        public Task BuyCommand(string[] args)
        {
            string sellStopPriceInput = IO.PromptForInputIfNecessary(args, 0, Messages.SellStopPrompt);
            double sellStopPrice = Double.Parse(sellStopPriceInput);

            if (Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(service, COMMON_TICKS))
            {
                //Rick: Not used in GUI version
                //service.PlaceBuyLimitOrder(Shares, TickType.ASK, sellStopPrice);
            }

            return Task.CompletedTask;
        }

        //Rick: Short at the current price with attached BuyStop
        public Task SellCommand(string[] args)
        {
            string buyStopPriceInput = IO.PromptForInputIfNecessary(args, 0, Messages.BuyStopPrompt);
            double buyStopPrice = Double.Parse(buyStopPriceInput);

            if (Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(service, COMMON_TICKS))
            {
                //Rick: Not used in GUI version
                //service.PlaceSellLimitOrder(Shares, TickType.BID, buyStopPrice);
            }

            return Task.CompletedTask;
        }

        //rick
        public Task BuyStopCommand(string[] args)
        {
            string buyStopPriceInput = IO.PromptForInputIfNecessary(args, 0, Messages.BuyStopPrompt);
            double buyStopPrice = Double.Parse(buyStopPriceInput);

            string sellStopPriceInput = IO.PromptForInputIfNecessary(args, 1, Messages.SellStopPrompt);
            double sellStopPrice = Double.Parse(sellStopPriceInput);

            if (Validation.HasValue(buyStopPrice)
                && Validation.HasValue(sellStopPrice)
                && Validation.TickerSet(service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(service, COMMON_TICKS))
            {
                //Rick: Not used in GUI version
                //service.PlaceBuyStopLimitOrder(Shares, TickType.ASK, buyStopPrice, sellStopPrice);
            }

            return Task.CompletedTask;
        }

        //rick
        public Task SellStopCommand(string[] args)
        {
            string sellStopPriceInput = IO.PromptForInputIfNecessary(args, 0, Messages.SellStopPrompt);
            double sellStopPrice = Double.Parse(sellStopPriceInput);

            string buyStopPriceInput = IO.PromptForInputIfNecessary(args, 1, Messages.BuyStopPrompt);
            double buyStopPrice = Double.Parse(buyStopPriceInput);

            if (Validation.HasValue(sellStopPrice)
                && Validation.HasValue(buyStopPrice)
                && Validation.TickerSet(service)
                //&& Validation.SharesSet(Shares)
                && Validation.TickDataAvailable(service, COMMON_TICKS))
            {
                //Rick: Not used in GUI version
                //service.PlaceSellStopLimitOrder(Shares, TickType.BID, sellStopPrice, buyStopPrice);
            }

            return Task.CompletedTask;
        }

        //TODO: Remove old menu functions
        public async Task ReversePositionCommand(string[] args)
        {
            await ScalePositionAsync(-2.0, true);
        }

        public async Task ClosePositionCommand(string[] args)
        {
            await ScalePositionAsync(-1.0, true);
        }

        public async Task CloseTwoThirdsPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.67, true);
        }     

        public async Task CloseHalfPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.5, true);
        }

        public async Task CloseThirdPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.33, true);
        }

        public async Task CloseQuarterPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.25, true);
        }

        public async Task LimitTakeProfitHalfCommand(string[] args)
        {
            string limitPriceInput = IO.PromptForInputIfNecessary(args, 0, Messages.LimitTakeProfitPrompt);
            double limitPrice = Double.Parse(limitPriceInput);

            if (Validation.HasValue(limitPrice))
            {
                await LimitTakeProfitAsync(0.5, limitPrice, true);
            }
            else
            {
                return;
            }           
        }

        public async Task LimitTakeProfitThirdCommand(string[] args)
        {
            string limitPriceInput = IO.PromptForInputIfNecessary(args, 0, Messages.LimitTakeProfitPrompt);
            double limitPrice = Double.Parse(limitPriceInput);

            if (Validation.HasValue(limitPrice))
            {
                await LimitTakeProfitAsync(0.33, limitPrice, true);
            }
            else
            {
                return;
            }
        }

        public async Task LimitTakeProfitQuarterCommand(string[] args)
        {
            string limitPriceInput = IO.PromptForInputIfNecessary(args, 0, Messages.LimitTakeProfitPrompt);
            double limitPrice = Double.Parse(limitPriceInput);

            if (Validation.HasValue(limitPrice))
            {
                await LimitTakeProfitAsync(0.25, limitPrice, true);
            }
            else
            {
                return;
            }
        }

        public async Task ListPositionsCommand(string[] args)
        {
            IEnumerable<Position> positions = await service
                .RequestPositionsAsync();

            if (Validation.PositionsExist(positions))
            {
                foreach (var position in positions)
                {
                    IO.ShowMessageCLI(position.ToString());
                }
            }
        }

        public Task SetRisk(string[] args)
        {
            string newRiskInput = IO.PromptForInputIfNecessary(args, 0, Messages.NewRiskPrompt);
            double newRisk = Double.Parse(newRiskInput);

            if (Validation.HasValue(newRisk))
            {
                if (newRisk >= 0.1 && newRisk <= 3.0)
                {
                    service.RiskPercent = newRisk;
                    IO.ShowMessageCLI("Set Risk Per Trade to {0}%", newRisk);
                }
                else
                {
                    IO.ShowMessageCLI("Risk Per Trade must be between 0.1% and 3%");
                }
            }
            else
            {
                IO.ShowMessageCLI("Set Risk: {0} is not valid", newRiskInput);
            }

            return Task.CompletedTask;
        }

        public Task SetEquity(string[] args)
        {
            string newEquityInput = IO.PromptForInputIfNecessary(args, 0, Messages.NewEquityPrompt);
            double newEquity = double.Parse(newEquityInput);

            if (Validation.HasValue(newEquity) && Validation.Positive(newEquity))
            { 
                service.TotalEquity = newEquity;
                IO.ShowMessageCLI("Set Equity to {0}", newEquity.ToCurrencyString());
            }
            else
            {
                IO.ShowMessageCLI("Set Equity: {0} is not valid", newEquityInput);
            }           

            return Task.CompletedTask;
        }

        public Task LoadStateCommand(string[] args)
        {
            AppState state = PropertySerializer.Deserialize<AppState>(PropertyFiles.STATE_FILE);

            service.TickerSymbol = state.TickerSymbol;
            Cash = state.Cash ?? 0;
            Shares = state.Shares ?? 10;

            IO.ShowMessageCLI(Messages.LoadedStateFormat, PropertyFiles.STATE_FILE);

            return Task.CompletedTask;
        }

        public Task SaveStateCommand(string[] args)
        {
            AppState state = new AppState();
            state.TickerSymbol = service.TickerSymbol;
            state.Shares = Shares;
            state.Cash = Cash;

            PropertySerializer.Serialize(state, PropertyFiles.STATE_FILE);

            IO.ShowMessageCLI(Messages.SavedStateFormat, PropertyFiles.STATE_FILE);

            return Task.CompletedTask;
        }

        public Task ClearScreenCommand(string[] args)
        {
            Console.Clear();

            return Task.CompletedTask;
        }

        public Task ShowMenuCommand(string[] args)
        {
            IO.ShowMessageCLI(menu.Render());

            return Task.CompletedTask;
        }
        #endregion

        #region Private helper methods
        private void SetPosition(Position position)
        {
            service.TickerSymbol = position?.Symbol ?? null;
            Shares = position?.PositionSize ?? 10;
        }

        public async Task SetInitialSharesAsync()
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
                IO.ShowMessageTextBox(globalOutputTextBox, string.Format(Messages.TimeoutErrorFormat, TimeSpan.FromMilliseconds(REQUEST_TIMEOUT).TotalSeconds));//GUI
                //IO.ShowMessageCLI(LogLevel.Error, Messages.TimeoutErrorFormat, TimeSpan.FromMilliseconds(REQUEST_TIMEOUT).TotalSeconds);
            }
        }

        public async Task ScalePositionAsync(double percent, bool outsideRth)
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
                    service.PlaceCloseLimitOrder(OrderActions.BUY, orderQuantity, TickType.ASK, outsideRth);
                }
                else if (orderDelta < 0)
                {
                    service.PlaceCloseLimitOrder(OrderActions.SELL, orderQuantity, TickType.BID, outsideRth);
                }
            }
        }

        public async Task LimitTakeProfitAsync(double percent, double limitPrice, bool outsideRth)
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
                    service.PlaceTakeProfitLimitOrder(OrderActions.SELL, orderQuantity, limitPrice, outsideRth);
                }
                else if (orderDelta < 0)
                {
                    service.PlaceTakeProfitLimitOrder(OrderActions.BUY, orderQuantity, limitPrice, outsideRth);
                }
            }
        }

        public Task CancelLastOrderCommand(string[] args)
        {
            service.CancelLastOrder();
            return Task.CompletedTask;
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
                case nameof(service.MaxAvailableFundsAccount): //Rick: formerly nameof(service.Accounts):
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
            //IO.ShowMessageCLI(Messages.SharesSetFormat, Shares);
        }

        private void OnCashChanged(PropertyChangedEventArgs eventArgs)
        {
            //IO.ShowMessageCLI(Messages.CashSetFormat, Cash.ToCurrencyString());
        }

        private void OnIsConnectedChanged(PropertyChangedEventArgs eventArgs)
        {
            if (service.IsConnected)
            {
                IO.ShowMessageTextBox(globalOutputTextBox, Messages.TwsConnected); //GUI
                //IO.ShowMessageCLI(LogLevel.Trace, Messages.TwsConnected);
            }
            else
            {
                IO.ShowMessageTextBox(globalOutputTextBox, Messages.TwsDisconnected);//GUI
                //IO.ShowMessageCLI(LogLevel.Fatal, Messages.TwsDisconnected);
            }
        }

        //Rick: Called after AccountSummary has found the account with the highest Available Funds
        private async void OnAccountsChanged(PropertyChangedEventArgs eventArgs)
        {
            service.TradedAccount = service.MaxAvailableFundsAccount;

            Position largestPosition = await service.RequestLargestPosition();
            SetPosition(largestPosition);

            // Warn about multiple accounts
            if (service.Accounts.Length > 1)
            {
                IO.ShowMessageTextBox(globalOutputTextBox, string.Format(Messages.MultipleAccountsWarningFormat, service.TradedAccount, service.TotalEquity.ToCurrencyString()));//GUI
                //IO.ShowMessageCLI(LogLevel.Warn, Messages.MultipleAccountsWarningFormat, service.TradedAccount, service.TotalEquity.ToCurrencyString());                
            }
            else
            {
                IO.ShowMessageTextBox(globalOutputTextBox, string.Format(Messages.SingleAccountFoundFormat, service.TradedAccount, service.TotalEquity.ToCurrencyString()));//GUI
                //IO.ShowMessageCLI(LogLevel.Warn, Messages.SingleAccountFoundFormat, service.TradedAccount, service.TotalEquity.ToCurrencyString());
            }

            IO.ShowMessageTextBox(globalOutputTextBox, "Using " + service.TotalEquity.ToCurrencyString() + " as Account Size");//GUI
            //IO.ShowMessageCLI(LogLevel.Warn, "Using " + service.TotalEquity.ToCurrencyString() + " as Account Size with " + service.RiskPercent + "% Risk Per Trade");

            // Show account type message
            if (service.TradedAccount.StartsWith(Messages.PaperAccountPrefix, StringComparison.InvariantCulture))
            {
                IO.ShowMessageTextBox(globalOutputTextBox,Messages.AccountTypePaper); //GUI
                //IO.ShowMessageCLI(LogLevel.Warn, Messages.AccountTypePaper);
            }
            else
            {
                IO.ShowMessageTextBox(globalOutputTextBox, Messages.AccountTypeLive); //GUI
                //IO.ShowMessageCLI(LogLevel.Warn, Messages.AccountTypeLive);
            }

        }

        private void OnTickerSymbolChanged(PropertyChangedEventArgs eventArgs)
        {
            var args = eventArgs as PropertyValueChangedEventArgs<string>;
            var oldValue = args.OldValue;
            var newValue = args.NewValue;

            if (!string.IsNullOrWhiteSpace(oldValue))
            {
                IO.ShowMessageTextBox(globalOutputTextBox, string.Format(Messages.TickerSymbolClearedFormat, oldValue));//GUI
                //IO.ShowMessageCLI(LogLevel.Trace, Messages.TickerSymbolClearedFormat, oldValue);
            }

            if (!string.IsNullOrWhiteSpace(newValue))
            {
                IO.ShowMessageTextBox(globalOutputTextBox, string.Format(Messages.TickerSymbolSetFormat, newValue));//GUI
                //IO.ShowMessageCLI(Messages.TickerSymbolSetFormat, newValue);
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

            IO.ShowMessageTextBox(globalOutputTextBox, string.Format(Messages.CommissionFormat, lastCommission.ToCurrencyString(), totalCommission.ToCurrencyString()));//GUI
            //IO.ShowMessageCLI(Messages.CommissionFormat, lastCommission.ToCurrencyString(), totalCommission.ToCurrencyString());
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
                IO.ShowMessageTextBox(globalOutputTextBox, string.Format(Messages.TwsErrorFormat, errorMessage)); //GUI
                //IO.ShowMessageCLI(LogLevel.Error, Messages.TwsErrorFormat, errorMessage);
            }
            if (exception != null)
            {
                IO.ShowMessageTextBox(globalOutputTextBox, exception.ToString());//GUI
                //IO.ShowMessageCLI(LogLevel.Error, exception.ToString());
            }

            // Post-output
            switch (errorCode)
            {
                case ErrorCodes.TICKER_NOT_FOUND:
                    Shares = 10;
                    break;
            }
        }

        internal void SetTextBox()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}