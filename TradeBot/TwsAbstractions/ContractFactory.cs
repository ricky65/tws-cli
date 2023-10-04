using IBApi;

namespace TradeBot.TwsAbstractions
{
    public static class ContractFactory
    {
        public static Contract CreateStockContract(string tickerSymbol)
        {
            return new Contract()
            {
                Symbol = tickerSymbol.ToUpper(),
                SecType = nameof(SecurityTypes.STK), 
                Currency = nameof(Currencies.USD),
                Exchange = nameof(Exchanges.SMART)
            };
        }

        //rick
        public static Contract CreateCFDContract(string CFDtickerSymbol)
        {
            return new Contract()
            {
                Symbol = CFDtickerSymbol.ToUpper(),
                SecType = nameof(SecurityTypes.CFD),
                Currency = nameof(Currencies.USD),
                Exchange = nameof(Exchanges.SMART)
            };
        }

        public static Contract CreateCMEFuturesContract(string futuresSymbol)
        {
            //Rick: Will request the earliest expiring contract for an index e.g. M2K/MNQ/MES
            return new Contract()
            {
                Symbol = futuresSymbol.ToUpper(),
                SecType = nameof(SecurityTypes.FUT),
                Currency = nameof(Currencies.USD),
                Exchange = nameof(Exchanges.CME)
            };
        }
    }
}