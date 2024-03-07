using NLog;
using System.Collections.Generic;
using TradeBot.Extensions;
using TradeBot.Gui;
using TradeBot.TwsAbstractions;
using static TradeBot.AppProperties;

namespace TradeBot
{
    public static class Validation
    {
        public static bool TickerSet(TradeService service)
        {
            return Validate(
                service.HasStock1TickerSymbol,
                Messages.TickerSymbolNotSetError);
        }

        public static bool TickerSetGUI(int stockNum, TradeService service)
        {
            //bool tickerSet = false;

            if (stockNum == 1)
                return service.HasStock1TickerSymbol;
            else if (stockNum == 2)
                return service.HasStock2TickerSymbol;
            else if (stockNum == 3)
                return service.HasStock3TickerSymbol;
            else if (stockNum == 4)
                return service.HasStock4TickerSymbol;

            //should never reach here
            return false;

            //return Validate(tickerSet, Messages.TickerSymbolNotSetError); //was used to give error message in console
        }

        public static bool SharesSet(double shares)
        {
            return Validate(
                shares > 0,
                Messages.SharesNotSetError);
        }

        public static bool TickDataAvailable(TradeService service, params int[] tickTypes)
        {
            return Validate(
                service.HasTicks(tickTypes),
                Messages.PriceDataUnavailableError);
        }
        public static bool TickDataAvailableGUI(int stockNum, TradeService service, params int[] tickTypes)
        {
            return Validate(
                service.HasTicks(stockNum, tickTypes),
                Messages.PriceDataUnavailableError);
        }

        public static bool PositionExists(Position position)
        {
            return Validate(
                position != null,
                Messages.PositionNotFoundError);
        }

        public static bool PositionsExist(IEnumerable<Position> positions)
        {
            return Validate(
                !positions.IsNullOrEmpty(),
                Messages.PositionsNotFoundError);
        }

        public static bool NotNullOrWhiteSpace(string str)
        {
            return Validate(
                !string.IsNullOrWhiteSpace(str),
                Messages.InvalidNonEmptyStringInputError);
        }

        public static bool HasValue(int? value)
        {
            return Validate(
                value.HasValue,
                Messages.InvalidIntegerInputError);
        }

        public static bool HasValue(double? value)
        {
            return Validate(
                value.HasValue,
                Messages.InvalidDecimalInputError);
        }

        public static bool Positive(int value)
        {
            return Validate(
                value >= 0,
                Messages.InvalidPositiveInputError);
        }

        public static bool Positive(double value)
        {
            return Validate(
                value >= 0,
                Messages.InvalidPositiveInputError);
        }

        private static bool Validate(bool isValid, string errorMessage)
        {
            if (!isValid)
            {                
                IO.ShowMessageCLI(LogLevel.Error, errorMessage);
                return false;
            }

            return true;
        }
    }
}
