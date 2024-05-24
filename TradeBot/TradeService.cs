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

        //Stock 1
        private int stock1StockReqContractDetailsId;
        private int stock1CFDReqContractDetailsId;
        private int stock1ReqMktDataId;
        private Contract stock1Contract;
        private Contract stock1CFDContract;
        private TickData stock1TickData;

        //Stock 2
        private int stock2StockReqContractDetailsId;
        private int stock2CFDReqContractDetailsId;
        private int stock2ReqMktDataId;
        private Contract stock2Contract;
        private Contract stock2CFDContract;
        private TickData stock2TickData;

        //Stock 3
        private int stock3StockReqContractDetailsId;
        private int stock3CFDReqContractDetailsId;
        private int stock3ReqMktDataId;
        private Contract stock3Contract;
        private Contract stock3CFDContract;
        private TickData stock3TickData;

        //Stock 4
        private int stock4StockReqContractDetailsId;
        private int stock4CFDReqContractDetailsId;
        private int stock4ReqMktDataId;
        private Contract stock4Contract;
        private Contract stock4CFDContract;
        private TickData stock4TickData;


        private int nextValidOrderId;

        //Offsets in cents for order types
        private double limitOffset = 0.05;
        private double buyStopOffset = 0.12;
        private double sellStopOffset = 0.12;

        private double limitOffsetLessThanOneDollar = 0.02;
        private double buyStopOffsetLessThanOneDollar = 0.03;
        private double sellStopOffsetLessThanOneDollar = 0.03;

        private double limitOffsetGreaterThanFourHundredDollars = 0.30;
        private double buyStopOffsetGreaterThanFourHundredDollars = 0.30;
        private double sellStopOffsetGreaterThanFourHundredDollars = 0.30;

        public double GetLimitOrderOffset(double priceValue) =>
            priceValue switch
            {
                >= 1.00 and <= 400.00 => limitOffset,
                > 400.00 => limitOffsetGreaterThanFourHundredDollars,                
                < 1.00 => limitOffsetLessThanOneDollar,
                _ => double.NaN
            };

        public double GetBuyStopLimitOrderOffset(double priceValue) =>
            priceValue switch
            {
                >= 1.00 and <= 400.00 => buyStopOffset,
                > 400.00 => buyStopOffsetGreaterThanFourHundredDollars,
                < 1.00 => buyStopOffsetLessThanOneDollar,
                _ => double.NaN
            };
        public double GetSellStopLimitOrderOffset(double priceValue) =>
            priceValue switch
            {
                >= 1.00 and <= 400.00 => sellStopOffset,
                > 400.00 => sellStopOffsetGreaterThanFourHundredDollars,
                < 1.00 => sellStopOffsetLessThanOneDollar,
                _ => double.NaN
            };

        //Maps an Account ID to Net Liquidation Value of account
        Dictionary<string, double> accountNetLiquidationValue = new Dictionary<string, double>();

        //Rick - number of orders cancelled, so we can jump to a fresh order id when required
        private int canceledOrderCount = 0;

        private TextBox globalOutputTextBox;
        private GroupBox stock1GroupBoxx;
        private GroupBox stock2GroupBoxx;
        private GroupBox stock3GroupBoxx;
        private GroupBox stock4GroupBoxx;
        public TradeService(int clientId, TextBox textBox, GroupBox stock1GroupBox, GroupBox stock2GroupBox, GroupBox stock3GroupBox, GroupBox stock4GroupBox)
        {
            ClientId = clientId;

            readerSignal = new EReaderMonitorSignal();
            clientSocket = new EClientSocket(this, readerSignal);

            globalOutputTextBox = textBox;
            stock1GroupBoxx = stock1GroupBox;
            stock2GroupBoxx = stock2GroupBox;
            stock3GroupBoxx = stock3GroupBox;
            stock4GroupBoxx = stock4GroupBox;

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

        private string _stock1TickerSymbol;
        public string Stock1TickerSymbol
        {
            get
            {
                return _stock1TickerSymbol;
            }
            set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _stock1TickerSymbol, value?.Trim().ToUpper());
            }
        }

        private string _stock2TickerSymbol;
        public string Stock2TickerSymbol
        {
            get
            {
                return _stock2TickerSymbol;
            }
            set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _stock2TickerSymbol, value?.Trim().ToUpper());
            }
        }

        private string _stock3TickerSymbol;
        public string Stock3TickerSymbol
        {
            get
            {
                return _stock3TickerSymbol;
            }
            set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _stock3TickerSymbol, value?.Trim().ToUpper());
            }
        }


        private string _stock4TickerSymbol;
        public string Stock4TickerSymbol
        {
            get
            {
                return _stock4TickerSymbol;
            }
            set
            {
                PropertyChanged.SetPropertyAndRaiseEvent(ref _stock4TickerSymbol, value?.Trim().ToUpper());
            }
        }

        //Rick
        private bool _stock1UseCFD = false;
        public bool Stock1UseCFD
        {
            get
            {
                return _stock1UseCFD;
            }
            set
            {
                _stock1UseCFD = value;
            }
        }

        private bool _stock2UseCFD = false;
        public bool Stock2UseCFD
        {
            get
            {
                return _stock2UseCFD;
            }
            set
            {
                _stock2UseCFD = value;
            }
        }

        private bool _stock3UseCFD = false;
        public bool Stock3UseCFD
        {
            get
            {
                return _stock3UseCFD;
            }
            set
            {
                _stock3UseCFD = value;
            }
        }

        private bool _stock4UseCFD = false;
        public bool Stock4UseCFD
        {
            get
            {
                return _stock4UseCFD;
            }
            set
            {
                _stock4UseCFD = value;
            }
        }

        public bool HasStock1TickerSymbol
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Stock1TickerSymbol);
            }
        }

        public bool HasStock2TickerSymbol
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Stock2TickerSymbol);
            }
        }

        public bool HasStock3TickerSymbol
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Stock3TickerSymbol);
            }
        }

        public bool HasStock4TickerSymbol
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Stock4TickerSymbol);
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

        public void PlaceBuyLimitOrder(int stockNum, double quantity, int tickType, double sellStopPrice, double riskPercent, bool outsideRth)
        {
            PlaceLimitOrder(stockNum, OrderActions.BUY, quantity, tickType, sellStopPrice, riskPercent, outsideRth);
        }
        
        public void PlaceSellLimitOrder(int stockNum, double quantity, int tickType, double sellStopPrice, double riskPercent, bool outsideRth)
        {
            PlaceLimitOrder(stockNum, OrderActions.SELL, quantity, tickType, sellStopPrice, riskPercent, outsideRth);
        }

        //Rick: 
        public void PlaceBuyStopLimitOrder(int stockNum, double quantity, int tickType, double buyStopPrice, double sellStopPrice, double riskPercent, bool outsideRth)
        {
            PlaceBuyStopLimitOrder(stockNum, OrderActions.BUY, quantity, tickType, buyStopPrice, sellStopPrice, riskPercent, outsideRth);
        }

        //Rick:
        public void PlaceSellStopLimitOrder(int stockNum, double quantity, int tickType, double sellStopPrice, double buyStopPrice, double riskPercent, bool outsideRth)
        {
            PlaceSellStopLimitOrder(stockNum, OrderActions.SELL, quantity, tickType, sellStopPrice, buyStopPrice, riskPercent, outsideRth);
        }

        public void PlaceLimitOrder(int stockNum, OrderActions action, double quantity, int tickType, double stopPrice, double riskPercent, bool outsideRth)
        {
            double? price = GetTick(stockNum, tickType);
            
            if (!price.HasValue)
            {
                return;
            }

            //Rick: +/- N cent offset
            double offsetPrice = 0.0;
            double orderLimitOffset = GetLimitOrderOffset(price.Value);

            if (action == OrderActions.BUY)
            {
                offsetPrice = price.Value + orderLimitOffset;

                if (!(stopPrice < price))
                {
                    IO.ShowMessageTextBox(globalOutputTextBox, "LONG Stop: Sell Stop must be less than ASK + " + orderLimitOffset);//GUI
                    //IO.ShowMessageCLI("LONG Stop: Sell Stop must be less than ASK + 3 cents");
                    return;
                }
            }
            else if (action == OrderActions.SELL)
            {
                offsetPrice = price.Value - orderLimitOffset;

                if (!(stopPrice > price))
                {
                    IO.ShowMessageTextBox(globalOutputTextBox, "SHORT Stop: Buy Stop must be greater than BID - " + orderLimitOffset);//GUI
                    //IO.ShowMessageCLI("SHORT Stop: Buy Stop must be greater than BID - 3 cents");
                    return;
                }
            }

            PlaceLimitOrder(stockNum, action, quantity, price.Value, offsetPrice, stopPrice, riskPercent, outsideRth);
        }

        public void PlaceLimitOrder(int stockNum, OrderActions action, double quantity, double price, double offsetPrice, double stopPrice, double riskPercent, bool outsideRth)
        {
            Contract contract = GetContract(stockNum);
            if (contract == null || price <= 0)
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
                 contract.Symbol, action.ToString(), price, stopPrice, riskPercent, Math.Round(riskAmount), numShares, Math.Round(numShares / 2.0), dollarAmount, percentageOfTotalEquity, totalEquity);
            IO.ShowMessageTextBox(globalOutputTextBox, riskStr);//GUI
            //IO.ShowMessageCLI(riskStr);

            //parent order
            Order parentOrder = OrderFactory.CreateLimitOrder(action, numShares, offsetPrice, false, outsideRth);
            parentOrder.Account = TradedAccount;
            parentOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, contract, parentOrder);

            //child stop order
            OrderActions stopAction = action == OrderActions.BUY ? OrderActions.SELL : OrderActions.BUY;
            Order sellStopChildOrder = !outsideRth ? OrderFactory.CreateStopOrder(stopAction, numShares, stopPrice, true) :
                OrderFactory.CreateStopLimitOrder(stopAction, numShares, stopAction == OrderActions.SELL ? stopPrice - GetSellStopLimitOrderOffset(price) : stopPrice + GetBuyStopLimitOrderOffset(price), stopPrice, true, outsideRth);
            sellStopChildOrder.Account = TradedAccount;
            sellStopChildOrder.ParentId = parentOrder.OrderId;
            sellStopChildOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, contract, sellStopChildOrder);
        }

        public void PlaceCloseLimitOrder(int stockNum, OrderActions action, double quantity, int tickType, bool outsideRth)
        {
            double? price = GetTick(stockNum, tickType);
            if (!price.HasValue)
            {
                return;
            }

            //Rick: +/- N cent offset
            if (action == OrderActions.BUY)
            {
                price += GetLimitOrderOffset(price.Value);               
            }
            else if (action == OrderActions.SELL)
            {
                price -= GetLimitOrderOffset(price.Value);
            }

            Order order = OrderFactory.CreateLimitOrder(action, quantity, price.Value, true, outsideRth);
            order.Account = TradedAccount;
            order.OrderId = GetNextValidOrderId();
            
            Contract contract = GetContract(stockNum);
            if (contract == null)
            {
                return;
            }

            clientSocket.placeOrder(nextValidOrderId++, contract, order);
        }

        public void PlaceTakeProfitLimitOrder(int stockNum, OrderActions action, double quantity, double limitPrice, bool outsideRth)
        {
            Contract contract = GetContract(stockNum);
            if (contract == null)
            {
                return;
            } 

            Order order = OrderFactory.CreateLimitOrder(action, quantity, limitPrice, true, outsideRth);
            order.Account = TradedAccount;
            order.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, contract, order);
        }


        //Rick
        public void PlaceBuyStopLimitOrder(int stockNum, OrderActions action, double quantity, int tickType, double buyStopPrice, double sellStopPrice, double riskPercent, bool outsideRth)
        {
            Contract contract = GetContract(stockNum);
            if (contract == null || buyStopPrice <= 0)
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
                 contract.Symbol, buyStopPrice, sellStopPrice, riskPercent, Math.Round(riskAmount), numShares, Math.Round(numShares / 2.0), dollarAmount, percentageOfTotalEquity, totalEquity);
            IO.ShowMessageTextBox(globalOutputTextBox, riskStr);//GUI
            //IO.ShowMessageCLI(riskStr);

            //Rick: Buy Stop Limit is StopPrice + N cents
            double limitPrice = buyStopPrice + GetBuyStopLimitOrderOffset(buyStopPrice);

            //Rick: Create parent order
            Order parentOrder = OrderFactory.CreateStopLimitOrder(action, numShares, limitPrice, buyStopPrice, false, outsideRth);
            parentOrder.Account = TradedAccount;
            parentOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, contract, parentOrder);

            //Rick: Create child stop order
            Order sellStopChildOrder = !outsideRth ? OrderFactory.CreateStopOrder(OrderActions.SELL, numShares, sellStopPrice, true) :
                OrderFactory.CreateStopLimitOrder(OrderActions.SELL, numShares, sellStopPrice - GetSellStopLimitOrderOffset(buyStopPrice), sellStopPrice, true, outsideRth);
            sellStopChildOrder.Account = TradedAccount;
            sellStopChildOrder.ParentId = parentOrder.OrderId;
            sellStopChildOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, contract, sellStopChildOrder);
        }

        //Rick
        public void PlaceSellStopLimitOrder(int stockNum, OrderActions action, double quantity, int tickType, double sellStopPrice, double buyStopPrice, double riskPercent, bool outsideRth)
        {
            Contract contract = GetContract(stockNum);
            if (contract == null || buyStopPrice <= 0)
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
                 contract.Symbol, sellStopPrice, buyStopPrice, riskPercent, Math.Round(riskAmount), numShares, Math.Round(numShares / 2.0), dollarAmount, percentageOfTotalEquity, totalEquity);
            IO.ShowMessageTextBox(globalOutputTextBox, riskStr);//GUI
            //IO.ShowMessageCLI(riskStr);

            //Rick: Sell Stop Limit is StopPrice - N cents
            double limitPrice = sellStopPrice - GetSellStopLimitOrderOffset(sellStopPrice);

            //Rick: Create parent order
            Order parentOrder = OrderFactory.CreateStopLimitOrder(action, numShares, limitPrice, sellStopPrice, false, outsideRth);
            parentOrder.Account = TradedAccount;
            parentOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, contract, parentOrder);

            //Rick: Create child stop order
            Order buyStopChildOrder = !outsideRth ?  OrderFactory.CreateStopOrder(OrderActions.BUY, numShares, buyStopPrice, true)
                : OrderFactory.CreateStopLimitOrder(OrderActions.BUY, numShares, buyStopPrice + GetBuyStopLimitOrderOffset(sellStopPrice), buyStopPrice, true, outsideRth);
            buyStopChildOrder.Account = TradedAccount;
            buyStopChildOrder.ParentId = parentOrder.OrderId;
            buyStopChildOrder.OrderId = GetNextValidOrderId();
            clientSocket.placeOrder(nextValidOrderId++, contract, buyStopChildOrder);
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

        public async Task<Position> RequestCurrentPositionAsync(string tickerSymbol)
        {
            Portfolio positions = await RequestPortfolioAsync();
            return positions.Get(tickerSymbol);
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

        public void RequestStockContractDetails(string tickerSymbol, int stockNum)
        {
            int reqContractDetailsId = NumberGenerator.NextRandomInt();
            if (stockNum == 1)
                stock1StockReqContractDetailsId = reqContractDetailsId;
            else if (stockNum == 2)
                stock2StockReqContractDetailsId = reqContractDetailsId;
            else if (stockNum == 3)
                stock3StockReqContractDetailsId = reqContractDetailsId;
            else if (stockNum == 4)
                stock4StockReqContractDetailsId = reqContractDetailsId;

            clientSocket.reqContractDetails(reqContractDetailsId, ContractFactory.CreateStockContract(tickerSymbol));
        }

        public void RequestCFDContractDetails(string tickerSymbol, int stockNum)
        {
            int reqContractDetailsId = NumberGenerator.NextRandomInt();
            if (stockNum == 1)
                stock1CFDReqContractDetailsId = reqContractDetailsId;
            else if (stockNum == 2)
                stock2CFDReqContractDetailsId = reqContractDetailsId;
            else if (stockNum == 3)
                stock3CFDReqContractDetailsId = reqContractDetailsId;
            else if (stockNum == 4)
                stock4CFDReqContractDetailsId = reqContractDetailsId;

            clientSocket.reqContractDetails(reqContractDetailsId, ContractFactory.CreateCFDContract(tickerSymbol));
        }

        public bool HasTicks(params int[] tickTypes)
        {
            if (stock1TickData == null)
            {
                return false;
            }

            var withPositiveValue = new Func<int, double, bool>((key, value)
                => value >= 0);
            return stock1TickData.HasTicks(withPositiveValue, tickTypes);
        }

        public bool HasTicks(int stockNum, params int[] tickTypes)
        {
            var withPositiveValue = new Func<int, double, bool>((key, value)
                => value >= 0);

            if (stockNum == 1)
            {
                if (stock1TickData != null)
                {
                    return stock1TickData.HasTicks(withPositiveValue, tickTypes);
                }                
            }
            else if (stockNum == 2)
            {
                if (stock2TickData != null)
                {
                    return stock2TickData.HasTicks(withPositiveValue, tickTypes);
                }               
            }
            else if (stockNum == 3)
            {
                if (stock3TickData != null)
                {
                    return stock3TickData.HasTicks(withPositiveValue, tickTypes);
                }
            }
            else if (stockNum == 4)
            {
                if (stock4TickData != null)
                {
                    return stock4TickData.HasTicks(withPositiveValue, tickTypes);
                }
            }

            return false;
        }

        public Task<bool> Stock1HasTicksAsync(params int[] tickTypes)
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
                    case nameof(stock1TickData):
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
        public double? GetTick(int stockNum, int tickType)
        {
            if (stockNum == 1)
            {
                return stock1TickData?.Get(tickType);
            }
            else if (stockNum == 2) 
            {
                return stock2TickData?.Get(tickType);
            }
            else if (stockNum == 3)
            {
                return stock3TickData?.Get(tickType);
            }
            else if (stockNum == 4)
            {
                return stock4TickData?.Get(tickType);
            }
            return null;
        }

        public double? Stock1GetTick(int tickType)
        {
            return stock1TickData?.Get(tickType);
        }

        public Contract GetContract(int stockNum) 
        {
            if (stockNum == 1) {
                return !Stock1UseCFD ? stock1Contract : stock1CFDContract;
            }
            else if (stockNum == 2)
            {
                return !Stock2UseCFD ? stock2Contract : stock2CFDContract;
            }
            else if (stockNum == 3)
            {
                return !Stock3UseCFD ? stock3Contract : stock3CFDContract;
            }
            else if (stockNum == 4)
            {
                return !Stock4UseCFD ? stock4Contract : stock4CFDContract;
            }

            return null;
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
                    //Stock1TickerSymbol = null;
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

            //Rick: Get Net Liquidation Value for all accounts - make the account with the largest NLV our traded account - use NLV as our totalEquity to calculate risk % per trade
            clientSocket.reqAccountSummary(NumberGenerator.NextRandomInt(), "All", AccountSummaryTags.NetLiquidation);

            //Rick: Get Available Funds for all accounts - make the account with the largest amount our traded account - use Available Funds value as our totalEquity to calculate risk % per trade
            //clientSocket.reqAccountSummary(NumberGenerator.NextRandomInt(), "All", AccountSummaryTags.AvailableFunds);
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
            if (tickId == stock1ReqMktDataId)
            {
                stock1TickData.Update(tickType, value);
                TickUpdated?.Invoke(tickType, value); //Rick TODO: Don't think this is necessary 
                return;
            }
            else if (tickId == stock2ReqMktDataId)
            {
                stock2TickData.Update(tickType, value);
                TickUpdated?.Invoke(tickType, value);
                return;
            }
            else if (tickId == stock3ReqMktDataId)
            {
                stock3TickData.Update(tickType, value);
                TickUpdated?.Invoke(tickType, value);
                return;
            }
            else if (tickId == stock4ReqMktDataId)
            {
                stock4TickData.Update(tickType, value);
                TickUpdated?.Invoke(tickType, value);
                return;
            }
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

            accountNetLiquidationValue[account] = double.Parse(value);
        }

        public void OnAccountSummaryEnd(int reqId)
        {
            //IO.ShowMessageCLI("AccountSummaryEnd. Req Id: " + reqId + "\n");

            //Rick: cancel the account summary request otherwise it updates every 3 minutes
            clientSocket.cancelAccountSummary(reqId);

            //Find account with highest Net Liquidation Value and use that as our Total Equity
            var largestAcc = accountNetLiquidationValue.Aggregate((l, r) => l.Value > r.Value ? l : r);
            totalEquity = largestAcc.Value;
            MaxAvailableFundsAccount = largestAcc.Key;            
        }

        public void OnContractDetails(int reqId, ContractDetails contractDetails)
        {
            //IO.ShowMessage("OnContractDetails. Req Id: " + reqId);

            //Stock 1
            if (reqId == stock1StockReqContractDetailsId || reqId == stock1CFDReqContractDetailsId)
            {
                string stock1str = contractDetails.Contract.Symbol + " (" + contractDetails.LongName + ")";
                IO.ShowMessageTextBox(globalOutputTextBox, "Stock 1 - OnContractDetails: " + contractDetails.Contract.SecType + " Contract Details retrieved for: " + stock1str);//GUI

                if (!Stock1UseCFD)
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
                    stock1Contract = contractDetails.Contract;
                }
                else if (Stock1UseCFD && contractSecType == "CFD")
                {
                    stock1CFDContract = contractDetails.Contract;
                    return;
                }

                if (stock1ReqMktDataId != 0)
                {
                    clientSocket.cancelMktData(stock1ReqMktDataId);
                }

                stock1TickData = new TickData();

                stock1ReqMktDataId = NumberGenerator.NextRandomInt();
                clientSocket.reqMktData(stock1ReqMktDataId, stock1Contract, "", false, false, null);
            }
            //Stock 2
            else if (reqId == stock2StockReqContractDetailsId || reqId == stock2CFDReqContractDetailsId)
            {
                string stock2str = contractDetails.Contract.Symbol + " (" + contractDetails.LongName + ")";
                IO.ShowMessageTextBox(globalOutputTextBox, "Stock 2 - OnContractDetails: " + contractDetails.Contract.SecType + " Contract Details retrieved for: " + stock2str);//GUI

                if (!Stock2UseCFD)
                    stock2str += " (Stock)";
                else
                    stock2str += " (CFD)";

                if (stock2GroupBoxx.InvokeRequired)
                {
                    stock2GroupBoxx.BeginInvoke(() => { stock2GroupBoxx.Text = stock2str; });

                }
                else
                {
                    stock2GroupBoxx.Text = stock2GroupBoxx.Text = stock2str;
                }

                //Rick: USD Stock returned is always the first one. TWS Doc says:
                //"Invoking reqContractDetails with a Contract object which has currency = USD will only return US contracts, even if there are non-US instruments which have the USD currency."

                //Rick: Market data unavailable for CFD contracts so we use the Stock contract for market data. We're only using the CFD contracts for placing orders
                var contractSecType = contractDetails.Contract.SecType;
                if (contractSecType == "STK")
                {
                    stock2Contract = contractDetails.Contract;
                }
                else if (Stock2UseCFD && contractSecType == "CFD")
                {
                    stock2CFDContract = contractDetails.Contract;
                    return;
                }

                if (stock2ReqMktDataId != 0)
                {
                    clientSocket.cancelMktData(stock2ReqMktDataId);
                }

                stock2TickData = new TickData();

                stock2ReqMktDataId = NumberGenerator.NextRandomInt();
                clientSocket.reqMktData(stock2ReqMktDataId, stock2Contract, "", false, false, null);
            }
            //Stock3
            else if (reqId == stock3StockReqContractDetailsId || reqId == stock3CFDReqContractDetailsId)
            {
                string stock3str = contractDetails.Contract.Symbol + " (" + contractDetails.LongName + ")";
                IO.ShowMessageTextBox(globalOutputTextBox, "Stock 3 - OnContractDetails: " + contractDetails.Contract.SecType + " Contract Details retrieved for: " + stock3str);//GUI

                if (!Stock3UseCFD)
                    stock3str += " (Stock)";
                else
                    stock3str += " (CFD)";

                if (stock3GroupBoxx.InvokeRequired)
                {
                    stock3GroupBoxx.BeginInvoke(() => { stock3GroupBoxx.Text = stock3str; });

                }
                else
                {
                    stock3GroupBoxx.Text = stock3GroupBoxx.Text = stock3str;
                }

                var contractSecType = contractDetails.Contract.SecType;
                if (contractSecType == "STK")
                {
                    stock3Contract = contractDetails.Contract;
                }
                else if (Stock3UseCFD && contractSecType == "CFD")
                {
                    stock3CFDContract = contractDetails.Contract;
                    return;
                }

                if (stock3ReqMktDataId != 0)
                {
                    clientSocket.cancelMktData(stock3ReqMktDataId);
                }

                stock3TickData = new TickData();

                stock3ReqMktDataId = NumberGenerator.NextRandomInt();
                clientSocket.reqMktData(stock3ReqMktDataId, stock3Contract, "", false, false, null);
            }
            //Stock 4
            else if (reqId == stock4StockReqContractDetailsId || reqId == stock4CFDReqContractDetailsId)
            {
                string stock4str = contractDetails.Contract.Symbol + " (" + contractDetails.LongName + ")";
                IO.ShowMessageTextBox(globalOutputTextBox, "Stock 4 - OnContractDetails: " + contractDetails.Contract.SecType + " Contract Details retrieved for: " + stock4str);//GUI

                if (!Stock4UseCFD)
                    stock4str += " (Stock)";
                else
                    stock4str += " (CFD)";

                if (stock4GroupBoxx.InvokeRequired)
                {
                    stock4GroupBoxx.BeginInvoke(() => { stock4GroupBoxx.Text = stock4str; });

                }
                else
                {
                    stock4GroupBoxx.Text = stock4GroupBoxx.Text = stock4str;
                }

                var contractSecType = contractDetails.Contract.SecType;
                if (contractSecType == "STK")
                {
                    stock4Contract = contractDetails.Contract;
                }
                else if (Stock4UseCFD && contractSecType == "CFD")
                {
                    stock4CFDContract = contractDetails.Contract;
                    return;
                }

                if (stock4ReqMktDataId != 0)
                {
                    clientSocket.cancelMktData(stock4ReqMktDataId);
                }

                stock4TickData = new TickData();

                stock4ReqMktDataId = NumberGenerator.NextRandomInt();
                clientSocket.reqMktData(stock4ReqMktDataId, stock4Contract, "", false, false, null);
            }

        }

        public void OnContractDetailsEnd(int reqId)
        {
            //IO.ShowMessage("OnContractDetailsEnd. Req Id: " + reqId);
        }

        #endregion
    }
}
