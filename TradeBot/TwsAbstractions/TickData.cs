﻿using System.Collections.Generic;

namespace TradeBot.TwsAbstractions
{
    public class TickData
    {
        private IDictionary<int, double> data;

        public TickData()
        {
            data = new Dictionary<int, double>();
        }

        public double this[int field]
        {
            get
            {
                return data.ContainsKey(field)
                    ? data[field] : -1;
            }
            set
            {
                if (data.ContainsKey(field))
                {
                    data[field] = value;
                }
                else
                {
                    data.Add(field, value);
                }
            }
        }
    }
}