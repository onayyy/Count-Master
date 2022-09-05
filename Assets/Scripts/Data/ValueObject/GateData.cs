using System;
using Enums;

namespace Data.ValueObject
{
    [Serializable]
    public class GateData
    {
        public Symbols SymbolType;
        public int GateValue = 10;
        public string GateValueText; 
    }
}