using IBApi;
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

        private System.Windows.Forms.TextBox textBox1;

        public TradeController(System.Windows.Forms.TextBox textBox)
        {

            service = new TradeService(Preferences.ClientId);
            menu = new TradeMenu(this);//Rick TODO: remove this for GUI 
            //Rick TODO: Move statusBar to TradePanel  
            statusBar = new TradeStatusBar(this, service);

            //Rick: GUI
            textBox1 = textBox;

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
            IO.ShowMessageTextBox(textBox1, Messages.WelcomeMessage);//Rick:GUI
            IO.ShowMessage(Messages.WelcomeMessage);
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
                service.PlaceBuyLimitOrder(Shares, TickType.ASK, sellStopPrice);
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
                service.PlaceSellLimitOrder(Shares, TickType.BID, buyStopPrice);
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
                service.PlaceBuyStopLimitOrder(Shares, TickType.ASK, buyStopPrice, sellStopPrice);
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
                service.PlaceSellStopLimitOrder(Shares, TickType.BID, sellStopPrice, buyStopPrice);
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

        public async Task CloseTwoThirdsPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.67);
        }     

        public async Task CloseHalfPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.5);
        }

        public async Task CloseThirdPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.33);
        }

        public async Task CloseQuarterPositionCommand(string[] args)
        {
            await ScalePositionAsync(-0.25);
        }

        public async Task LimitTakeProfitHalfCommand(string[] args)
        {
            string limitPriceInput = IO.PromptForInputIfNecessary(args, 0, Messages.LimitTakeProfitPrompt);
            double limitPrice = Double.Parse(limitPriceInput);

            if (Validation.HasValue(limitPrice))
            {
                await LimitTakeProfitAsync(0.5, limitPrice);
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
                await LimitTakeProfitAsync(0.33, limitPrice);
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
                await LimitTakeProfitAsync(0.25, limitPrice);
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
                    IO.ShowMessage(position.ToString());
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
                    IO.ShowMessage("Set Risk Per Trade to {0}%", newRisk);
                }
                else
                {
                    IO.ShowMessage("Risk Per Trade must be between 0.1% and 3%");
                }
            }
            else
            {
                IO.ShowMessage("Set Risk: {0} is not valid", newRiskInput);
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
                IO.ShowMessage("Set Equity to {0}", newEquity.ToCurrencyString());
            }
            else
            {
                IO.ShowMessage("Set Equity: {0} is not valid", newEquityInput);
            }           

            return Task.CompletedTask;
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

        private async Task LimitTakeProfitAsync(double percent, double limitPrice)
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
                    service.PlaceTakeProfitLimitOrder(OrderActions.SELL, orderQuantity, limitPrice);
                }
                else if (orderDelta < 0)
                {
                    service.PlaceTakeProfitLimitOrder(OrderActions.BUY, orderQuantity, limitPrice);
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

        //Rick: Called after AccountSummary has found the account with the highest Available Funds
        private async void OnAccountsChanged(PropertyChangedEventArgs eventArgs)
        {
            service.TradedAccount = service.MaxAvailableFundsAccount;

            Position largestPosition = await service.RequestLargestPosition();
            SetPosition(largestPosition);

            // Warn about multiple accounts
            if (service.Accounts.Length > 1)
            {
                IO.ShowMessage(LogLevel.Warn, Messages.MultipleAccountsWarningFormat, service.TradedAccount, service.TotalEquity.ToCurrencyString());                
            }
            else
            {
                IO.ShowMessage(LogLevel.Warn, Messages.SingleAccountFoundFormat, service.TradedAccount, service.TotalEquity.ToCurrencyString());
            }
            
            IO.ShowMessage(LogLevel.Warn, "Using " + service.TotalEquity.ToCurrencyString() + " as Account Size with " + service.RiskPercent + "% Risk Per Trade");

            // Show account type message
            if (service.TradedAccount.StartsWith(Messages.PaperAccountPrefix, StringComparison.InvariantCulture))
            {
                IO.ShowMessage(LogLevel.Warn, Messages.AccountTypePaper);
            }
            else
            {
                IO.ShowMessage(LogLevel.Warn, Messages.AccountTypeLive);
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

            //RICK: TODO Investigate -  this seems to clash with input currently in the console
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

        internal void SetTextBox()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}