﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Infrastructure.Abstractions;

namespace TradeX.Infrastructure.Services
{
    internal class RandomNumberProvider : IRandomNumberProvider
    {
        private readonly Random _random = new();
        private readonly List<int> _directions = [1, -1];

        private const decimal minMagnitude = 1;
        private const decimal maxMagnitude = 2;

        public int GetDirection()
        {
            return _directions[_random.Next(0, _directions.Count)];
        }

        public decimal GetMagnitude()
        {
            return (decimal)_random.NextDouble() * (maxMagnitude - minMagnitude) + minMagnitude;
        }
    }
}
