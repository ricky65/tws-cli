﻿using IBApi;
using System;
using System.Collections.Generic;
using TradeBot.Events;
using TradeBot.Extensions;
using TradeBot.FileIO;
using TradeBot.Generated;
using TradeBot.Gui;
using TradeBot.MenuFramework;
using TradeBot.TwsAbstractions;
using TradeBot.Utils;
using static TradeBot.GlobalProperties;
using static TradeBot.Gui.Window;

namespace TradeBot
{
    public class TradeBotConsole
    {
        public static void Main(string[] args)
        {
            TradeBotConsole console = new TradeBotConsole();
            console.Start();
        }

        private readonly IList<int> ignoredErrorCodes = new List<int>()
        {
            ErrorCodes.MARKET_DATA_FARM_DISCONNECTED,
            ErrorCodes.MARKET_DATA_FARM_CONNECTED,
            ErrorCodes.HISTORICAL_DATA_FARM_DISCONNECTED,
            ErrorCodes.HISTORICAL_DATA_FARM_CONNECTED,
            ErrorCodes.HISTORICAL_DATA_FARM_INACTIVE,
            ErrorCodes.MARKET_DATA_FARM_INACTIVE
        };

        private TradeBotService service;
        private Menu menu;

        private bool shouldExitApplication;

        public TradeBotConsole()
        {
            service = new TradeBotService();

            InitializeEventHandlers();
            InitializeConsole();
            InitializeMenu();
        }

        #region Initialization
        private void InitializeEventHandlers()
        {
            Window.SetWindowCloseHandler(OnWindowClose);

            service.PropertyValueChanged += OnPropertyValueChanged;
            service.ConnectionClosed += OnConnectionClosed;
            service.TickDataUpdated += OnTickDataUpdated;
            service.ErrorOccured += OnError;
        }

        private void InitializeConsole()
        {
            Console.Title = Messages.AppName;
            // Set the console buffer height to the maximum allowed value.
            Console.BufferHeight = Int16.MaxValue - 1;
            if (Preferences.CenterWindow)
            {
                Window.SetWindowSizeAndCenter(
                    Preferences.WindowWidth,
                    Preferences.WindowHeight);
            }
        }

        private void InitializeMenu()
        {
            menu = new Menu();

            Action<IList<string>, Action> addMenuOption = (entry, command)
                => menu.AddMenuOption(entry[0], entry[1], command);

            MenuOptionEntries entries = Messages.MenuOptionEntries;
            addMenuOption(entries.ReloadSavedState, ReloadSavedStateCommand);
            addMenuOption(entries.SetTickerSymbol, SetTickerSymbolDataCommand);
            addMenuOption(entries.ClearTickerSymbol, ClearTickerSymbolCommand);
            addMenuOption(entries.SetStepSize, SetStepSizeCommand);
            addMenuOption(entries.SetStepSizeFromCash, SetStepSizeFromCashCommand);
            addMenuOption(entries.Buy, BuyCommand);
            addMenuOption(entries.Sell, SellCommand);
            addMenuOption(entries.ReversePosition, ReversePositionCommand);
            addMenuOption(entries.ClosePosition, ClosePositionCommand);
            addMenuOption(entries.ListAllPositions, ListAllPositionsCommand);
            addMenuOption(entries.Misc, MiscCommand);
            addMenuOption(entries.ClearScreen, ClearScreenCommand);
            addMenuOption(entries.Help, HelpCommand);
            addMenuOption(entries.ExitApplication, ExitApplicationCommand);
        }
        #endregion

        #region Public methods
        public void Start()
        {
            IO.ShowMessage(Messages.WelcomeMessage);

            try
            {
                service.Connect();
                LoadState();
                while (!shouldExitApplication)
                {
                    menu.PromptForMenuOption().Command();
                }
            }
            catch (Exception e)
            {
                IO.ShowMessage(e.Message, MessageType.ERROR);
            }
            finally
            {
                Shutdown();
                if (OS.IsWindows())
                {
                    IO.PromptForChar(Messages.PressAnyKeyToExit);
                }
            }
        }
        #endregion

        #region Menu commands
        private void ReloadSavedStateCommand()
        {
            LoadState();
        }

        private void SetTickerSymbolDataCommand()
        {
            string tickerSymbol = IO.PromptForInput(Messages.SelectTickerPrompt);
            IfNotNullOrWhiteSpace(tickerSymbol)(() =>
            {
                service.TickerSymbol = tickerSymbol;
            });
        }

