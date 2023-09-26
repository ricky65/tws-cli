using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace TradeBot.TwsAbstractions
{
    public class Portfolio : Dictionary<string, Position>
    {
        public Position Get(string tickerSymbol)
        {
            if (tickerSymbol == null)
            {
                return null;
            }

            Position position;
            TryGetValue(tickerSymbol, out position);
            return position;
        }

        public async void Update(Position position, TradeService ts)
        {
            string tickerSymbol = position.Symbol;
            if (position.PositionSize == 0)
            {
                //Rick: Check if any active orders (Stop Loss/Limit Take Profit) and cancel them
                ts.openOrdersDict.Clear();
                ts.orderStatusDict.Clear();

                ts.clientSocket.reqOpenOrders(); // Bind previous opened orders

                //ts.openOrderEndTCS = new TaskCompletionSource();

                //await ts.WaitForOpenOrderEnd();

                //var openOrdersForTicker = ts.openOrdersDict.Values.Where(o => o.Symbol == tickerSymbol);

                //foreach (OpenOrder openOrder in openOrdersForTicker)
                //{
                //    ts.CancelOrder(openOrder.OrderId);
                //}

                Remove(tickerSymbol);
            }            
            else
            {
                if (ContainsKey(tickerSymbol))
                {
                    //Rick: TODO: Test this Monday morning
                    //double absNewPosSize = Math.Abs(position.PositionSize);
                    //if (absNewPosSize < Math.Abs(this[tickerSymbol].PositionSize))
                    //{
                    //    //Rick: New position size is now less than previous position size (part of position closed) so reduce any active orders (Stop Loss/Limit Take Profit) > New Pos Size to the New Pos Size
                        ts.openOrdersDict.Clear();
                        ts.orderStatusDict.Clear();

                        ts.clientSocket.reqOpenOrders(); // Bind previous opened orders

                    //    ts.openOrderEndTCS = new TaskCompletionSource();

                    //    await ts.WaitForOpenOrderEnd();

                    //    var openOrdersForTicker = ts.openOrdersDict.Values.Where(o => o.Symbol == tickerSymbol);

                    //    foreach (OpenOrder openOrder in openOrdersForTicker)
                    //    {
                    //        if (openOrder.Order.TotalQuantity > absNewPosSize) { 
                    //            openOrder.Order.TotalQuantity = absNewPosSize;
                    //            openOrder.Order.ParentId = 0;//Rick: Need to set ParentId to 0 as Stop was probably a child 
                    //            ts.ModifyOrder(openOrder.OrderId, openOrder.Contract, openOrder.Order);
                    //        }
                    //    }      
                    //}

                    this[tickerSymbol] = position;
                    
                }
                else
                {
                    Add(tickerSymbol, position);
                }
            }
        }
    }
}
