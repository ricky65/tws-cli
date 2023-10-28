using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using TradeBot.Events;
using TradeBot.Extensions;
using TradeBot.Gui;
using TradeBot.TwsAbstractions;
using TradeBot.Utils;

namespace TradeBot
{
    public class TradeService : EWrapperImpl
    {
        private EReaderSignal readerSignal;
        private EClientSocket clientSocket;

        private Portfolio portfolio;
        private TaskCompletionSource<string> accountDownloadEndTCS;

        private int tickerId;
        private Contract stockContract;
        private Contract CFDContract;
        private TickData tickData;

        private int nextValidOrderId;

        //Offsets in cents for order types
        private double limitOffset = 0.03;
        private double buyStopOffset = 0.11;
        private double sellStopOffset = 0.11;

        //maps an Account to Available Funds in account
        Dictionary<string, double> accountAvailableFunds = new Dictionary<string, double>();

        //Rick - number of orders cancelled, so we can jump to a fresh order id when required
        private int canceledOrderCount = 0;

        private TextBox globalOutputTextBox;
        private GroupBox stock1GroupBoxx;
        public TradeService(int clientId, TextBox textBox, GroupBox stock1GroupBox)
        {
            ClientId = clientId;

            readerSignal = new EReaderMonitorSignal();
            clientSocket = new EClientSocket(this, readerSignal);

            globalOutputTextBox = textBox;
            stock1GroupBoxx = stock1GroupBox;

            // TradeBot events
            PropertyChanged += OnPropertyChanged;

            // EWrapperImpl events
            Error += OnError;
            ConnectAck += OnConnectAck;
            ConnectionClosed += OnConnectionClosed;
            ManagedAccounts += OnManagedAccounts;
            NextValidId += OnNextValidId;
            TickPrice += OnTickPrice;
            TickSize += OnTickSize;
            TickGeneric += OnTickGeneric;
            UpdatePortfolio += OnUpdatePortfolio;
            AccountDownloadEnd += OnAccountDownloadEnd;
            CommissionReport += OnCommissionReport;
            AccountSummary += OnAccountSummary;//Rick
            AccountSummaryEnd += OnAccountSummaryEnd;//Rick
            ContractDetails += OnContractDetails;//rick
            ContractDetailsEnd += OnContractDetailsEnd;//rick
        }

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event TickUpdatedEventHandler TickUpdated;
        public event PositionUpdatedEventHandler PositionUpdated;
        #endregion

        #region Properties

        private double totalEquity = 5_100;
        public double TotalEquity { get => totalEquity; set => totalEquity = value; }
        
        private double globalRiskPercent = 1.25;
        public double RiskPercent { get => globalRiskPercent; set => PropertyChanged.SetPropertyAndRaiseEvent(ref globalRiskPercent, value); }

        public int ClientId { get; }