        private void ClearTickerSymbolCommand()
        {
            service.TickerSymbol = null;
        }

        private void SetStepSizeCommand()
        {
            string stepSizeString = IO.PromptForInput(Messages.StepSizePrompt);
            int? stepSize = stepSizeString.ToInt();
            IfHasValue(stepSize)(() =>
            {
                service.StepSize = stepSize.Value;
            });
        }

        private void SetStepSizeFromCashCommand()
        {
            Validator[] validators = { IfTickerSet(), IfPriceDataAvailable() };
            Validate(validators, () =>
            {
                string cashString = IO.PromptForInput(Messages.StepSizeFromCashPrompt);
                double? cash = cashString.ToDouble();
                IfHasValue(cash)(() =>
                {
                    double sharePrice = Tick(TickType.LAST);
                    service.StepSize = StockMath.CalculateStepSizeFromCashValue(cash.Value, sharePrice);
                });
            });
        }

        private void BuyCommand()
        {
            Validator[] validators = { IfTickerSet(), IfStepSizeSet(), IfPriceDataAvailable() };
            Validate(validators, () =>
            {
                service.PlaceOrder(OrderActions.BUY, service.StepSize, Tick(TickType.ASK));
            });
        }

        private void SellCommand()
        {
            Validator[] validators = { IfTickerSet(), IfStepSizeSet(), IfPriceDataAvailable() };
            Validate(validators, () =>
            {
                service.PlaceOrder(OrderActions.SELL, service.StepSize, Tick(TickType.BID));
            });
        }

        private void ReversePositionCommand()
        {
            Validator[] validators = { IfTickerSet(), IfPriceDataAvailable() };
            Validate(validators, () =>
            {

            });
        }

        private void ClosePositionCommand()
        {
            Validator[] validators = { IfTickerSet(), IfPriceDataAvailable() };
            Validate(validators, () =>
            {
            });
        }

        private async void ListAllPositionsCommand()
        {
            IList<PositionInfo> positions = await service.GetAllPositionsForAllAccounts();
            foreach (var position in positions)
            {
                IO.ShowMessage(Messages.AllPositionsFormat, position.PositionSize, position.Contract.Symbol, position.Account);
            }
        }

        private void MiscCommand()
        {
        }

        private void ClearScreenCommand()
        {
            Console.Clear();
        }

        private void HelpCommand()
        {
            IO.ShowMessage(menu.ToString());
        }

        private void ExitApplicationCommand()
        {
            shouldExitApplication = true;
        }
        #endregion

        #region Event handlers
        private bool OnWindowClose(CloseReason reason)
        {
            Shutdown();

            // return false since we didn't handle the control signal, 
            // i.e. Environment.Exit(-1);
            return false;
        }

        private void OnPropertyValueChanged(object sender, PropertyValueChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(service.ManagedAccounts):
                    OnManagedAccountsChanged(eventArgs);
                    break;
                case nameof(service.TickerSymbol):
                    OnTickerChanged(eventArgs);
                    break;
                case nameof(service.StepSize):
                    OnStepSizeChanged(eventArgs);
                    break;
            }
        }

        private void OnManagedAccountsChanged(PropertyValueChangedEventArgs eventArgs)
        {
            string[] accounts = eventArgs.NewValue as string[];
            if (accounts != null && accounts.Length > 0)
            {
                string tradedAccount = accounts[0];
                service.TradedAccount = tradedAccount;

                if (accounts.Length > 1)
                {
                    IO.ShowMessage(Messages.MultipleAccountsWarningFormat, MessageType.WARNING, tradedAccount);
                }

                if (tradedAccount.StartsWith(Messages.PaperAccountPrefix, StringComparison.InvariantCulture))
                {
                    IO.ShowMessage(Messages.AccountTypePaper, MessageType.SUCCESS);
                }
                else
                {
                    IO.ShowMessage(Messages.AccountTypeLive, MessageType.WARNING);
                }
            }
        }

        private void OnTickerChanged(PropertyValueChangedEventArgs eventArgs)
        {
            string oldValue = eventArgs.OldValue as string;
            if (!string.IsNullOrWhiteSpace(oldValue))
            {
                IO.ShowMessage(Messages.TickerSymbolClearedFormat, oldValue);
            }
            string newValue = eventArgs.NewValue as string;
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                IO.ShowMessage(Messages.TickerSymbolSetFormat, newValue);
            }

