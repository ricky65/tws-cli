using IBApi;

namespace TradeBot.TwsAbstractions
{
    public static class OrderFactory
    {
        public static Order CreateLimitOrder(OrderActions action, double quantity, double limitPrice, bool transmitNow)
        {
            return new Order()
            {
                Action = action.ToString(),
                TotalQuantity = quantity,
                LmtPrice = limitPrice,
                OrderType = OrderTypes.LMT.ToString(),
                Tif = TimeInForce.GTC.ToString(),
                Transmit = transmitNow, //Rick: only last child in the bracket has Transmit = true
                OutsideRth = false
            };
        }

        //Rick
        public static Order CreateStopLimitOrder(OrderActions action, double quantity, double limitPrice, double stopPrice)
        {
            return new Order()
            {
                Action = action.ToString(),
                TotalQuantity = quantity,
                LmtPrice = limitPrice,
                AuxPrice = stopPrice,
                OrderType = "STP LMT",
                Tif = TimeInForce.GTC.ToString(),
                Transmit = false, //Rick: only last child in the bracket has Transmit = true
                TriggerMethod = 7, //Rick: 7 is last or bid/ask
                OutsideRth = false               
            };
        }
        //Rick
        public static Order CreateStopOrder(OrderActions action, double quantity,double stopPrice)
        {
            return new Order()
            {
                Action = action.ToString(),
                TotalQuantity = quantity,
                AuxPrice = stopPrice,
                OrderType = "STP",
                Tif = TimeInForce.GTC.ToString(),
                Transmit = true, //Rick: only last child in the bracket has Transmit = true
                TriggerMethod = 7, //Rick: 7 is last or bid/ask
                OutsideRth = false
            };
        }
    }
}
