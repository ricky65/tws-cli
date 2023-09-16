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
                SecType = SecurityTypes.STK.ToString(), 
                Currency = Currencies.USD.ToString(),
                Exchange = Exchanges.SMART.ToString()
            };
        }

        //rick
        public static Contract CreateCFDContract(string CFDtickerSymbol)
        {
            return new Contract()
            {
                Symbol = CFDtickerSymbol.ToUpper(),
                SecType = SecurityTypes.CFD.ToString(),
                Currency = Currencies.USD.ToString(),
                Exchange = Exchanges.SMART.ToString()
            };
        }
    }
}