            UpdateConsoleTitle();
        }

        private void OnStepSizeChanged(PropertyValueChangedEventArgs eventArgs)
        {
            IO.ShowMessage(Messages.StepSizeSetFormat, eventArgs.NewValue);

            UpdateConsoleTitle();
        }

        private void OnTickDataUpdated()
        {
            UpdateConsoleTitle();
        }

        private void OnConnectionClosed()
        {
            IO.ShowMessage(Messages.TwsDisconnectedError, MessageType.ERROR);
        }

        private void OnError(int id, int errorCode, string errorMessage)
        {
            if (ignoredErrorCodes.Contains(errorCode))
            {
                return;
            }

            IO.ShowMessage(Messages.TwsErrorFormat, MessageType.ERROR, errorMessage);
        }
        #endregion

        #region Private helper methods
        private void Shutdown()
        {
            service.Disconnect();
            SaveState();
        }

        private void SaveState()
        {
            service.SaveState();
            IO.ShowMessage(Messages.SavedStateFormat, MessageType.SUCCESS, PropertyFiles.STATE_FILE);
        }

        private void LoadState()
        {
            IO.ShowMessage(Messages.LoadedStateFormat, MessageType.SUCCESS, PropertyFiles.STATE_FILE);
            service.LoadState();
        }

        private void UpdateConsoleTitle()
        {
            string appName = Messages.AppName;
            IList<string> infoStrings = new List<string>();
            if (!string.IsNullOrWhiteSpace(appName))
            {
                infoStrings.Add(appName);
            }
            infoStrings.Add(string.Format(Messages.TitleTickerSymbol, service.TickerSymbol ?? Messages.TitleTickerSymbolNotSelected));
            infoStrings.Add(string.Format(Messages.TitleStepSize, service.StepSize));
            infoStrings.Add(string.Format(Messages.TitleLastFormat, Tick(TickType.LAST)));
            infoStrings.Add(string.Format(Messages.TitleBidAskFormat, Tick(TickType.BID), Tick(TickType.ASK)));
            infoStrings.Add(string.Format(Messages.TitleVolumeFormat, Tick(TickType.VOLUME)));
            infoStrings.Add(string.Format(Messages.TitleCloseFormat, Tick(TickType.CLOSE)));
            infoStrings.Add(string.Format(Messages.TitleOpenFormat, Tick(TickType.OPEN)));
            Console.Title = string.Join(Messages.TitleDivider, infoStrings);
        }

        private double Tick(int tickType)
        {
            return service.GetTickData(tickType);
        }
        #endregion

        #region Validations
        private delegate bool Validation();
        private delegate bool Validator(Action ifValidCallback);

        private void Validate(Validator[] validators, Action ifValidCallback)
        {
            bool valid = false;
            foreach (var validator in validators)
            {
                valid &= validator(null);
            }
            if (valid)
            {
                ifValidCallback();
            }
        }

        private Validator IfTickerSet()
        {
            return CreateValidator(
                () => service.TickerSymbol != null,
                Messages.TickerSymbolNotSetError);
        }

        private Validator IfStepSizeSet()
        {
            return CreateValidator(
                () => service.StepSize > 0,
                Messages.StepSizeNotSetError);
        }

        private Validator IfPriceDataAvailable()
        {
            Func<int, bool> ifAvailable = (tickType)
                => Tick(tickType) > 0;

            return CreateValidator(
                () => ifAvailable(TickType.LAST) && ifAvailable(TickType.ASK) && ifAvailable(TickType.BID),
                Messages.PriceDataUnavailableError);
        }

        private Validator IfNotNullOrWhiteSpace(string str)
        {
            return CreateValidator(
                () => !string.IsNullOrWhiteSpace(str),
                Messages.InvalidNonEmptyStringInputError);
        }

        private Validator IfHasValue(int? nullable)
        {
            return CreateValidator(
                () => nullable.HasValue,
                Messages.InvalidIntegerInputError);
        }

        private Validator IfHasValue(double? nullable)
        {
            return CreateValidator(
                () => nullable.HasValue,
                Messages.InvalidDecimalInputError);
        }

        private Validator CreateValidator(Validation validation, string errorMessage)
        {
            return (ifValidCallback) =>
            {
                bool valid = validation();
                if (valid)
                {
                    ifValidCallback?.Invoke();
                }
                else
                {
                    IO.ShowMessage(errorMessage, MessageType.VALIDATION_ERROR);
                }
                return valid;
            };
        }
        #endregion
    }
}
