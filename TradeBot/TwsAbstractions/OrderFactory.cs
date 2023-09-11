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
                OrderType = OrderTypes.Limit,
                Tif = TimeInForce.DAY.ToString(),
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
                OrderType = OrderTypes.StopLimit,
                Tif = TimeInForce.DAY.ToString(),
                Transmit = false, //Rick: only last child in the bracket has Transmit = true
                TriggerMethod = (int)TriggerMethod.BidAsk, 
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
                OrderType = OrderTypes.Stop,
                Tif = TimeInForce.DAY.ToString(),
                Transmit = true, //Rick: only last child in the bracket has Transmit = true
                TriggerMethod = (int)TriggerMethod.BidAsk, 
                OutsideRth = false
            };
        }
    }
}
