using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using TradeBot.Generated;
using TradeBot.Gui;
using TradeBot.MenuFramework;
using static TradeBot.AppProperties;

namespace TradeBot
{
    public class TradeMenu : Menu
    {
        public TradeMenu(TradeController controller)
        {
            InitMenu(controller);
        }

        private void InitMenu(TradeController controller)
        {
            var titleDivider = new MenuDivider();
            AddMenuItem(new MenuTitle(Messages.MenuTitle, titleDivider));

            var addMenuOption = new Action<IList<string>, MenuCommand>((entry, command)
                => AddMenuItem(new MenuOption(entry[0], entry[1], command)));

            var menuOptionDivider = new MenuDivider();
            var addMenuOptionDivider = new Action(()
                => AddMenuItem(menuOptionDivider));

            MenuOptionEntries entries = Messages.MenuOptionEntries;

            addMenuOption(entries.SetTickerSymbol, controller.PromptForTickerSymbolCommand);
            addMenuOption(entries.SetCFDTickerSymbol, controller.PromptForCFDtickerSymbolCommand);
            addMenuOptionDivider();

            addMenuOption(entries.SetSharesFromCash, controller.PromptForCashCommand);
            addMenuOption(entries.SetShares, controller.PromptForSharesCommand);
            addMenuOption(entries.SetSharesFromPosition, controller.SetSharesFromPositionCommand);
            addMenuOptionDivider();

            addMenuOption(entries.Buy, controller.BuyCommand);

            //Rick
            addMenuOption(entries.BuyStop, controller.BuyStopCommand);
            addMenuOption(entries.SellStop, controller.SellStopCommand);

            addMenuOption(entries.Sell, controller.SellCommand);
            addMenuOption(entries.ReversePosition, controller.ReversePositionCommand);
            addMenuOption(entries.ClosePosition, controller.ClosePositionCommand);
            //Rick
            addMenuOption(entries.CloseHalfPosition, controller.CloseHalfPositionCommand);
            addMenuOption(entries.CloseThirdPosition, controller.CloseThirdPositionCommand);
            addMenuOption(entries.CloseTwoThirdsPosition, controller.CloseTwoThirdsPositionCommand);
            addMenuOptionDivider();
            //Rick
            addMenuOption(entries.LimitTakeProfitHalf, controller.LimitTakeProfitHalfCommand);
            addMenuOption(entries.LimitTakeProfitThird, controller.LimitTakeProfitThirdCommand);
            addMenuOptionDivider();

            addMenuOption(entries.ListPositions, controller.ListPositionsCommand);
            addMenuOptionDivider();

            addMenuOption(entries.SetRisk, controller.SetRisk);
            addMenuOption(entries.SetEquity, controller.SetEquity);
            addMenuOptionDivider();

            addMenuOption(entries.LoadState, controller.LoadStateCommand);
            addMenuOption(entries.SaveState, controller.SaveStateCommand);
            addMenuOptionDivider();

            addMenuOption(entries.ClearScreen, controller.ClearScreenCommand);
            addMenuOption(entries.ShowMenu, controller.ShowMenuCommand);

            var menuEndDivider = new MenuDivider();
            AddMenuItem(menuEndDivider);

            int dividerLength = GetLongestMenuEntryLength();
            var createDividerString = new Func<string, string>((charString) =>
            {
                return !string.IsNullOrEmpty(charString)
                    ? new string(charString.First(), dividerLength)
                    : string.Empty;
            });
            titleDivider.DividerString = createDividerString(Messages.MenuTitleDividerChar);
            menuOptionDivider.DividerString = createDividerString(Messages.MenuOptionDividerChar);
            menuEndDivider.DividerString = createDividerString(Messages.MenuEndDividerChar);
        }

        public async Task Run()
        {
            string[] input = PromptForMenuOptionInput();
            await HandleMenuOptionInputAsync(input);
        }

        private string[] PromptForMenuOptionInput()
        {
            return IO.PromptForInput().Split();
        }

        private async Task HandleMenuOptionInputAsync(string[] input)
        {
            string key = input.FirstOrDefault();
            MenuOption menuOption = getMenuOption(key);
            if (menuOption != null)
            {
                string[] args = input.Skip(1).ToArray();
                await menuOption.Command(args);
            }
            else
            {
                IO.ShowMessage(LogLevel.Error, Messages.InvalidMenuOption);
            }
        }

    }
}