        private bool _isConnected;
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            private set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _isConnected, value);
            }
        }

        private string[] _accounts;
        public string[] Accounts
        {
            get
            {
                return _accounts;
            }
            private set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _accounts, value);
            }
        }

        private string _maxAvailableFundsAccount;
        public string MaxAvailableFundsAccount
        {
            get
            {
                return _maxAvailableFundsAccount;
            }
            private set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _maxAvailableFundsAccount, value);
            }
        }

        private string _tradedAccount;
        public string TradedAccount
        {
            get
            {
                return _tradedAccount;
            }
            set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _tradedAccount, value);
            }
        }

        private string _tickerSymbol;
        public string TickerSymbol
        {
            get
            {
                return _tickerSymbol;
            }
            set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _tickerSymbol, value?.Trim().ToUpper());
            }
        }

        //Rick
        private bool _useCFD = false;
        public bool UseCFD
        {
            get
            {
                return _useCFD;
            }
            set
            {
                _useCFD = value;
            }
        }

        public bool HasTickerSymbol
        {
            get
            {
                return !string.IsNullOrWhiteSpace(TickerSymbol);
            }
        }

        private IList<CommissionReport> _commissionReports = new List<CommissionReport>();
        public IList<CommissionReport> CommissionReports
        {
            get
            {
                return _commissionReports;
            }
            private set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _commissionReports, value);
            }
        }
        #endregion

        #region Public methods
        public void Connect(string clientUrl, int clientPort)
        {
            clientSocket.eConnect(clientUrl, clientPort, ClientId);

            // Create a reader to consume incoming messages 
            // and store them in a queue.
            var reader = new EReader(clientSocket, readerSignal);
            reader.Start();

            // Once the messages are in the queue, an additional thread is needed to process them.
            // This is best accomplished by a dedicated long-running background thread.
            Task.Factory.StartNew(() =>
            {
                while (clientSocket.IsConnected())
                {
                    readerSignal.waitForSignal();
                    // It's worth noting that message processing is what raises all EWrapper events,
                    // e.g. NextValidId, TickPrice, Position, PositionEnd, Error, ConnectionClosed, etc.
                    // Events are handled by the thread that raises them, i.e. this thread!
                    reader.processMsgs();
                }
            },
            TaskCreationOptions.LongRunning);
        }

        public void Disconnect()
        {
            clientSocket.eDisconnect();
            IsConnected = false;
        }

        public void PlaceBuyLimitOrder(double quantity, int tickType, double sellStopPrice, double riskPercent, bool outsideRth)
        {
            PlaceLimitOrder(OrderActions.BUY, quantity, tickType, sellStopPrice, riskPercent, outsideRth);
        }
        
        public void PlaceSellLimitOrder(double quantity, int tickType, double sellStopPrice, double riskPercent, bool outsideRth)
        {
            PlaceLimitOrder(OrderActions.SELL, quantity, tickType, sellStopPrice, riskPercent, outsideRth);
        }

        //Rick: 
        public void PlaceBuyStopLimitOrder(double quantity, int tickType, double buyStopPrice, double sellStopPrice, double riskPercent, bool outsideRth)
        {
            PlaceBuyStopLimitOrder(OrderActions.BUY, quantity, tickType, buyStopPrice, sellStopPrice, riskPercent, outsideRth);
        }

        //Rick:
        public void PlaceSellStopLimitOrder(double quantity, int tickType, double sellStopPrice, double buyStopPrice, double riskPercent, bool outsideRth)
        {
            PlaceSellStopLimitOrder(OrderActions.SELL, quantity, tickType, sellStopPrice, buyStopPrice, riskPercent, outsideRth);
        }

        public void PlaceLimitOrder(OrderActions action, double quantity, int tickType, double stopPrice, double riskPercent, bool outsideRth)
        {
            double? price = GetTick(tickType);
            
            if (!price.HasValue)
            {
                return;
            }

            //Rick: Have +/- 3 cent offset
            double offsetPrice = 0.0;

            if (action == OrderActions.BUY)
            {
                offsetPrice = price.Value + limitOffset;

                if (!(stopPrice < price))
                {
                    IO.ShowMessageTextBox(globalOutputTextBox, "LONG Stop: Sell Stop must be less than ASK + 3 cents");//GUI
                    //IO.ShowMessageCLI("LONG Stop: Sell Stop must be less than ASK + 3 cents");
                    return;
                }
            }
            else if (action == OrderActions.SELL)
            {
                offsetPrice = price.Value - limitOffset;

                if (!(stopPrice > price))
                {
                    IO.ShowMessageTextBox(globalOutputTextBox, "SHORT Stop: Buy Stop must be greater than BID - 3 cents");//GUI
                    //IO.ShowMessageCLI("SHORT Stop: Buy Stop must be greater than BID - 3 cents");
                    return;
                }
            }

            PlaceLimitOrder(action, quantity, price.Value, offsetPrice, stopPrice, riskPercent, outsideRth);
        }

        public void PlaceLimitOrder(OrderActions action, double quantity, double price, double offsetPrice, double stopPrice, double riskPercent, bool outsideRth)
        {
            if (stockContract == null || price <= 0)
            {
                return;
            }

            //Rick: Calculate number of shares to buy risking N% of account
            double currentPriceStopLossDiff = Math.Abs(price - stopPrice);
            double riskAmount = riskPercent * totalEquity / 100.00;
            double numShares = Math.Floor(riskAmount / currentPriceStopLossDiff);
            double dollarAmount = numShares * price;
            double percentageOfTotalEquity = dollarAmount / totalEquity * 100.00;

            //Rick: likely 5* Phase 2/3 clinical trial - no margin for Biotechs on my IBKR account so just use max account size in this case
            if (riskPercent >= 2.5 && percentageOfTotalEquity > 100.00)
            {
                numShares = Math.Floor(totalEquity / price);
            }

            var riskStr = String.Format("{0} Limit {1} - Price {2} - Stop {3} - Risk: {4}% (${5}) - {6} shares (Half: {7}) (${8:0.00}) ({9:0.00}% of ${10})",
                 stockContract.Symbol, action.ToString(), price, stopPrice, riskPercent, Math.Round(riskAmount), numShares, Math.Round(numShares / 2.0), dollarAmount, percentageOfTotalEquity, totalEquity);
            IO.ShowMessageTextBox(globalOutputTextBox, riskStr);//GUI
            //IO.ShowMessageCLI(riskStr);

            //parent order
            Order parentOrder = OrderFactory.CreateLimitOrder(action, numShares, offsetPrice, false, outsideRth);
            parentOrder.Account = TradedAccount;
            parentOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, !UseCFD ? stockContract: CFDContract, parentOrder);

            //child stop order
            OrderActions stopAction = action == OrderActions.BUY ? OrderActions.SELL : OrderActions.BUY;
            Order sellStopChildOrder = !outsideRth ? OrderFactory.CreateStopOrder(stopAction, numShares, stopPrice, true) :
                OrderFactory.CreateStopLimitOrder(stopAction, numShares, stopAction == OrderActions.SELL ? stopPrice - sellStopOffset : stopPrice + buyStopOffset, stopPrice, true, outsideRth);
            sellStopChildOrder.Account = TradedAccount;
            sellStopChildOrder.ParentId = parentOrder.OrderId;
            sellStopChildOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, !UseCFD ? stockContract : CFDContract, sellStopChildOrder);
        }

        public void PlaceCloseLimitOrder(OrderActions action, double quantity, int tickType, bool outsideRth)
        {
            double? price = GetTick(tickType);

            if (stockContract == null || !price.HasValue)
            {
                return;
            }

            //Rick: Have +/- 3 cent offset
            if (action == OrderActions.BUY)
            {
                price += limitOffset;               
            }
            else if (action == OrderActions.SELL)
            {
                price -= limitOffset;             
            }


            Order order = OrderFactory.CreateLimitOrder(action, quantity, price.Value, true, outsideRth);
            order.Account = TradedAccount;
            order.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, !UseCFD ? stockContract : CFDContract, order);
        }

        public void PlaceTakeProfitLimitOrder(OrderActions action, double quantity, double limitPrice, bool outsideRth)
        {
            if (stockContract == null)
            {
                return;
            } 


            Order order = OrderFactory.CreateLimitOrder(action, quantity, limitPrice, true, outsideRth);
            order.Account = TradedAccount;
            order.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, !UseCFD ? stockContract : CFDContract, order);
        }


        //Rick
        public void PlaceBuyStopLimitOrder(OrderActions action, double quantity, int tickType, double buyStopPrice, double sellStopPrice, double riskPercent, bool outsideRth)
        {
            if (stockContract == null || buyStopPrice <= 0)
        {
                return;
            }

            if (!(sellStopPrice < buyStopPrice))
            {
                IO.ShowMessageTextBox(globalOutputTextBox, "LONG Stop Limit: Sell Stop must be less than Buy Stop");
                //IO.ShowMessageCLI("LONG Stop Limit: Sell Stop must be less than Buy Stop");
                return;
            }

            //Rick: Calculate number of shares to buy risking N% of account
            double currentPriceStopLossDiff = Math.Abs(buyStopPrice - sellStopPrice);
            double riskAmount = riskPercent * totalEquity / 100.00;
            double numShares = Math.Floor(riskAmount / currentPriceStopLossDiff);
            double dollarAmount = numShares * buyStopPrice;
            double percentageOfTotalEquity = dollarAmount / totalEquity * 100.00;

            //Rick: likely 5* Phase 2/3 clinical trial - no margin for Biotechs on my IBKR account so just use max account size in this case
            if (riskPercent >= 2.5 && percentageOfTotalEquity > 100.00)
            {
                numShares = Math.Floor(totalEquity / (buyStopPrice + 0.05));
            }

            var riskStr = String.Format("{0} BUY Stop Limit - Price {1} - Stop {2} - Risk: {3}% (${4}) - {5} shares (Half: {6}) (${7:0.00}) ({8:0.00}% of ${9})",
                 stockContract.Symbol, buyStopPrice, sellStopPrice, riskPercent, Math.Round(riskAmount), numShares, Math.Round(numShares / 2.0), dollarAmount, percentageOfTotalEquity, totalEquity);
            IO.ShowMessageTextBox(globalOutputTextBox, riskStr);//GUI
            //IO.ShowMessageCLI(riskStr);

            //Rick: Buy Stop Limit is StopPrice + 11 cents
            double limitPrice = buyStopPrice + buyStopOffset;

            //Rick: Create parent order
            Order parentOrder = OrderFactory.CreateStopLimitOrder(action, numShares, limitPrice, buyStopPrice, false, outsideRth);
            parentOrder.Account = TradedAccount;
            parentOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, !UseCFD ? stockContract : CFDContract, parentOrder);

            //Rick: Create child stop order
            Order sellStopChildOrder = !outsideRth ? OrderFactory.CreateStopOrder(OrderActions.SELL, numShares, sellStopPrice, true) :
                OrderFactory.CreateStopLimitOrder(OrderActions.SELL, numShares, sellStopPrice - sellStopOffset, sellStopPrice, true, outsideRth);
            sellStopChildOrder.Account = TradedAccount;
            sellStopChildOrder.ParentId = parentOrder.OrderId;
            sellStopChildOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, !UseCFD ? stockContract : CFDContract, sellStopChildOrder);
        }

        //Rick
        public void PlaceSellStopLimitOrder(OrderActions action, double quantity, int tickType, double sellStopPrice, double buyStopPrice, double riskPercent, bool outsideRth)
        {
            if (stockContract == null || buyStopPrice <= 0)
            {
                return;
            }

            if (!(buyStopPrice > sellStopPrice)) 
            {
                IO.ShowMessageTextBox(globalOutputTextBox, "SHORT Stop Limit: Buy Stop must be greater than Sell Stop");
                //IO.ShowMessageCLI("SHORT Stop Limit: Buy Stop must be greater than Sell Stop");
                return;
            }

            //Rick: Calculate number of shares to buy risking N% of account
            double currentPriceStopLossDiff = Math.Abs(buyStopPrice - sellStopPrice);
            double riskAmount = riskPercent * totalEquity / 100.00;
            double numShares = Math.Floor(riskAmount / currentPriceStopLossDiff);
            double dollarAmount = numShares * buyStopPrice;
            double percentageOfTotalEquity = dollarAmount / totalEquity * 100.00;

            var riskStr = String.Format("{0} SELL Stop Limit - Price {1} - Stop {2} - Risk: {3}% (${4}) - {5} shares (Half: {6}) (${7:0.00}) ({8:0.00}% of ${9})",
                 stockContract.Symbol, sellStopPrice, buyStopPrice, riskPercent, Math.Round(riskAmount), numShares, Math.Round(numShares / 2.0), dollarAmount, percentageOfTotalEquity, totalEquity);
            IO.ShowMessageTextBox(globalOutputTextBox, riskStr);//GUI
            //IO.ShowMessageCLI(riskStr);

            //Rick: Sell Stop Limit is StopPrice - 11 cents
            double limitPrice = sellStopPrice - sellStopOffset;

            //Rick: Create parent order
            Order parentOrder = OrderFactory.CreateStopLimitOrder(action, numShares, limitPrice, sellStopPrice, false, outsideRth);
            parentOrder.Account = TradedAccount;
            parentOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, !UseCFD ? stockContract : CFDContract, parentOrder);

            //Rick: Create child stop order
            Order buyStopChildOrder = !outsideRth ?  OrderFactory.CreateStopOrder(OrderActions.BUY, numShares, buyStopPrice, true)
                : OrderFactory.CreateStopLimitOrder(OrderActions.BUY, numShares, buyStopPrice + buyStopOffset, buyStopPrice, true, outsideRth);
            buyStopChildOrder.Account = TradedAccount;
            buyStopChildOrder.ParentId = parentOrder.OrderId;
            buyStopChildOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, !UseCFD ? stockContract : CFDContract, buyStopChildOrder);
        }

        public void CancelLastOrder()
        {
           CancelOrder(--nextValidOrderId);
            ++canceledOrderCount;
        }

        public void CancelOrder(int orderId)
        {
            clientSocket.cancelOrder(orderId);
        }

        //Rick: jump to a fresh order id when required
        public int GetNextValidOrderId()
        {
            nextValidOrderId += canceledOrderCount;
            canceledOrderCount = 0;
            return nextValidOrderId;
        }

        public async Task<Position> RequestCurrentPositionAsync()
        {
            Portfolio positions = await RequestPortfolioAsync();
            return positions.Get(TickerSymbol);
        }

        public async Task<Position> RequestLargestPosition()
        {
            IEnumerable<Position> positions = await RequestPositionsAsync();
            return positions
                .OrderByDescending(p => p.PositionSize)
                .FirstOrDefault();
        }

        public async Task<IEnumerable<Position>> RequestPositionsAsync()
        {
            Portfolio positions = await RequestPortfolioAsync();
            return positions.Values;
        }

        public async Task<Portfolio> RequestPortfolioAsync()
        {
            await accountDownloadEndTCS.Task;
            return portfolio;
        }

        public void RequestStockContractDetails(string tickerSymbol)
        {
            clientSocket.reqContractDetails(NumberGenerator.NextRandomInt(), ContractFactory.CreateStockContract(tickerSymbol));
        }

        public void RequestCFDContractDetails(string tickerSymbol)
        {
            clientSocket.reqContractDetails(NumberGenerator.NextRandomInt(), ContractFactory.CreateCFDContract(tickerSymbol));
        }

        //Rick TODO: Request Live Orders

        public bool HasTicks(params int[] tickTypes)
        {
            if (tickData == null)
            {
                return false;
            }

            var withPositiveValue = new Func<int, double, bool>((key, value)
                => value >= 0);
            return tickData.HasTicks(withPositiveValue, tickTypes);
        }

        public Task<bool> HasTicksAsync(params int[] tickTypes)
        {
            // If we already have the tick data, then there is no need 
            // to wait for the next round of tick updates.
            if (HasTicks(tickTypes))
            {
                return Task.FromResult(true);
            }

            // Otherwise, proceed with fancy asynchronous code!
            var tcs = new TaskCompletionSource<bool>();

            var onTickUpdated = new TickUpdatedEventHandler((tickType, value) =>
            {
                if (HasTicks(tickTypes))
                {
                    tcs.TrySetResult(true);
                }
            });

            var onPropertyChanged = new PropertyChangedEventHandler((eventArgs) =>
            {
                switch (eventArgs.PropertyName)
                {
                    case nameof(tickData):
                        // If the TickData collection was re-assigned, then abort.
                        tcs.TrySetResult(false);
                        break;
                }
            });

            var onError = new Action<int, int, string, Exception>((id, code, msg, ex) =>
            {
                tcs.TrySetResult(false);
            });

            tcs.Task.ContinueWith(t =>
            {
                TickUpdated -= onTickUpdated;
                PropertyChanged -= onPropertyChanged;
                Error -= onError;
            });

            TickUpdated += onTickUpdated;
            PropertyChanged += onPropertyChanged;
            Error += onError;

            return tcs.Task;
        }

        //Rick: returns bid/ask depending on tickType
        public double? GetTick(int tickType)
        {
            return tickData?.Get(tickType);
        }
        #endregion

        #region PropertyChanged callbacks
        private void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(TradedAccount):
                    OnTradedAccountChanged(eventArgs);
                    break;
                    //case nameof(TickerSymbol): //Rick: No longer used. Contract is obtained from IBKR via OnContractDetails to remove ambiguity errors
                    //    OnTickerSymbolChanged(eventArgs);
                    //    break;
            }
        }

        private void OnTradedAccountChanged(PropertyChangedEventArgs eventArgs)
        {
            var args = eventArgs as PropertyValueChangedEventArgs<string>;
            var oldValue = args.OldValue;
            var newValue = args.NewValue;

            if (!string.IsNullOrWhiteSpace(oldValue))
            {
                clientSocket.reqAccountUpdates(false, oldValue);
            }

            portfolio = new Portfolio();
            accountDownloadEndTCS = new TaskCompletionSource<string>();

            if (!string.IsNullOrWhiteSpace(newValue))
            {
                clientSocket.reqAccountUpdates(true, newValue);
            }
        }

        //Rick: No longer used. Contract is obtained from IBKR via OnContractDetails to remove ambiguity errors
        //private void OnTickerSymbolChanged(PropertyChangedEventArgs eventArgs)
        //{
        //    var args = eventArgs as PropertyValueChangedEventArgs<string>;
        //    var oldValue = args.OldValue;
        //    var newValue = args.NewValue;

        //    if (!string.IsNullOrWhiteSpace(oldValue))
        //    {
        //        clientSocket.cancelMktData(tickerId);
        //    }

        //    tickData = new TickData();
        //    if (!string.IsNullOrWhiteSpace(newValue))
        //    {
        //        tickerId = NumberGenerator.NextRandomInt();
        //        stockContract = ContractFactory.CreateStockContract(newValue);
        //        clientSocket.reqMktData(tickerId, stockContract, "", false, false, null);
        //    }
        //    else
        //    {
        //        tickerId = -1;
        //        stockContract = null;
        //    }
        //}
        #endregion

        #region TWS callbacks
        private void OnError(int id, int errorCode, string errorMessage, Exception exception)
        {
            switch (errorCode)
            {
                case ErrorCodes.TICKER_NOT_FOUND:
                    TickerSymbol = null;
                    break;
            }
        }

        private void OnConnectAck()
        {
            // When this event is triggered within an asynchronous context, 
            // the client will have to start the flow of information to TWS.
            if (clientSocket.AsyncEConnect)
            {
                clientSocket.startApi();
            }

            IsConnected = true;
        }

        private void OnConnectionClosed()
        {
            IsConnected = false;
        }

        private void OnManagedAccounts(string accounts)
        {
            Accounts = accounts
                .Split(new string[] { "," }, StringSplitOptions.None)
                .Select(s => s.Trim())
                .ToArray();

            //Rick: Print accounts found
            IO.ShowMessageTextBox(globalOutputTextBox, "Accounts found:");//GUI
            //IO.ShowMessageCLI("Accounts found:");
            foreach (var acct in Accounts)
            {
                IO.ShowMessageTextBox(globalOutputTextBox, acct);//GUI
                //IO.ShowMessageCLI(acct);
            }

            //Rick: Get Available Funds for all accounts - make the account with the largest amount our traded account - use Available Funds value as our totalEquity to calculate risk % per trade
            clientSocket.reqAccountSummary(NumberGenerator.NextRandomInt(), "All", AccountSummaryTags.AvailableFunds);
        }

        private void OnNextValidId(int orderId)
        {
            nextValidOrderId = orderId;
        }

        private void OnTickPrice(int tickId, int field, double price, TickAttrib canAutoExecute)
        {
            UpdateTickData(tickId, field, price);
        }

        private void OnTickSize(int tickId, int field, long size)
        {
            UpdateTickData(tickId, field, size);
        }

        private void OnTickGeneric(int tickId, int field, double value)
        {
            UpdateTickData(tickId, field, value);
        }

        private void UpdateTickData(int tickId, int tickType, double value)
        {
            if (tickId != tickerId)
            {
                return;
            }

            tickData.Update(tickType, value);
            TickUpdated?.Invoke(tickType, value);
        }

        private void OnUpdatePortfolio(Contract contract, double positionSize, double marketPrice, double marketValue, double avgCost, double unrealisedPNL, double realisedPNL, string account)
        {
            var position = new Position(account, contract, positionSize, avgCost, marketPrice, marketValue, unrealisedPNL, realisedPNL);
            portfolio.Update(position);
            PositionUpdated?.Invoke(position);
        }

        private void OnAccountDownloadEnd(string account)
        {
            accountDownloadEndTCS.TrySetResult(account);
        }

        private void OnCommissionReport(CommissionReport report)
        {
            CommissionReports.Add(report);
            PropertyChanged.RaiseEvent(CommissionReports, nameof(CommissionReports));
        }

        //Rick: Use Account summary to get AvailableFunds from each account and choose account with highest amount
        private void OnAccountSummary(int reqId, string account, string tag, string value, string currency)
        {
            IO.ShowMessageTextBox(globalOutputTextBox, "Acct Summary. ReqId: " + reqId + ", Acct: " + account + ", Tag: " + tag + ", Value: " + value + ", Currency: " + currency);//GUI
            //IO.ShowMessageCLI("Acct Summary. ReqId: " + reqId + ", Acct: " + account + ", Tag: " + tag + ", Value: " + value + ", Currency: " + currency);

            accountAvailableFunds[account] = double.Parse(value);
        }

        public void OnAccountSummaryEnd(int reqId)
        {
            //IO.ShowMessageCLI("AccountSummaryEnd. Req Id: " + reqId + "\n");

            //Rick: cancel the account summary request otherwise it updates every 3 minutes
            clientSocket.cancelAccountSummary(reqId);

            //Find account with maximum Available Funds
            var largestAcc = accountAvailableFunds.Aggregate((l, r) => l.Value > r.Value ? l : r);
            totalEquity = largestAcc.Value; //Make Total Equity Available Funds
            MaxAvailableFundsAccount = largestAcc.Key;            
        }

        public void OnContractDetails(int reqId, ContractDetails contractDetails)
        {
            //IO.ShowMessage("OnContractDetails. Req Id: " + reqId);
            
            string stock1str = contractDetails.Contract.Symbol + " (" + contractDetails.LongName + ")";
            IO.ShowMessageTextBox(globalOutputTextBox, "OnContractDetails: " + contractDetails.Contract.SecType + " Contract Details retrieved for: " + stock1str);//GUI
            //IO.ShowMessageCLI("OnContractDetails: " + contractDetails.Contract.SecType + " Contract Details retrieved for: " + stock1str);
            
            if (!UseCFD)
                stock1str += " (Stock)";
            else
                stock1str += " (CFD)";

            if (stock1GroupBoxx.InvokeRequired) 
            {
                stock1GroupBoxx.BeginInvoke(() => { stock1GroupBoxx.Text = stock1str; });
                
            }
            else
            {
                stock1GroupBoxx.Text = stock1GroupBoxx.Text = stock1str; 
            }

            //Rick: USD Stock returned is always the first one. TWS Doc says:
            //"Invoking reqContractDetails with a Contract object which has currency = USD will only return US contracts, even if there are non-US instruments which have the USD currency."

            //Rick: Market data unavailable for CFD contracts so we use the Stock contract for market data. We're only using the CFD contracts for placing orders
            var contractSecType = contractDetails.Contract.SecType;
            if (contractSecType == "STK")
            {
                stockContract = contractDetails.Contract;
            }
            else if (UseCFD && contractSecType == "CFD")
            {
                CFDContract = contractDetails.Contract;
                return;
            }

            if (tickerId != 0)
            {
                clientSocket.cancelMktData(tickerId);
            }

            tickData = new TickData();

            tickerId = NumberGenerator.NextRandomInt();
            clientSocket.reqMktData(tickerId, stockContract, "", false, false, null);

        }

        public void OnContractDetailsEnd(int reqId)
        {
            //IO.ShowMessage("OnContractDetailsEnd. Req Id: " + reqId);
        }

        #endregion
    }
}
