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
    }
}