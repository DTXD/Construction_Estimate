using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Du_Toan_Xay_Dung.Models
{
    public class UserWorkViewModel
    {
        public UserWorkViewModel() { }
        public UserWorkViewModel(UserWork obj)
        {
            BuildingItem_ID = obj.BuildingItem_ID;
            Sub_BuildingItem_ID = obj.Sub_BuildingItem_ID;
            IndexSheet = obj.IndexSheet;
            ID = obj.ID;
            NormWork_ID = obj.NormWork_ID;
            Name = obj.Name;
            Unit = obj.Unit;
            Number = obj.Number;
            Horizontal = obj.Horizontal;
            Vertical = obj.Vertical;
            Height = obj.Height;
            Area = obj.Area;
            PriceMaterial = obj.PriceMaterial;
            PriceLabor = obj.PriceLabor;
            PriceMachine = obj.PriceMachine;
            SumMaterial = obj.SumMaterial;
            SumLabor = obj.SumLabor;
            SumMachine = obj.SumMachine;

        }
        public long BuildingItem_ID { get; set; }
        public long Sub_BuildingItem_ID { get; set; }
        public long IndexSheet { get; set; }
        public string ID { get; set; }
        public string NormWork_ID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal? Number { get; set; }
        public decimal? Horizontal { get; set; }
        public decimal? Vertical { get; set; }
        public decimal? Height { get; set; }
        public decimal? Area { get; set; }
        public decimal? PriceMaterial { get; set; }
        public decimal? PriceLabor { get; set; }
        public decimal? PriceMachine { get; set; }
        public decimal? SumMaterial { get; set; }
        public decimal? SumLabor { get; set; }
        public decimal? SumMachine { get; set; }

    }
}