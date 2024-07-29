using IBApi;

namespace TradeBot.TwsAbstractions
{
    public static class OrderFactory
    {
        public static Order CreateLimitOrder(OrderActions action, double quantity, double limitPrice, bool transmitNow, bool outsideRth)
        {
            return new Order()
            {
                Action = action.ToString(),
                TotalQuantity = quantity,
                LmtPrice = limitPrice,
                OrderType = OrderTypes.Limit,
                Tif = nameof(TimeInForce.GTC),
                Transmit = transmitNow, //Rick: only last child in the bracket has Transmit = true
                OutsideRth = outsideRth
            };
        }

        //Rick
        public static Order CreateStopLimitOrder(OrderActions action, double quantity, double limitPrice, double stopPrice, bool transmitNow, bool outsideRth)
        {
            return new Order()
            {
                Action = action.ToString(),
                TotalQuantity = quantity,
                LmtPrice = limitPrice,
                AuxPrice = stopPrice,
                OrderType = OrderTypes.StopLimit,
                Tif = nameof(TimeInForce.GTC),
                Transmit = transmitNow, //Rick: only last child in the bracket has Transmit = true
                TriggerMethod = (int)TriggerMethod.LastOfBidAsk, 
                OutsideRth = outsideRth
            };
        }
        //Rick
        public static Order CreateStopOrder(OrderActions action, double quantity, double stopPrice, bool transmitNow)
        {
            return new Order()
            {
                Action = action.ToString(),
                TotalQuantity = quantity,
                AuxPrice = stopPrice,
                OrderType = OrderTypes.Stop,
                Tif = nameof(TimeInForce.GTC),
                Transmit = transmitNow, //Rick: only last child in the bracket has Transmit = true
                TriggerMethod = (int)TriggerMethod.LastOfBidAsk, 
                OutsideRth = false
            };
        }
    }
}
